using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingQueuePublisher.Services
{
    public class RabbitMQService
    {
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly int _port;
        private readonly string _username ;
        private readonly string _password ;

        public RabbitMQService(string hostname, string queueName , int port ,string username , string  password  )
        {
            _hostname = hostname;
            _queueName = queueName;
            _port = port;
            _username = username;
            _password = password;
        }

        public void CreateQueue()
        {

            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = _username;
            factory.Password = _password;

            var endpoints = new System.Collections.Generic.List<AmqpTcpEndpoint> {
                            new AmqpTcpEndpoint(_hostname,_port),
                          };
            var conn = factory.CreateConnection(endpoints);
            var ch = conn.CreateModel();
            ch.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

    }
}
