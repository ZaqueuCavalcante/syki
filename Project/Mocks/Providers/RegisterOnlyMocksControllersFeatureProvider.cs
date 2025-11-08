using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Exato.Mocks.Providers;

public sealed class RegisterOnlyMocksControllersFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        for (int i = feature.Controllers.Count - 1; i >= 0; i--)
        {
            var ctrl = feature.Controllers[i];

            if (!ctrl.AssemblyQualifiedName.StartsWith("Exato.Mocks"))
                feature.Controllers.RemoveAt(i);
        }
    }
}
