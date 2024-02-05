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

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
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
            options.DocumentTitle = "Syki API";
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Syki 1.0");
            options.DefaultModelsExpandDepth(-1);
        });
    }
}
