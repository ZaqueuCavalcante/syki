using Syki.Domain;

namespace Syki.Database;

public static class DbSeed
{
    public static Faculdade NovaRoma = new()
    {
        Nome = "Nova Roma",
        Campi = new()
        {
            new Campus { Nome = "Caruaru" },
            new Campus { Nome = "Recife" },
        },
        Cursos = new()
        {
            new Curso { Nome = "Análise e Desenvolvimento de Sistemas" },
            new Curso { Nome = "Direito" },
            new Curso { Nome = "Pedagogia" },
            new Curso { Nome = "Engenharia Civil" },
        },
    };

    public static List<Faculdade> Faculdades = new()
    {
        new Faculdade("Nova Roma"),
        new Faculdade("UPE"),
        new Faculdade("UFPE"),
        new Faculdade("UFRPE"),
        new Faculdade("IFPE"),
        new Faculdade("USP"),
        new Faculdade("ITA"),
        new Faculdade("Unicamp"),
    };
}
