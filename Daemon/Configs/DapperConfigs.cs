using Dapper;

namespace Syki.Daemon.Configs;

public static class DapperConfigs
{
    public static void AddDapperConfigs(this IServiceCollection _)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
}
