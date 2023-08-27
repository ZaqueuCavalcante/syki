namespace Syki.Configs;

public static class AuthorizationConfigs
{
    public const string Adm = nameof(Adm);
    public const string Aluno = nameof(Aluno);
    public const string Professor = nameof(Professor);
    public const string Academico = nameof(Academico);

    public static void AddAuthorizationConfigs(this IServiceCollection services)
    {
        services.AddAuthorization();
    }
}
