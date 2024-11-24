using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.Controllers;
using Syki.Back.Features.Cross.SkipUserRegister;

namespace Syki.Back.Filters;

public class AuthOperationsFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
            .Union(context.MethodInfo.GetCustomAttributes(true))
            .OfType<AuthorizeAttribute>();

        var controllerDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
        var controller = controllerDescriptor != null ? controllerDescriptor.ControllerName : null;

        if (authAttributes.Any() && controller !="SkipUserRegister")
        {
            var securityRequirement = new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        }
                    },
                    new string[] {}
                }
            };
            operation.Security = [securityRequirement];
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
        }
    }
}
