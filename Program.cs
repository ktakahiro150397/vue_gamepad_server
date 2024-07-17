using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core.ItemService;
using Core.PadInput;
using Core.Response;
using PadInput.GamePadInput;
using PadInput.Win32Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ItemService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var ServerTicks = builder.Configuration.GetValue<int>("ServerTick");

// app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/GetInputStream", async (int joyId, HttpContext context, CancellationToken ct) =>
{
    uint tickRate = (uint)(1000 / ServerTicks);

    // 待機時間の精度を向上
    NativeMethods.timeBeginPeriod(1);

    GamepadInput gamepadInput = new GamepadInput();

    context.Response.Headers.Append("Content-Type", "text/event-stream");

    int inputFrameCount = 0;
    while (!ct.IsCancellationRequested)
    {
        // 60Hzでゲームパッドの入力チェックを行う
        await Task.Delay((int)tickRate);
        if (inputFrameCount < 99)
        {
            inputFrameCount++;
        }

        gamepadInput.GetPadInput(joyId);

        if (((IGamePadInput)gamepadInput).IsInputChangeFromPreviousFrame)
        {
            // 送信データを現在の入力状態から作成
            var testResponse = new GetInputStreamResponse();

            testResponse.SetDirectionState(gamepadInput.joyInfo.dwPOV);
            testResponse.SetButtonState(gamepadInput.joyInfo.dwButtons);
            testResponse.time_stamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            testResponse.previous_push_frame = inputFrameCount;
            inputFrameCount = 0; // 入力フレームを0にリセット

            // 送信データを書き込み
            await context.Response.WriteAsync($"data: ");
            await JsonSerializer.SerializeAsync(context.Response.Body, testResponse);
            await context.Response.WriteAsync($"\n\n");
            await context.Response.Body.FlushAsync();

        }

        // タイマー精度リセット
        NativeMethods.timeEndPeriod(1);
    }
}).WithName("GetInputStream").WithOpenApi();

app.MapGet("/GetDevices", async (HttpContext context) =>
{

    var ret = new List<GetDevicesResponse>();

    var maxDeviceCount = NativeMethods.joyGetNumDevs();
    for (int i = 0; i < maxDeviceCount; i++)
    {
        JOYCAPS caps = new JOYCAPS();
        var info = NativeMethods.joyGetDevCaps(i, ref caps, Marshal.SizeOf<JOYCAPS>());
        if (info == JoyGetPosExReturnValue.JOYERR_NOERROR)
        {
            var device = new GetDevicesResponse
            {
                joyId = i,
                device_name = caps.szPname,
                device_id = $"{caps.wMid}-{caps.wPid}",
                server_tick = ServerTicks
            };
            ret.Add(device);
        }
    }

    await JsonSerializer.SerializeAsync(context.Response.Body, ret);
    await context.Response.Body.FlushAsync();
}).WithName("GetDevices").WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
