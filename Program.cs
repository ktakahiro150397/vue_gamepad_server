using System.Net.Http.Json;
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

app.MapGet("/GetInputStream", async (int joyId, HttpContext context, CancellationToken ct) =>
{
    GamepadInput gamepadInput = new GamepadInput();

    context.Response.Headers.Append("Content-Type", "text/event-stream");
    while (!ct.IsCancellationRequested)
    {
        // 60Hzでゲームパッドの入力チェックを行う
        await Task.Delay(1000 / 60);

        gamepadInput.GetPadInput(joyId);

        if (((IGamePadInput)gamepadInput).IsInputChangeFromPreviousFrame)
        {
            // 送信データを現在の入力状態から作成
            var testResponse = new GetInputStreamResponse();

            testResponse.SetDirectionState(gamepadInput.joyInfo.dwPOV);
            testResponse.SetButtonState(gamepadInput.joyInfo.dwButtons);
            testResponse.time_stamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // 送信データを書き込み
            await context.Response.WriteAsync($"data: ");
            await JsonSerializer.SerializeAsync(context.Response.Body, testResponse);
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
