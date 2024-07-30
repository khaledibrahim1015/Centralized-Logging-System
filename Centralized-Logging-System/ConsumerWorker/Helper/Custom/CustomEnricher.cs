using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerWorker.Helper.Custom
{
    public class CustomEnricher : ILogEventEnricher
    {
        private readonly string _logFilePath;

        public CustomEnricher(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("LogFilePath", _logFilePath));
        }
    }

}
