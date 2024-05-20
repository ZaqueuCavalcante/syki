using Microsoft.AspNetCore.Http;
using Syki.Back.Features.Academic.CreateClass;

namespace Syki.Tests.Base;

public static class TestData
{
    public static string Email => $"{Guid.NewGuid().ToString().OnlyNumbers()}@syki.com";

    public static IEnumerable<object[]> ValidNames()
    {
        foreach (var name in new List<string>() { "Keu", "Maju", "Maria", "Naldinho Silva", })
        {
            yield return new object[] { name };
        }
    }

    public static IEnumerable<object[]> InvalidNames()
    {
        foreach (var name in new List<string>() { null, "", "a", "42", " ", "  ", "     ", "JP", })
        {
            yield return new object[] { name };
        }
    }

    public static IEnumerable<object[]> InvalidUserRegisterTokens()
    {
        var empty = Guid.Empty.ToString();
        var random = Guid.NewGuid().ToString();
        foreach (var name in new List<string>() { null, "", "a", "42", " ", "  ", "     ", "JP", empty, random, })
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

    public static List<string> InvalidEmailsList =
    [
        "",
        " ",
        "zaqueugmail",
        "majuasp.net",
        "#@%^%#$@#$@#.com",
        "@example.com",
        "Joe Smith <email@example.com>",
        "email.example.com",
        "email@example@example.com",
        ".email@example.com",
        "email.@example.com",
        "email..email@example.com",
        "email@example.com (Joe Smith)",
        "email@example",
        "email@-example.com",
        "email@example..com",
        "Abc..123@example.com",
    ];

    public static IEnumerable<object[]> InvalidEmails()
    {
        foreach (var email in InvalidEmailsList)
        {
            yield return new object[] { email };
        }
    }

    public static IEnumerable<object[]> InvalidPasswords()
    {
        foreach (var role in new List<string>()
        {
            "",
            " ",
            "syki",
            "syki123",
            "Syki123",
            "lalal.com",
            "12@3lalala",
            "5816811681816",
        })
        {
            yield return new object[] { role };
        }
    }

    public static IEnumerable<object[]> InvalidMfaTokens()
    {
        foreach (var role in new List<string>()
        {
            "",
            " ",
            "syki",
            "5464",
            "lalal.com",
            "5816811681816",
            Guid.NewGuid().ToHashCode().ToString()[..6]
        })
        {
            yield return new object[] { role };
        }
    }

    public static IEnumerable<object[]> CamelCaseNames()
    {
        foreach (var name in new List<(string, string)>()
        {
            ("", ""),
            (" ", ""),
            (null!, ""),
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
            ("", ""),
            (" ", ""),
            (null!, ""),
            ("ewfewfewf", ""),
            ("629.219.140-00", "62921914000"),
            ("(81) 98578-9526", "81985789526"),
            ("yu2v34y1434u6b54u6b", "23414346546"),
            ("18.297.767/0001-90", "18297767000190"),
        })
        {
            yield return new object[] { text };
        }
    }

    public static IEnumerable<object[]> TextsContains()
    {
        foreach (var text in new List<(string, string, string?)>()
        {
            ("Banco de Dados", "72", null),
            ("Banco de Dados", "72", ""),
            ("Banco de Dados", "72", " "),
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

    public static IEnumerable<object[]> AuditPaths()
    {
        foreach (var text in new List<(PathString, bool)>()
        {
            ("/academic/campi", true),
            ("/academic/courses", true),
            ("/login", false),
            ("/users", false),
            ("/login/mfa", false),
            ("/reset-password", false),
            ("/reset-password-token", false),
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
            (0.23M, "0.23"),
            (9.85M, "9.85"),
            (15.74M, "15.74"),
            (153.87M, "153.87"),
        })
        {
            yield return new object[] { text };
        }
    }

    public static IEnumerable<object[]> CourseTypeEnumToDescription()
    {
        foreach (var text in new List<(CourseType, string)>()
        {
            (CourseType.Bacharelado, "Bacharelado"),
            (CourseType.Licenciatura, "Licenciatura"),
            (CourseType.Tecnologo, "Tecnólogo"),
            (CourseType.Especializacao, "Especialização"),
            (CourseType.Mestrado, "Mestrado"),
            (CourseType.Doutorado, "Doutorado"),
            (CourseType.PosDoutorado, "Pós-Doutorado"),
        })
        {
            yield return new object[] { text };
        }
    }

    public static IEnumerable<object[]> CourseTypeEnumForIsIn()
    {
        foreach (var text in new List<(Enum, bool)>()
        {
            (CourseType.Bacharelado, true),
            (StudentDisciplineStatus.Matriculado, false),
            (CourseType.Tecnologo, true),
            (Shift.Vespertino, false),
        })
        {
            yield return new object[] { text };
        }
    }

    public static IEnumerable<object[]> ConflictingSchedules()
    {
        foreach (var list in new List<List<Schedule>>()
        {
            new() {
                new Schedule(Day.Segunda, Hour.H07_00, Hour.H08_00),
                new Schedule(Day.Segunda, Hour.H07_30, Hour.H07_45),
            },
            new() {
                new Schedule(Day.Segunda, Hour.H10_00, Hour.H11_00),
                new Schedule(Day.Segunda, Hour.H09_30, Hour.H12_15),
            },
            new() {
                new Schedule(Day.Segunda, Hour.H07_00, Hour.H08_00),
                new Schedule(Day.Segunda, Hour.H07_30, Hour.H08_30),
            },
            new() {
                new Schedule(Day.Segunda, Hour.H07_30, Hour.H08_30),
                new Schedule(Day.Segunda, Hour.H07_00, Hour.H08_00),
            },
            new() {
                new Schedule(Day.Segunda, Hour.H07_00, Hour.H08_00),
                new Schedule(Day.Terca, Hour.H08_00, Hour.H09_00),
                new Schedule(Day.Segunda, Hour.H07_15, Hour.H07_45),
            },
            new() {
                new Schedule(Day.Quarta, Hour.H12_00, Hour.H15_30),
                new Schedule(Day.Quarta, Hour.H13_00, Hour.H14_15),
            },
        })
        {
            yield return new object[] { list };
        }
    }

    public static IEnumerable<object[]> ValidSchedules()
    {
        foreach (var list in new List<List<Schedule>>()
        {
            new() {
                new Schedule(Day.Segunda, Hour.H07_00, Hour.H08_00),
                new Schedule(Day.Segunda, Hour.H08_00, Hour.H09_00),
            },
            new() {
                new Schedule(Day.Segunda, Hour.H08_00, Hour.H09_00),
                new Schedule(Day.Segunda, Hour.H07_00, Hour.H08_00),
            },
            new() {
                new Schedule(Day.Segunda, Hour.H07_00, Hour.H08_00),
                new Schedule(Day.Terca, Hour.H08_00, Hour.H09_00),
                new Schedule(Day.Segunda, Hour.H09_45, Hour.H10_15),
            },
            new() {
                new Schedule(Day.Quarta, Hour.H12_00, Hour.H15_30),
                new Schedule(Day.Quarta, Hour.H11_15, Hour.H12_00),
            },
        })
        {
            yield return new object[] { list };
        }
    }
}
