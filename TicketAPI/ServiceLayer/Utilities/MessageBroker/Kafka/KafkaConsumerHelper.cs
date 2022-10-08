using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace ServiceLayer.Utilities.MessageBroker.Kafka
{
    public class KafkaConsumerHelper
    {
        private readonly IConfiguration _configuration;

        public KafkaConsumerHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConsumeMessage(string topic, string groupId)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(topic);
                var result = consumer.Consume();
                consumer.Close();
                return result.Value;
            }
        }
    }
}
