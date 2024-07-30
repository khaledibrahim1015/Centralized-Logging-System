using ConsumerWorker.Helper.Custom;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerWorker.RabbitMQService
{
    public class RabbitMQConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public readonly string _appLogFilePath;
        public readonly string _elasticIndexName;
        public readonly string _queueName;
        private readonly Serilog.ILogger _logger;

        public RabbitMQConsumer(string hostName, string queueName, string elasticIndexName, string appLogFilePath, bool useCustomFormat = false)
        {
            _appLogFilePath = appLogFilePath;
            _elasticIndexName = elasticIndexName;
            _queueName = queueName;

            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                Port = 30505,
                UserName = "Dev",
                Password = "Dev@889765",
                VirtualHost = "/"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            _logger = new LoggerConfiguration()
                    .Enrich.With(new CustomEnricher(appLogFilePath))
                    .WriteTo.Logger(l => l
                        .WriteTo.File(formatter: useCustomFormat ? new CustomJsonFormatter() : new Serilog.Formatting.Json.JsonFormatter(),
                        appLogFilePath, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, fileSizeLimitBytes: 1000000))
                    .CreateLogger();
        }

        public void Start()
        {
            try
            {
                if (string.IsNullOrEmpty(_queueName))
                {
                    _logger.Error("Queue name is null or empty.");
                    throw new ArgumentNullException(nameof(_queueName));
                }

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"[x] Received {message}");

                    _logger.Information(message);
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };

                _channel.BasicConsume(queue: _queueName,
                                     autoAck: false,
                                     consumer: consumer);

                //Console.WriteLine("Press [enter] to exit.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Start method of RabbitMQConsumer");
                throw;
            }
        }

        public void Close()
        {
            _channel.Close();
            _connection.Close();
        }
    }

}
