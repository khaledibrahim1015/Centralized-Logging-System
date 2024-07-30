using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingQueuePublisher.Services
{
    public class RabbitMQHostedService : IHostedService
    {
        private readonly RabbitMQService _rabbitMqService;
        private readonly ILogger<RabbitMQHostedService> _logger;

        public RabbitMQHostedService(RabbitMQService rabbitMqService, ILogger<RabbitMQHostedService> logger)
        {
            _rabbitMqService = rabbitMqService;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _rabbitMqService.CreateQueue();
            _logger.LogInformation("Create Queue Successfully !");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
