using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Syki.Back.Filters;

public class IdParameterExamplesFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var parameter in operation.Parameters)
        {
            if (parameter.In.IsIn(ParameterLocation.Path, ParameterLocation.Query) && parameter.Name.Contains("id", StringComparison.OrdinalIgnoreCase))
            {
                if (parameter.Example == null)
                {
                    parameter.Example = new OpenApiString(Guid.CreateVersion7().ToString());
                }
            }
        }
    }
}
