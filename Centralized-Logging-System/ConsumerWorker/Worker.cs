using ConsumerWorker.RabbitMQService;

namespace ConsumerWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEnumerable<RabbitMQConsumer> _rabbitMQConsumers;

        public Worker(ILogger<Worker> logger, IEnumerable<RabbitMQConsumer> rabbitMQConsumers)
        {
            _logger = logger;
            _rabbitMQConsumers = rabbitMQConsumers;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {


            while (!stoppingToken.IsCancellationRequested)
            {

                foreach (var consumer in _rabbitMQConsumers)
                {
                    _logger.LogInformation($"starting consuming  {consumer._queueName}");
                    consumer.Start();
                }
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var consumer in _rabbitMQConsumers)
            {
                consumer.Close();
            }
            return base.StopAsync(cancellationToken);
        }
    }
}
