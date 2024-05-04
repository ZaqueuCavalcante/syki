using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Interfaces;

namespace Syki.Back.Configs;

public static class SwaggerConfigs
{
    public static void AddDocsConfigs(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Description = ReadResource("api-intro.md"),
                Extensions = new Dictionary<string, IOpenApiExtension>
                {
                    { "x-logo", new OpenApiObject
                    {
                        { "url", new OpenApiString("/syki-logo.png") },
                    }}
                },
            });

            options.ExampleFilters();

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

        services.AddSwaggerExamplesFromAssemblyOf(typeof(Program));
    }

    public static void UseDocs(this IApplicationBuilder app)
    {
        app.UseStaticFiles();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DocumentTitle = "Syki API";
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Syki 1.0");
            options.DefaultModelsExpandDepth(-1);
        });

        app.UseReDoc(c =>
        {
            c.RoutePrefix = "docs";
            c.DocumentTitle = "Syki API";
            c.SpecUrl = "/swagger/v1/swagger.json";
        });
    }

    public static string ReadResource(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(name));

        using Stream stream = assembly.GetManifestResourceStream(resourcePath)!;
        using StreamReader reader = new(stream);

        return reader.ReadToEnd();
    }
}
