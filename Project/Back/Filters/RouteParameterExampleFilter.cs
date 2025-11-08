using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Exato.Back.Filters;

public class IdParameterExamplesFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var parameter in operation.Parameters)
        {
            if (!parameter.In.IsIn(ParameterLocation.Path, ParameterLocation.Query))
                continue;

            if (parameter.Example != null)
                continue;

            if (parameter.Name.Contains("id", StringComparison.OrdinalIgnoreCase))
            {
                parameter.Example = new OpenApiString(Guid.NewGuid().ToString());
            }
        }
    }
}
