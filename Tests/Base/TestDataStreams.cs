using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Base;

public static class TestDataStreams
{
    public static IEnumerable<object[]> ValidNames()
    {
        foreach (var name in new List<string>() { "Keu", "Maju", "Maria", "Naldinho Silva", })
        {
            yield return new object[] { name };
        }
    }

    public static IEnumerable<object[]> InvalidNames()
    {
        foreach (var name in new List<string>() { null, "", "a", " ", "  ", "     ", "JP", })
        {
            yield return new object[] { name };
        }
    }

    public static IEnumerable<object[]> InvalidPeriods()
    {
        foreach (var id in new List<string>() { null, "", "   ", "lalala", "1969.1", "1970.3", "1970.0", "1971.90", "2001", "202", "2023.9", "2070.0", })
        {
            yield return new object[] { id };
        }
    }

    public static IEnumerable<object[]> ValidPeriods()
    {
        foreach (var id in new List<string>() { "1970.1", "1970.2", "2023.1", "2023.2", "2070.1", "2070.2", })
        {
            yield return new object[] { id };
        }
    }

    public static IEnumerable<object[]> AllUsersRoles()
    {
        foreach (var role in AllRoles)
        {
            yield return new object[] { role };
        }
    }

    public static IEnumerable<object[]> AllRolesExceptAcademico()
    {
        foreach (var role in AllRoles)
        {
            if (role == Academico) continue;
            yield return new object[] { role };
        }
    }

    public static IEnumerable<object[]> AllRolesExceptAdm()
    {
        foreach (var role in AllRoles)
        {
            if (role == Adm) continue;
            yield return new object[] { role };
        }
    }

    public static IEnumerable<object[]> AllRolesExceptAluno()
    {
        foreach (var role in AllRoles)
        {
            if (role == Aluno) continue;
            yield return new object[] { role };
        }
    }

    public static IEnumerable<object[]> CamelCaseNames()
    {
        foreach (var name in new List<(string, string)>()
        {
            ("AspNetUsers", "asp_net_users"),
            ("AspNetUserRoles", "asp_net_user_roles"),
            ("AspNetRoleClaims", "asp_net_role_claims"),
        })
        {
            yield return new object[] { name };
        }
    }

    public static IEnumerable<object[]> GuidsToHashCodes()
    {
        foreach (var name in new List<(Guid, int)>()
        {
            (Guid.Parse("e2e833ce-9eee-4755-96be-66c52d7dc260"), 2833_9475),
            (Guid.Parse("bab5f379-ac8b-446d-9325-13d18cd42227"), 5379_8446),
            (Guid.Parse("439f1f9d-5be0-4456-8364-a2a2391953bb"), 4391_9504),
        })
        {
            yield return new object[] { name };
        }
    }

    public static IEnumerable<object[]> FormatedStrings()
    {
        foreach (var text in new List<(string, string)>()
        {
            ("629.219.140-00", "62921914000"),
            ("18.297.767/0001-90", "18297767000190"),
            ("yu2v34y1434u6b54u6b", "23414346546"),
            ("(81) 98578-9526", "81985789526"),
        })
        {
            yield return new object[] { text };
        }
    }

    public static IEnumerable<object[]> TextsContains()
    {
        foreach (var text in new List<(string, string, string)>()
        {
            ("Banco de Dados", "72", "dados"),
            ("Banco de Dados", "72", "72"),
            ("Informática e Sociedade", "Chat GPT", "Informáti"),
            ("Informática e Sociedade", "Chat GPT", "chat"),
            ("Estatística Aplicada", "50", "estatística"),
            ("Computação em Nuvem e Web Services", "web", "nuvem"),
            ("Projeto Integrador I: Concepção e Prototipação", "60", "Projeto Integrador I"),
            ("Direito e Economia", "", "econo"),
            ("Sociologia Jurídica", "leis", "socio"),
            ("Direito Empresarial II", "80", "II"),
            ("Direito Empresarial II", "80", "80"),
            ("Monografia Final", "final", "grafia"),
            ("3681515", "6816", "515"),
            ("8681918485", "Lalala", "681"),
            ("1681616851651", "57681", "6168"),
            ("6841861", "68416", "84"),
        })
        {
            yield return new object[] { text };
        }
    }

    public static IEnumerable<object[]> TextsNotContains()
    {
        foreach (var text in new List<(string, string, string)>()
        {
            ("Banco de Dados", "72", "objeto"),
            ("Banco de Dados", "72", "sociedade"),
            ("Informática e Sociedade", "Chat GPT", "bard"),
            ("Informática e Sociedade", "Chat GPT", "dados"),
            ("Estatística Aplicada", "50", "51"),
            ("Computação em Nuvem e Web Services", "web", "mobile"),
            ("Projeto Integrador I: Concepção e Prototipação", "60", "II"),
            ("Direito e Economia", "", "80"),
            ("Sociologia Jurídica", "leis", "poo"),
            ("Direito Empresarial II", "80", "socio"),
            ("Direito Empresarial II", "80", "leis"),
            ("Monografia Final", "final", "tcc"),
            ("3681515", "6816", "8641"),
            ("8681918485", "Lalala", "0"),
            ("1681616851651", "57681", "684"),
            ("6841861", "68416", "68419"),
        })
        {
            yield return new object[] { text };
        }
    }

    public static IEnumerable<object[]> DecimalsStringsForFormat()
    {
        foreach (var text in new List<(decimal, string)>()
        {
            (0.00M, "0.00"),
            (9.85M, "9.85"),
            (0.23M, "0.23"),
            (15.00M, "15.00"),
        })
        {
            yield return new object[] { text };
        }
    }
}
