using LoggingQueuePublisher.Configuration;
using LoggingQueuePublisher.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingQueuePublisher.Factory
{
    public class RabbitMqPublisherFactory
    {

        internal readonly RabbitMqClientConfig _config;
        public RabbitMqPublisherFactory(IOptions<RabbitMqClientConfig> config)
        {
            _config = config.Value;
        }

        public RabbitMQService CreateRabbitMQService()
        {
            return new RabbitMQService(_config.HostName,
                                        _config.RouteKey
                                        , _config.Port
                                          , _config.UserName
                                            , _config.Password);

        }

    }
}
