using Syki.Back.Auth.Schemes;
using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    /// <summary>
    /// Basta que o usuário esteja logado.
    /// </summary>
    public static AuthorizationBuilder AddSykiPolicy(this AuthorizationBuilder builder, string name)
    {
        return builder.AddPolicy(name, policy => policy
            .RequireAuthenticatedUser()
            .AddAuthenticationSchemes(JwtBearerScheme.Name));
    }

    /// <summary>
    /// O usuário precisa estar logado e seu perfil de acesso deve possuir as permissões especificadas.
    /// </summary>
    public static AuthorizationBuilder AddSykiPolicy(this AuthorizationBuilder builder, string name, params SykiPermission[] permissions)
    {
        var ids = permissions.Select(x => x.Id).ToList();

        return builder.AddPolicy(name, policy => policy
            .RequireAuthenticatedUser()
            .RequireAssertion(x => x.User.Permissions.Any(p => ids.Contains(p)))
            .AddAuthenticationSchemes(JwtBearerScheme.Name));
    }

    /// <summary>
    /// O usuário precisa estar logado e seu perfil de acesso deve possuir o tipo base e as permissões especificadas.
    /// </summary>
    public static AuthorizationBuilder AddSykiPolicy(this AuthorizationBuilder builder, string name, UserType userType, params SykiPermission[] permissions)
    {
        var ids = permissions.Select(x => x.Id).ToList();

        return builder.AddPolicy(name, policy => policy
            .RequireAuthenticatedUser()
            .RequireAssertion(x => x.User.Type == userType && x.User.Permissions.Any(p => ids.Contains(p)))
            .AddAuthenticationSchemes(JwtBearerScheme.Name));
    }
}
