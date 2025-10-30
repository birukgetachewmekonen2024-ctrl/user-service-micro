using Serilog;

namespace UserManagement.Service.Logging
{
    public static class SerilogConfig
    {
        public static void Configure()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/user-management-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}