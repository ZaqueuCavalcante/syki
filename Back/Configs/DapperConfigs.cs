using Dapper;

namespace Syki.Back.Configs;

public static class DapperConfigs
{
    public static void AddDapperConfigs(this IServiceCollection _)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
}
