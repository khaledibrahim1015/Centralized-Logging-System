# Centralized Logging System
## Overview
This project implements a centralized logging system using .NET Core, Serilog, RabbitMQ, and Elasticsearch Kibbana . It consists of two main components:

## Publisher:
### Configures Serilog in .NET Core applications to send logs to RabbitMQ queues.
## Consumer:
### A background service that consumes logs from RabbitMQ queues, writes them to files, and inserts them into Elasticsearch indices.

## Architecture
![impl3](https://github.com/khaledibrahim1015/Queuing-Logger/assets/91853322/cb5d5e98-e68f-4e90-9ba0-8b007f54cd14)


Prerequisites
- .NET Core 3.1 or later
- RabbitMQ server
- Elasticsearch server

### Publisher Component
Installation
Add the following NuGet packages to your .NET Core application:
``` 
dotnet add package Serilog
dotnet add package Serilog.Sinks.RabbitMQ
dotnet add package RabbitMQ.Client
```
Configuration

1. Add the following to your appsettings.json:
```
{
  "RabbitMqClientConfig": {
    "UserName": "your_username",
    "Password": "your_password",
    "HostName": "your_rabbitmq_host",
    "Port": "your_port",
    "RouteKey": "your_queue_name"
  }
}
```
2- In Program.cs, use the ConfigurationHelper to set up Serilog:
```
public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = ConfigurationHelper.SetConfigurationLogging(true);
        try
        {
            Log.Information("Application Started.");
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application start-up failed");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
```

```
This setup uses a custom ConfigurationHelper class to configure Serilog with RabbitMQ.
The SetConfigurationLogging method reads RabbitMQ configuration from appsettings.json and sets up Serilog accordingly.
The useCustomFormat parameter allows for using a custom JSON formatter if set to true. This gives you flexibility in how your logs are formatted before being sent to RabbitMQ.
By calling ConfigurationHelper.SetConfigurationLogging(true) in Program.cs, you're setting up Serilog with RabbitMQ integration and using a custom JSON formatter.
This approach provides more control over the Serilog configuration and allows for easier customization of log formatting and enrichment.

```

3- In Startup.cs, add RabbitMQ services:
Register it in DI CONTAINER 
```
public void ConfigureServices(IServiceCollection services)
{
    services.AddRabbitMQ(Configuration);
}
```
IN .NET8
```
builder.Services.AddRabbitMQ(Configuration);
```
## Usage
Use Serilog to log messages in your application To A specific queue have been created (RouteKey):
```
Log.Information("This is an information message");
Log.Warning("This is a warning message");
Log.Error("This is an error message");
```

### Consumer Component
Installation
Create a new .NET Core console application and add the following NuGet packages:

```
dotnet add package RabbitMQ.Client
dotnet add package NEST
dotnet add package Serilog
dotnet add package Serilog.Sinks.RabbitMQ
```
Configuration
1- Add the following to your appsettings.json:
```
{
  "RabbitMQSettings": {
    "HostName": "your_rabbitmq_host",
    "ElasticsearchUrl": "http://your_elasticsearch_host:9200",
    "Consumers": [
      {
        "QueueName": "queue1",
        "ElasticIndexName": "index1",
        "LogFilePath": "/path/to/logs/queue1.log"
      },
      {
        "QueueName": "queue2",
        "ElasticIndexName": "index2",
        "LogFilePath": "/path/to/logs/queue2.log"
      },
// here to ad new consumer configuration
    {
            "QueueName": "",
            "ElasticIndexName": "",
            "LogFilePath": ""
    }
    ]
  }
}
```

2- In Program.cs, use the ConfigurationHelper to set up Serilog , configure the consumer service:
```
    public class Program
    {
        public static void Main(string[] args)
        {


            // To Override BuiltIn Logger In .Net 
            Log.Logger = ConfigurationHelper.SetConfigurationLogging();
            try
            {

                Log.Information("Starting up the Consumer Service ...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {

                Log.Fatal(ex, "There was a problem starting the service");
            }
            finally
            {

                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseSystemd()  //  Run as a linux daemmon   or .UseWindowsService as windows service 
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {

                    IConfiguration configuration = hostContext.Configuration;
                    // Register services 
                    services.AddRabbitMQConsumers(configuration);
                    services.AddHostedService<Worker>();


                });
    }

```

## Deployment

Deploy your .NET Core applications with the Publisher component configured.
Set up RabbitMQ server and create the necessary queues.
Set up Elasticsearch server and create the required indices.
Deploy the Consumer application to a server that can access both RabbitMQ and Elasticsearch.

Monitoring and Maintenance

Monitor RabbitMQ queues for message buildup.
Check Elasticsearch indices for proper log insertion.
Regularly review and rotate log files created by the Consumer.









