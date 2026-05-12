using Syki.Back.Auth.Schemes;
using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public static List<PolicyMetadata> All = [];

    /// <summary>
    /// Basta que o usuário esteja logado.
    /// </summary>
    public static AuthorizationBuilder AddSykiPolicy(this AuthorizationBuilder builder, string name)
    {
        All.Add(new() { Name = name, Permissions = [] });

        return builder.AddPolicy(name, policy => policy
            .RequireAuthenticatedUser()
            .AddAuthenticationSchemes(JwtBearerScheme.Name));
    }

    /// <summary>
    /// O usuário precisa estar logado numa instituição onde seu perfil de acesso possui as permissões especificadas.
    /// </summary>
    public static AuthorizationBuilder AddSykiPolicy(this AuthorizationBuilder builder, string name, params SykiPermission[] permissions)
    {
        All.Add(new() { Name = name, Permissions = permissions.ToList() });

        var ids = permissions.Select(x => x.Id).ToList();

        return builder.AddPolicy(name, policy => policy
            .RequireAuthenticatedUser()
            .RequireAssertion(x => x.User.Permissions.Any(p => ids.Contains(p)))
            .AddAuthenticationSchemes(JwtBearerScheme.Name));
    }
}
