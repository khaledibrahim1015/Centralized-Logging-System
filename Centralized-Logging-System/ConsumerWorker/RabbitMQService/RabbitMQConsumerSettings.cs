using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerWorker.RabbitMQService
{
   
     public class RabbitMQConsumerSettings
    {
        public string QueueName { get; set; } = string.Empty;
        public string ElasticIndexName { get; set; } = string.Empty;
        public string LogFilePath { get; set; } = string.Empty;

    }

}
