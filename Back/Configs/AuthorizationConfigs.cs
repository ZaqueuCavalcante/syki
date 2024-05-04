namespace Syki.Back.Configs;

public static class AuthorizationConfigs
{
    public const string Adm = nameof(Adm);
    public const string Academico = nameof(Academico);
    public const string Professor = nameof(Professor);
    public const string Aluno = nameof(Aluno);
    public static List<string> AllRoles = [Adm, Academico, Professor, Aluno];

    public static void AddAuthorizationConfigs(this IServiceCollection services)
    {
        services.AddAuthorization();
    }
}
