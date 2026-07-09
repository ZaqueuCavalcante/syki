using Dapper;
using System.Data;

namespace Estud.Back.Database.Mappers;

public class StringListTypeHandler : SqlMapper.TypeHandler<List<string>>
{
    public override List<string> Parse(object value)
    {
        return ((string[])value).ToList();
    }

    public override void SetValue(IDbDataParameter parameter, List<string>? value)
    {
        parameter.Value = value;
    }
}
