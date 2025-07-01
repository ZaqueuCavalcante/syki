using Dapper;
using Syki.Back.Database.Mappers;

namespace Syki.Back.Configs;

public static class DapperConfigs
{
    public static void AddDapperConfigs(this WebApplicationBuilder _)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        SqlMapper.AddTypeHandler(new StringEnumArrayHandler<CommandStatus>());
    }
}
