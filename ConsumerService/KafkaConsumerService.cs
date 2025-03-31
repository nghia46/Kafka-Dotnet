using Confluent.Kafka;

public class KafkaConsumerService(ILogger<KafkaConsumerService> logger, ConsumerConfig config) : BackgroundService
{

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe("test-topic");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    logger.LogInformation(
                        "Consumed message '{Value}' at: '{TopicPartitionOffset}'. Timestamp: {Timestamp}",
                        result.Message.Value,
                        result.TopicPartitionOffset,
                        result.Message.Timestamp.UtcDateTime);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error consuming Kafka message");
                }
            }

            consumer.Close();
        }, stoppingToken);
    }
}