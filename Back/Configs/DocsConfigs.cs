using System.Reflection;
using Scalar.AspNetCore;
using Microsoft.OpenApi.Any;
using Syki.Back.Auth.Schemes;
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
                if (group == "campi") return ["🏫 Campi"];
                if (group == "courses") return ["📚 Courses"];
                if (group == "course-curriculums") return ["📋 Curriculums"];
                if (group == "course-offerings") return ["🗓️ Offerings"];
                if (group == "disciplines") return ["📖 Disciplines"];
                if (group == "periods") return ["📅 Periods"];
                if (group == "students") return ["🎓 Students"];
                if (group == "teachers") return ["👨‍🏫 Teachers"];
                if (group == "identity") return ["🛡️ Identity"];
                if (group == "users") return ["👩🏻‍🎓 Users"];
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
                JwtBearerScheme.Name,
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Cookie,
                    Name = JwtBearerScheme.Cookie,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "JWT enviado no cookie http only",
                });

            options.DescribeAllParametersInCamelCase();

            var xmlPath = Path.Combine(AppContext.BaseDirectory, "Back.xml");
            options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
        });

        builder.Services.AddSwaggerExamplesFromAssemblyOf(typeof(Program));
        builder.Services.AddOpenApi();
    }

    public static void MapScalarDocs(this IEndpointRouteBuilder options)
    {
        options.MapScalarApiReference("/docs", options =>
        {
            options.WithModels(false);
            options.WithTitle("Syki API Docs");
            options.WithDocumentDownloadType(DocumentDownloadType.Json);
            options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");
            options
                .AddPreferredSecuritySchemes("Bearer")
                .AddHttpAuthentication("Bearer", x => x.Token = "your.bearer.token");

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
