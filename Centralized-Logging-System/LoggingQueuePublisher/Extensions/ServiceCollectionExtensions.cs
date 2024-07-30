using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LoggingQueuePublisher.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using LoggingQueuePublisher.Configuration;
using LoggingQueuePublisher.Factory;

namespace LoggingQueuePublisher.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<RabbitMqClientConfig>(option => configuration.GetSection("RabbitMqClientConfig").Bind(option));
            services.AddSingleton<RabbitMqPublisherFactory>();


            services.AddSingleton(serviceProvider =>
            {
                var factory = serviceProvider.GetRequiredService<RabbitMqPublisherFactory>();
                return factory.CreateRabbitMQService();
            });

            services.AddHostedService<RabbitMQHostedService>();

            return services;
        }

    }
}
