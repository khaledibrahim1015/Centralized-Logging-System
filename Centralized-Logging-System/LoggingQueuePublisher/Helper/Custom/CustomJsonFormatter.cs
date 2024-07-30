using Serilog.Events;
using Serilog.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoggingQueuePublisher.Helper.Custom
{
    public class CustomJsonFormatter : ITextFormatter
    {


        public void Format(LogEvent logEvent, TextWriter output)
        {
            var logObject = new
            {
                Timestamp = logEvent.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"), // Custom timestamp format
                Level = logEvent.Level,
                Message = logEvent.RenderMessage(),
                Exception = logEvent.Exception?.ToString(),
                Properties = logEvent.Properties.ToDictionary(p => p.Key, p => p.Value.ToString())
            };

            var json = JsonSerializer.Serialize(logObject, new JsonSerializerOptions { WriteIndented = true });
            output.WriteLine(json);
        }



    }

}
