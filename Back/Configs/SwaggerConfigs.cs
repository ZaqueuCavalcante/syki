using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Syki.Back.Configs;

public static class SwaggerConfigs
{
    public static void AddSwaggerConfigs(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Syki",
                Version = "1.0",
                Description = "Syki API.",
                Contact = new OpenApiContact() { Name = "Zaqueu Cavalcante", Email = "zaqueudovale@gmail.com" },
                TermsOfService = new Uri("https://docs.github.com"),
                License = new OpenApiLicense() { Name = "License", Url = new Uri("https://opensource.org/licenses/MIT") }
            });

            options.DescribeAllParametersInCamelCase();

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
        });
    }

    public static void UseSwaggerThings(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Syki 1.0");
            options.DefaultModelsExpandDepth(-1);
        });
    }
}
