
using LoggingQueuePublisher.Configuration;
using LoggingQueuePublisher.Extensions ;

using Serilog;
using System.Configuration;

namespace BillPayment.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // To Override BuiltIn Logger In .Net 
            Log.Logger = ConfigurationHelper.SetConfigurationLogging(true);

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Use the extension method to add RabbitMQ services
            builder.Services.AddRabbitMQ(builder.Configuration);




            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();
           
            app.Run();
        }
    }
}
