using ConsumerWorker.Factory;
using ConsumerWorker.RabbitMQService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMQConsumers(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQSettings"));
        services.AddSingleton<RabbitMQConsumerFactory>();

        services.AddSingleton(serviceProvider =>
        {
            var factory = serviceProvider.GetRequiredService<RabbitMQConsumerFactory>();
            var consumers = factory.GetConsumers();
            return consumers;
        });

        // Register each consumer as a singleton service
        services.AddSingleton<IEnumerable<RabbitMQConsumer>>(serviceProvider =>
        {
            var factory = serviceProvider.GetRequiredService<RabbitMQConsumerFactory>();
            return factory.GetConsumers();
        });

        return services;
    }
}
