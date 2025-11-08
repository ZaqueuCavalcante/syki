using Dapper;
using Exato.Back.Database.Mappers;

namespace Exato.Back.Configs;

public static class DapperConfigs
{
    public static void AddDapperConfigs(this WebApplicationBuilder _)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        SqlMapper.AddTypeHandler(new IntListTypeHandler());
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        SqlMapper.AddTypeHandler(new StringEnumArrayTypeHandler<CommandStatus>());
    }
}
