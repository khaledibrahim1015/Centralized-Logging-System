using ConsumerWorker.Helper;
using ConsumerWorker.RabbitMQService;
using Serilog;

namespace ConsumerWorker
{
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
                //.UseSystemd()  //  Run as a linux daemmon 
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {

                    IConfiguration configuration = hostContext.Configuration;
                    services.AddRabbitMQConsumers(configuration);
                    services.AddHostedService<Worker>();


                });
    }
}