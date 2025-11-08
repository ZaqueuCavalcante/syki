using Dapper;
using System.Data;

namespace Exato.Back.Database.Mappers;

public class IntListTypeHandler : SqlMapper.TypeHandler<List<int>>
{
    public override List<int> Parse(object value)
    {
        return ((int[])value).ToList();
    }

    public override void SetValue(IDbDataParameter parameter, List<int>? value)
    {
        parameter.Value = value;
    }
}
