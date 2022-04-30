using Akka.Actor;
using Serilog;

namespace Hafnia.Shared.Config;

public static class ActorSystemConfig
{
    public static ActorSystem CreateSystemFromPath(string systemName, string hoconConfigPath, ILogger? logger = null)
    {
        if (!File.Exists(hoconConfigPath))
            throw new FileNotFoundException("No file found", hoconConfigPath);

        string hoconConfig = File.ReadAllText(hoconConfigPath);

        return CreateSystem(systemName, hoconConfig, logger);
    }

    public static ActorSystem CreateSystem(string systemName, string hoconConfig, ILogger? logger = null)
    {
        if (logger == null)
        {
            logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Information()
                .CreateLogger();
        }

        Log.Logger = logger;

        ActorSystem system = ActorSystem.Create(systemName, hoconConfig);

        return system;
    }
}
