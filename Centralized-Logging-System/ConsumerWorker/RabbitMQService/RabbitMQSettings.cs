using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerWorker.RabbitMQService
{
    public class RabbitMQSettings
    {
        public string HostName { get; set; } = string.Empty;
        public List<RabbitMQConsumerSettings> Consumers { get; set; } = new List<RabbitMQConsumerSettings>();
        public string ElasticsearchUrl { get; set; } = string.Empty;
    }

}
