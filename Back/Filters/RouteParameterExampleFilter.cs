using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Syki.Back.Filters;

public class IdParameterExamplesFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var routeTemplate = context.ApiDescription.RelativePath?.ToLowerInvariant() ?? string.Empty;

        foreach (var parameter in operation.Parameters)
        {
            if (!parameter.In.IsIn(ParameterLocation.Path, ParameterLocation.Query))
                continue;

            if (parameter.Example != null)
                continue;

            if (routeTemplate.Contains("academic/enrollment-periods") && parameter.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                parameter.Example = new OpenApiString($"{DateTime.Now.Year}.1");
                continue;
            }

            if (parameter.Name.Contains("id", StringComparison.OrdinalIgnoreCase))
            {
                parameter.Example = new OpenApiString(Guid.CreateVersion7().ToString());
            }
        }
    }
}
