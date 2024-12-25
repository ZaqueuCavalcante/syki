using Dapper;
using System.Data;

namespace Syki.Back.Configs;

public static class DapperConfigs
{
    public static void AddDapperConfigs(this IServiceCollection _)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        SqlMapper.AddTypeHandler(new StringEnumArrayHandler<SykiTaskStatus>());
    }
}

public class StringEnumArrayHandler<T> : SqlMapper.TypeHandler<T[]> where T : Enum
{
    public override T[] Parse(object value)
    {
        return ((string[])value).Select(x => x.ToEnum<T>()).ToArray();
    }

    public override void SetValue(IDbDataParameter parameter, T[]? value)
    {
        parameter.Value = value;
    }
}
