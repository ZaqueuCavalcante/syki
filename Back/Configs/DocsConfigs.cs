using System.Reflection;
using Scalar.AspNetCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Interfaces;

namespace Syki.Back.Configs;

public static class DocsConfigs
{
    public static void AddDocsConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Syki API Docs",
                Description = ReadResource("api-intro.md"),
                Extensions = new Dictionary<string, IOpenApiExtension>
                {
                    { "x-logo", new OpenApiObject
                    {
                        { "url", new OpenApiString("/syki-logo.png") },
                    }}
                },
            });

            options.EnableAnnotations();

            options.TagActionsBy(api =>
            {
                var group = api.RelativePath.Split("/")[0];
                if (group == "academic") return ["🏫 Academic"];
                if (group == "student") return ["👩🏻‍🎓 Student"];
                if (group == "teacher") return ["👨🏻‍🏫 Teacher"];
                if (group == "adm") return ["🛡️ Adm"];
                return ["🧱 Cross"];
            });
            options.DocInclusionPredicate((name, api) => true);

            options.SchemaFilter<EnumSchemaFilter>();
            options.OperationFilter<AuthOperationsFilter>();
            options.DocumentFilter<HttpMethodSorterDocumentFilter>();

            options.ExampleFilters();

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer",
            });

            options.DescribeAllParametersInCamelCase();

            var xmlPath = Path.Combine(AppContext.BaseDirectory, "Back.xml");
            options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

            var xmlPath2 = Path.Combine(AppContext.BaseDirectory, "Shared.xml");
            options.IncludeXmlComments(xmlPath2, includeControllerXmlComments: true);
        });

        builder.Services.AddSwaggerExamplesFromAssemblyOf(typeof(Program));
        builder.Services.AddOpenApi();
    }

    public static void MapScalarDocs(this IEndpointRouteBuilder options)
    {
        options.MapScalarApiReference(options =>
        {
            options.WithModels(false);
            options.WithDownloadButton(false);
            options.WithTitle("Syki API Docs");
            options.WithEndpointPrefix("/docs/{documentName}");
            options.WithOpenApiRoutePattern("/swagger/v1/swagger.json");
            options
                .WithPreferredScheme("Bearer")
                .WithHttpBearerAuthentication(bearer =>
                {
                    bearer.Token = "your.bearer.token";
                })
                .WithHttpBasicAuthentication(basic => { });
        });
    }

    public static void UseDocs(this IApplicationBuilder app)
    {
        app.UseStaticFiles();
        app.UseSwagger();
    }

    private static string ReadResource(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(name));

        using Stream stream = assembly.GetManifestResourceStream(resourcePath)!;
        using StreamReader reader = new(stream);

        return reader.ReadToEnd();
    }
}
