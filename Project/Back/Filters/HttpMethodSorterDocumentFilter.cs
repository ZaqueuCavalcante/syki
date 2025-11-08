using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Exato.Back.Filters;

public class HttpMethodSorterDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument document, DocumentFilterContext context)
    {
        // Order path groups (OpenApiPathItems) alphabetically
        var pathKvps = document.Paths
            .OrderBy(pathKvp =>
            {
                if (pathKvp.Value.Operations.Any(x => x.Key == OperationType.Post)) return 10;
                if (pathKvp.Value.Operations.Any(x => x.Key == OperationType.Get)) return 20;
                if (pathKvp.Value.Operations.Any(x => x.Key == OperationType.Put)) return 30;
                if (pathKvp.Value.Operations.Any(x => x.Key == OperationType.Delete)) return 40;
                return 50;
            })
            .ToList();

        document.Paths.Clear();
        pathKvps.ForEach(kvp => document.Paths.Add(kvp.Key, kvp.Value));

        // Order operations by method within each group
        document.Paths.ToList().ForEach(pathKvp =>
        {
            var operationKvps = pathKvp.Value.Operations
                .OrderBy(kvp =>
                {
                    var weight = kvp.Key switch
                    {
                        OperationType.Post => 10,
                        OperationType.Get => 20,
                        OperationType.Put => 30,
                        OperationType.Delete => 40,
                        _ => 50,
                    };
                    return weight;
                })
                .ToList();

            pathKvp.Value.Operations.Clear();
            operationKvps.ForEach(operationKvp =>
            {
                pathKvp.Value.Operations.Add(operationKvp);
            });
        });
    }
}
