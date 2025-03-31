using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Load configuration from appsettings.json
builder.Services.AddSingleton(new ProducerConfig{
    BootstrapServers = builder.Configuration["Kafka:BootstrapServers"],
    SocketTimeoutMs = 10000,
    MessageTimeoutMs = 10000,
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
