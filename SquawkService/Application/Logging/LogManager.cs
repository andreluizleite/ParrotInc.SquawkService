using Microsoft.Extensions.Logging;
using Serilog;

namespace ParrotInc.SquawkService.Application.Logging
{
    public sealed class LogManager
    {
        private static readonly Lazy<LogManager> _instance = new Lazy<LogManager>(() => new LogManager());

        public static LogManager Instance => _instance.Value;

        public void LogInformation<T>(ILogger<T> logger, string messageTemplate, params object[] propertyValues)
        {
            logger.LogInformation(messageTemplate, propertyValues);
        }

        public void LogError<T>(ILogger<T> logger, Exception ex, string messageTemplate, params object[] propertyValues)
        {
            logger.LogError(ex, messageTemplate, propertyValues);
        }

        public void LogWarning<T>(ILogger<T> logger, string messageTemplate, params object[] propertyValues)
        {
            logger.LogWarning(messageTemplate, propertyValues);
        }
    }
    }
