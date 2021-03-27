using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Core.Logging
{
    public sealed class MonetaLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            if (categoryName.Contains("."))
            {
                categoryName = categoryName.Substring(categoryName.LastIndexOf('.') + 1);
            }

            return new MonetaLogger(categoryName);
        }


        public void Dispose() { }
    }

    public class MonetaLogger : ILogger
    {
        private readonly string _Name;

        public MonetaLogger(string name)
        {
            _Name = name;
        }

        public IDisposable BeginScope<TState>(TState state) => default;

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            ConsoleColor originalColor = Console.ForegroundColor;


            switch (logLevel)
            {
                case LogLevel.Trace:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Information:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Critical:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.None:
                    break;
            }

            Console.Write($"[{DateTime.UtcNow:HH:mm:ss}] [{logLevel,-11}] ");
            Console.ForegroundColor = originalColor;
            Console.WriteLine($" {_Name} - {formatter(state, exception)}");
        }
    }
}
