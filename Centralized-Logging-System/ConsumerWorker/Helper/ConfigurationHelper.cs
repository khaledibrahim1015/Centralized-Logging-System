using Nest;
using Serilog.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog.Exceptions;
using Serilog.Core;

namespace ConsumerWorker.Helper
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

        public static Logger SetConfigurationLogging()
         //  based on this configuration msg will be
         //  //[Information] [2024-06-05T14:35:27.6123456Z] [MachineName=YourMachine] [ProcessName=YourProcess] [ProcessId=12345] [ThreadName=MainThread] [ThreadId=1] [Environment=Development] message
         => new LoggerConfiguration()
               .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .Enrich.WithProcessName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadName()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("Environment", _environment)
                .WriteTo.Console()  //  move writetofile in customformat 

                .CreateLogger();

        public static void WriteToElasticsearch(string indexName, string message, string elasticsearchUrl)
        {
            var settings = new ConnectionSettings(new Uri(elasticsearchUrl)).DefaultIndex(indexName);
            var client = new ElasticClient(settings);

            var logEntry = JsonSerializer.Deserialize<dynamic>(message);
            var indexResponse = client.IndexDocument(logEntry);
        }
    }

}
