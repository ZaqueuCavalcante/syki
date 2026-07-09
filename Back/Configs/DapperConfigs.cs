using Dapper;
using Estud.Back.Database.Mappers;

namespace Estud.Back.Configs;

public static class DapperConfigs
{
    public static void AddDapperConfigs(this WebApplicationBuilder _)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        SqlMapper.AddTypeHandler(new IntListTypeHandler());
        SqlMapper.AddTypeHandler(new StringListTypeHandler());
    }
}
