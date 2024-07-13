using System.Text.Json;
using Core.ItemService;

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

app.MapGet("/GetItemStreamTest", async (HttpContext context, ItemService service, CancellationToken ct) =>
{
    context.Response.Headers.Append("Content-Type", "text/event-stream");

    while (!ct.IsCancellationRequested)
    {
        // 送信するアイテムを待機
        var item = await service.WaitForNewItem();

        // 送信データを書き込み
        await context.Response.WriteAsync($"data: ");
        await JsonSerializer.SerializeAsync(context.Response.Body, item);
        await context.Response.WriteAsync($"\n\n");
        await context.Response.Body.FlushAsync();

        service.Reset();
    }

}).WithName("GetItemStreamTest").WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
