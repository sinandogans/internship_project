using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace ServiceLayer.Utilities.MessageBroker.Kafka
{
    public class KafkaPublisherHelper
    {
        private readonly IConfiguration _configuration;

        public KafkaPublisherHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void PublishMessage<T>(string topic, T message)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                ClientId = Dns.GetHostName(),
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                producer.ProduceAsync(topic, new Message<Null, string> { Value = message.ToString() });
                producer.Flush();
            }
        }
    }
}
