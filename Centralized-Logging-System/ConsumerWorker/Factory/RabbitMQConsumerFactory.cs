using ConsumerWorker.RabbitMQService;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerWorker.Factory
{
    public class RabbitMQConsumerFactory
    {
        private readonly RabbitMQSettings _settings;
        private readonly List<RabbitMQConsumer> _consumers;

        public RabbitMQConsumerFactory(IOptions<RabbitMQSettings> settings)
        {
            _settings = settings.Value;
            _consumers = new List<RabbitMQConsumer>();

            foreach (var consumerSettings in _settings.Consumers)
            {
                var consumer = new RabbitMQConsumer(
                    _settings.HostName,
                    consumerSettings.QueueName,
                    consumerSettings.ElasticIndexName,
                    consumerSettings.LogFilePath, true);
                _consumers.Add(consumer);
            }
        }

        public IEnumerable<RabbitMQConsumer> GetConsumers()
        {
            return _consumers;
        }


    }

}
