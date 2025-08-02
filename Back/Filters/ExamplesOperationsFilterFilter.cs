using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Syki.Back.Filters;

public class ExamplesOperationsFilterFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var method = context.MethodInfo;

        var swaggerAttrs = method
            .GetCustomAttributes<SwaggerResponseExampleAttribute>(inherit: false);

        foreach (var attr in swaggerAttrs)
        {
            var providerType = attr.ExamplesProviderType;
            var responseType = GetExampleGenericType(providerType);
            var statusCode = attr.StatusCode.ToString();

            if (responseType == null)
                continue;

            if (!operation.Responses.ContainsKey(statusCode))
            {
                operation.Responses[statusCode] = new OpenApiResponse
                {
                    Description = GetDefaultDescription(statusCode),
                    Content = new Dictionary<string, OpenApiMediaType>()
                };
            }

            var schema = context.SchemaGenerator.GenerateSchema(responseType, context.SchemaRepository);
            operation.Responses[statusCode].Content["application/json"] = new OpenApiMediaType
            {
                Schema = schema
            };
        }
    }

    private static Type? GetExampleGenericType(Type providerType)
    {
        return providerType
            .GetInterfaces()
            .FirstOrDefault(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IMultipleExamplesProvider<>))
            ?.GetGenericArguments()[0];
    }

    private string GetDefaultDescription(string statusCode) => statusCode switch
    {
        "200" => "Success",
        "400" => "Bad Request",
        "401" => "Unauthorized",
        "403" => "Forbidden",
        "404" => "Not Found",
        _ => "Response"
    };
}
