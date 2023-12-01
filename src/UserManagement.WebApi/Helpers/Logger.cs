using Serilog;
using ILogger = Serilog.ILogger;

namespace UserManagement.WebApi.Helpers;

public class Logger
{
    private static readonly ILogger _logger;

    static Logger()
    {
        _logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .WriteTo.File("log.txt")
            .CreateLogger();
    }

    public static void Info(string message)
    {
        _logger.Information(message);
    }

    public static void Error(string message)
    {
        _logger.Error(message);
    }

    public static void Error(Exception ex)
    {
        _logger.Error(ex, ex.Message);
    }
}
