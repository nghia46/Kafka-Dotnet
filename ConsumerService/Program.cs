using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton(new ConsumerConfig
{
    BootstrapServers = builder.Configuration["Kafka:BootstrapServers"],
    GroupId = "consumer-group-1",
    AutoOffsetReset = AutoOffsetReset.Earliest
});
builder.Services.AddHostedService<KafkaConsumerService>();
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
