using Serilog.Events;
using Serilog.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsumerWorker.Helper.Custom
{
    public class CustomJsonFormatter : ITextFormatter
    {

        public void Format(LogEvent logEvent, TextWriter output)
        {
            // A helper function to clean unwanted characters from strings
            string CleanString(string input)
            {

                if (string.IsNullOrEmpty(input))
                {
                    return input;
                }

                return input.Replace("\r\n", "")
                            .Replace("\n", "")
                            .Replace("\u0022", "")
                            .Replace("\\u0022", "")
                            .Replace("\\n", "")
                            .Replace("\\u003E", "")
                            .Replace("\\u0027", "")
                            .Replace("\\u003C", "")
                            .Replace("\\u003D", "")
                            .Replace("\\u0026", "")
                            .Replace("\\u0025", "")
                            .Replace("\\u002C", "")
                            .Replace("\\u002F", "");
            }

            var logObject = new
            {
                Timestamp = logEvent.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                Level = logEvent.Level,
                Message = CleanString(logEvent.RenderMessage()),
                Exception = logEvent.Exception != null ? CleanString(logEvent.Exception.ToString()) : null,
                Properties = logEvent.Properties.ToDictionary(
                    p => CleanString(p.Key),
                    p => CleanString(p.Value.ToString())
                )
            };

            var json = JsonSerializer.Serialize(logObject, new JsonSerializerOptions { WriteIndented = true });
            output.WriteLine(CleanString(json));
        }




    }

}
