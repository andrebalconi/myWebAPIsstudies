using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Logging
{
    public class CustomLoggerProvider : ILoggerProvider 
    {
        readonly CustomLoggerProviderConfiguration loggingConfig;
        readonly ConcurrentDictionary<string, CustomerLogger> loggers = new ConcurrentDictionary<string, CustomerLogger>();

        public CustomLoggerProvider(CustomLoggerProviderConfiguration config)
        {
            loggingConfig = config;
        }
        public ILogger CreateLogger(string categoryName) 
        {
            return loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, loggingConfig));
        }

        public void Dispose()
        {
            loggers.Clear();
        }
    }
}
