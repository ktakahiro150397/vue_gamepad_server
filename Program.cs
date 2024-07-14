using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core.ItemService;
using Core.PadInput;
using PadInput.GamePadInput;
using PadInput.Win32Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ItemService>();
builder.Services.AddSingleton<TestPadInputService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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

app.MapGet("/GetItemStreamTest", async (HttpContext context, TestPadInputService service, CancellationToken ct) =>
{
    context.Response.Headers.Append("Content-Type", "text/event-stream");

    while (!ct.IsCancellationRequested)
    {
        // 送信するアイテムを待機
        var item = await service.WaitForPadInput();

        // 送信データを書き込み
        await context.Response.WriteAsync($"data: ");
        await JsonSerializer.SerializeAsync(context.Response.Body, item);
        await context.Response.WriteAsync($"\n\n");
        await context.Response.Body.FlushAsync();

        service.TaskReset();
    }

}).WithName("GetItemStreamTest").WithOpenApi();

app.MapGet("/GetInputStream", async (int joyId, HttpContext context, TestPadInputService service, CancellationToken ct) =>
{
    GamepadInput gamepadInput = new GamepadInput();

    context.Response.Headers.Append("Content-Type", "text/event-stream");
    while (!ct.IsCancellationRequested)
    {
        // 60Hzでゲームパッドの入力チェックを行う
        await Task.Delay(1000 / 60);

        gamepadInput.GetPadInput(joyId);

        // TODO : dwPosは角度に100を乗じた数で、その角度で方向キー入力方向が検知できる
        // 8 -> 0 / 6 -> 9000 / 2 -> 18000 / 4 -> 27000
        // 十字キー前提の場合、それぞれの値と一体するかどうかを確認すればよさげ

        if (((IGamePadInput)gamepadInput).IsInputChangeFromPreviousFrame)
        {
            // 送信データを書き込み
            await context.Response.WriteAsync($"data: ");
            await JsonSerializer.SerializeAsync(context.Response.Body, gamepadInput.joyInfo);
            await context.Response.WriteAsync($"\n\n");
            await context.Response.Body.FlushAsync();
        }

    }
}).WithName("GetInputStream").WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
