using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.RabbitMQ;

using RabbitMQ.Client;
using Serilog.Core;
using Microsoft.Extensions.Configuration;
using LoggingQueuePublisher.Helper.Custom;

namespace LoggingQueuePublisher.Configuration
{
    public class ConfigurationHelper
    {
        private static string _environment
        {
            get => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }


        private static IConfigurationRoot GetConfigurationRoot(string environment = "")
        {
            var builder = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            if (!string.IsNullOrEmpty(environment))
                builder.AddJsonFile($"appsettings{environment}.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        public static string GetAppSettingsKeyValue(string key, string environment = "")
                  => GetConfigurationRoot(environment).GetValue<string>(key) ?? string.Empty;

        public static string GetAppSettingsSectionValue(string sectionName, string key, string environment = "")
                 => GetConfigurationRoot(environment).GetSection(sectionName).GetValue<string>(key) ?? string.Empty;



        public static Logger SetConfigurationLogging(bool useCustomFormat = false)
        {
            var logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .MinimumLevel.Override("System", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .Enrich.WithExceptionDetails()
                        .Enrich.WithProcessName()
                        .Enrich.WithProcessId()
                        .Enrich.WithThreadName()
                        .Enrich.WithThreadId()
                        .Enrich.WithProperty("Environment", _environment)
                        .WriteTo.Console()
                        .WriteTo.RabbitMQ((rabbitmqCllientConfiguration, sinkConfiguration) =>
                        {
                            rabbitmqCllientConfiguration.From(new RabbitMQClientConfiguration()
                            {
                                //  set the RabbitMQ server credentials.
                                Username = GetAppSettingsSectionValue("RabbitMqClientConfig", "UserName"),
                                Password = GetAppSettingsSectionValue("RabbitMqClientConfig", "Password"),
                                // sets the RabbitMQ exchange. An empty string indicates the default exchange.
                                Exchange = "",
                                //  take message with a specific routing key (queuename)  to send message to a specific queue 
                                ExchangeType = ExchangeType.Direct,
                                // ensures that messages are marked as durable. persistence  //  that means make sure message still there untill consumer read it !
                                DeliveryMode = RabbitMQDeliveryMode.Durable,
                                RouteKey = GetAppSettingsSectionValue("RabbitMqClientConfig", "RouteKey"), // sets the routing key (or queue name) where messages will be sent.
                                                                                                           //Port = 5672,
                                Hostnames = { $"{GetAppSettingsSectionValue("RabbitMqClientConfig", "HostName")}" },
                                Port = int.Parse(GetAppSettingsSectionValue("RabbitMqClientConfig", "Port"))

                            });
                            //  Customize Msg send 
                            sinkConfiguration.TextFormatter = !useCustomFormat ? new Serilog.Formatting.Json.JsonFormatter() : new CustomJsonFormatter();

                        })
                        .CreateLogger();
            return logger;
        }
    }
}
