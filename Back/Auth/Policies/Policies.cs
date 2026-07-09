using Estud.Back.Auth.Schemes;
using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    /// <summary>
    /// Basta que o usuário esteja logado.
    /// </summary>
    public static AuthorizationBuilder AddEstudPolicy(this AuthorizationBuilder builder, string name)
    {
        return builder.AddPolicy(name, policy => policy
            .RequireAuthenticatedUser()
            .AddAuthenticationSchemes(JwtBearerScheme.Name));
    }

    /// <summary>
    /// O usuário precisa estar logado e seu perfil de acesso deve possuir as permissões especificadas.
    /// </summary>
    public static AuthorizationBuilder AddEstudPolicy(this AuthorizationBuilder builder, string name, params EstudPermission[] permissions)
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
    public static AuthorizationBuilder AddEstudPolicy(this AuthorizationBuilder builder, string name, UserType userType, params EstudPermission[] permissions)
    {
        var ids = permissions.Select(x => x.Id).ToList();

        return builder.AddPolicy(name, policy => policy
            .RequireAuthenticatedUser()
            .RequireAssertion(x => x.User.Type == userType && (ids.Count == 0 || x.User.Permissions.Any(p => ids.Contains(p))))
            .AddAuthenticationSchemes(JwtBearerScheme.Name));
    }
}
