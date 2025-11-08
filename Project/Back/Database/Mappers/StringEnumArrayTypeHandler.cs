using Dapper;
using System.Data;

namespace Exato.Back.Database.Mappers;

public class StringEnumArrayTypeHandler<T> : SqlMapper.TypeHandler<T[]> where T : Enum
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
