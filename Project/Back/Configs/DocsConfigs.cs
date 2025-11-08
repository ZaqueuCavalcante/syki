using System.Reflection;
using Scalar.AspNetCore;
using Microsoft.OpenApi.Models;

namespace Exato.Back.Configs;

public static class DocsConfigs
{
    public static void AddDocsConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Exato API Docs",
                Description = ReadResource("api-intro.md"),
            });

            options.EnableAnnotations();

            options.TagActionsBy(api =>
            {
                var group = api.RelativePath.Split("/")[0];
                if (group == "office") return ["🛡️ Office"];
                if (group == "orgs") return ["🏢 Orgs"];
                if (group == "web") return ["🌐 Web"];
                return ["🧱 Cross"];
            });
            options.DocInclusionPredicate((name, api) => true);

            options.SchemaFilter<EnumSchemaFilter>();
            options.OperationFilter<AuthOperationsFilter>();
            options.OperationFilter<IdParameterExamplesFilter>();
            options.OperationFilter<ExamplesOperationsFilterFilter>();
            options.DocumentFilter<HttpMethodSorterDocumentFilter>();

            options.ExampleFilters();

            options.AddSecurityDefinition(
                AuthenticationConfigs.BearerScheme,
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Cookie,
                    Type = SecuritySchemeType.ApiKey,
                    Name = AuthenticationConfigs.BearerCookie,
                    Description = "JWT enviado no cookie http only",
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
        options.MapScalarApiReference("/docs", options =>
        {
            options.WithModels(false);
            options.WithTitle("Exato API Docs");
            options.WithDocumentDownloadType(DocumentDownloadType.Json);
            options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");
            options
                .AddPreferredSecuritySchemes(AuthenticationConfigs.BearerScheme)
                .AddApiKeyAuthentication(AuthenticationConfigs.BearerScheme, x => x.Value = AuthenticationConfigs.BearerCookie);

            options.CustomCss = @"
                :root {
                    --scalar-sidebar-width: 300px;
                }
            ";
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
