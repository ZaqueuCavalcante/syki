using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.CreateAcademicPeriod;

namespace Syki.Tests.Base;

public static class TestData
{
    public static string Email => $"{Guid.CreateVersion7().ToString().OnlyNumbers()}@syki.com";

    public static IEnumerable<object[]> ValidNames()
    {
        foreach (var name in new List<string>() { "Keu", "Maju", "Maria", "Naldinho Silva", })
        {
            yield return [name];
        }
    }

    public static IEnumerable<object[]> InvalidNames()
    {
        foreach (var name in new List<string>() { null, "", "a", "42", " ", "  ", "     ", "JP", })
        {
            yield return [name];
        }
    }

    public static IEnumerable<object[]> InvalidUserRegisterTokens()
    {
        var empty = Guid.Empty.ToString();
        var random = Guid.CreateVersion7().ToString();
        foreach (var name in new List<string>() { null, "", "a", "42", "qwerty123", " ", "  ", "     ", "JP", empty, random, })
        {
            yield return [name];
        }
    }

    public static IEnumerable<object[]> InvalidPeriods()
    {
        foreach (var id in new List<string>() { null, "", "   ", "lalala", "1969.1", "1970.3", "1970.0", "1971.90", "2001", "202", "2023.9", "2070.0", })
        {
            yield return [id];
        }
    }

    public static IEnumerable<object[]> ValidPeriods()
    {
        foreach (var id in new List<string>() { "1970.1", "1970.2", "2023.1", "2023.2", "2070.1", "2070.2", })
        {
            yield return [id];
        }
    }

    public static IEnumerable<object[]> InvalidEmails()
    {
        List<string> emails = [
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
        foreach (var email in emails)
        {
            yield return [email];
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
            yield return [role];
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
            "123456",
            "lalal.com",
            "5816811681816",
        })
        {
            yield return [role];
        }
    }

    public static IEnumerable<object[]> CamelCaseNames()
    {
        foreach (var (camel, snake) in new List<(string, string)>()
        {
            ("", ""),
            (" ", ""),
            (null!, ""),
            ("AspNetUsers", "asp_net_users"),
            ("AspNetUserRoles", "asp_net_user_roles"),
            ("AspNetRoleClaims", "asp_net_role_claims"),
        })
        {
            yield return [camel, snake];
        }
    }

    public static IEnumerable<object[]> GuidsToHashCodes()
    {
        foreach (var (guid, hashCode) in new List<(Guid, int)>()
        {
            (Guid.Parse("e2e833ce-9eee-4755-96be-66c52d7dc260"), 6652_7260),
            (Guid.Parse("bab5f379-ac8b-446d-9325-13d18cd42227"), 3184_2227),
            (Guid.Parse("439f1f9d-5be0-4456-8364-a2a2391953bb"), 2239_1953),
            (Guid.Parse("0197428e-be72-7d8a-94d3-6cf24a0e55f4"), 3624_0554)
        })
        {
            yield return [guid, hashCode];
        }
    }

    public static IEnumerable<object[]> FormatedStrings()
    {
        foreach (var (text, numbers) in new List<(string, string)>()
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
            yield return [text, numbers];
        }
    }

    public static IEnumerable<object[]> TextsContains()
    {
        foreach (var (text1, text2, search) in new List<(string, string, string?)>()
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
            yield return [text1, text2, search!];
        }
    }

    public static IEnumerable<object[]> TextsNotContains()
    {
        foreach (var (text1, text2, search) in new List<(string, string, string)>()
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
            yield return [text1, text2, search!];
        }
    }

    public static IEnumerable<object[]> DecimalsStringsForFormat()
    {
        foreach (var (number, text) in new List<(decimal, string)>()
        {
            (0.00M, "0.00"),
            (0.23M, "0.23"),
            (9.85M, "9.85"),
            (15.74M, "15.74"),
            (153.87M, "153.87"),
        })
        {
            yield return [number, text];
        }
    }

    public static IEnumerable<object[]> CourseTypeEnumToDescription()
    {
        foreach (var (type, description) in new List<(CourseType, string)>()
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
            yield return [type, description];
        }
    }

    public static IEnumerable<object[]> CourseTypeEnumForIsIn()
    {
        foreach (var (value, isIn) in new List<(Enum, bool)>()
        {
            (CourseType.Bacharelado, true),
            (StudentDisciplineStatus.Matriculado, false),
            (CourseType.Tecnologo, true),
            (Shift.Vespertino, false),
        })
        {
            yield return [value, isIn];
        }
    }

    public static IEnumerable<object[]> EnumsInvalidValues()
    {
        foreach (var value in new List<Enum>()
        {
            (Day)69,
            (Hour)69,
            (UserRole)69,
            (CourseType)69,
            (ClassStatus)69,
            (StudentDisciplineStatus)(-69),
        })
        {
            yield return [value];
        }
    }

    public static IEnumerable<Schedule[]> ConflictingSchedules()
    {
        yield return new[]
        {
            new Schedule(Day.Monday, Hour.H07_00, Hour.H08_00),
            new Schedule(Day.Monday, Hour.H07_30, Hour.H07_45),
        };

        yield return new[]
        {
            new Schedule(Day.Monday, Hour.H10_00, Hour.H11_00),
            new Schedule(Day.Monday, Hour.H09_30, Hour.H12_15),
        };

        yield return new[]
        {
            new Schedule(Day.Monday, Hour.H07_00, Hour.H08_00),
            new Schedule(Day.Monday, Hour.H07_30, Hour.H08_30),
        };

        yield return new[]
        {
            new Schedule(Day.Monday, Hour.H07_30, Hour.H08_30),
            new Schedule(Day.Monday, Hour.H07_00, Hour.H08_00),
        };

        yield return new[]
        {
            new Schedule(Day.Monday, Hour.H07_00, Hour.H08_00),
            new Schedule(Day.Tuesday, Hour.H08_00, Hour.H09_00),
            new Schedule(Day.Monday, Hour.H07_15, Hour.H07_45),
        };

        yield return new[]
        {
            new Schedule(Day.Wednesday, Hour.H12_00, Hour.H15_30),
            new Schedule(Day.Wednesday, Hour.H13_00, Hour.H14_15),
        };
    }

    public static IEnumerable<Schedule[]> ValidSchedules()
    {
        yield return new[]
        {
            new Schedule(Day.Monday, Hour.H07_00, Hour.H08_00),
            new Schedule(Day.Monday, Hour.H08_00, Hour.H09_00),
        };

        yield return new[]
        {
            new Schedule(Day.Monday, Hour.H08_00, Hour.H09_00),
            new Schedule(Day.Monday, Hour.H07_00, Hour.H08_00),
        };

        yield return new[]
        {
            new Schedule(Day.Monday, Hour.H07_00, Hour.H08_00),
            new Schedule(Day.Tuesday, Hour.H08_00, Hour.H09_00),
            new Schedule(Day.Monday, Hour.H09_45, Hour.H10_15),
        };

        yield return new[]
        {
            new Schedule(Day.Wednesday, Hour.H12_00, Hour.H15_30),
            new Schedule(Day.Wednesday, Hour.H11_15, Hour.H12_00),
        };
    }

    public static IEnumerable<object[]> Holidays()
    {
        foreach (var day in new List<DateOnly>()
        {
            new(2024, 01, 01), // Confraternização Universal
            new(2024, 04, 21), // Tiradentes
            new(2024, 05, 01), // Dia do Trabalho
            new(2024, 09, 07), // Independência do Brasil
            new(2024, 10, 12), // Nossa Senhora Aparecida
            new(2024, 11, 02), // Finados
            new(2024, 11, 15), // Proclamação da República
            new(2024, 12, 25), // Natal
        })
        {
            yield return [day];
        }
    }

    public static IEnumerable<object[]> NotHolidays()
    {
        foreach (var day in new List<DateOnly>()
        {
            new(2024, 01, 02),
        })
        {
            yield return [day];
        }
    }

    public static IEnumerable<object[]> HoursDiffsInMinutes()
    {
        foreach (var (hourA, hourB, diff) in new List<(Hour, Hour, int)>()
        {
            (Hour.H07_00, Hour.H07_00, 00),
            (Hour.H07_00, Hour.H07_15, 15),
            (Hour.H07_00, Hour.H07_45, 45),
            (Hour.H07_00, Hour.H08_00, 01*60),
            (Hour.H07_15, Hour.H08_30, 01*60+15),
            (Hour.H07_15, Hour.H08_45, 01*60+30),
            (Hour.H07_30, Hour.H08_30, 01*60),
            (Hour.H07_00, Hour.H12_00, 05*60),
            (Hour.H13_00, Hour.H19_00, 06*60),
            (Hour.H07_15, Hour.H09_45, 02*60+30),
            (Hour.H08_45, Hour.H12_00, 03*60+15),
            (Hour.H08_15, Hour.H15_45, 07*60+30),
            (Hour.H07_15, Hour.H07_00, 15),
            (Hour.H19_00, Hour.H13_00, 06*60),
            (Hour.H08_30, Hour.H07_15, 01*60+15),
            (Hour.H08_45, Hour.H07_15, 01*60+30),
        })
        {
            yield return [hourA, hourB, diff];
        }
    }

    public static IEnumerable<object[]> MinutesForFormat()
    {
        foreach (var (minutes, text) in new List<(int, string)>()
        {
            (0, "0"),
            (15, "15min"),
            (30, "30min"),
            (45, "45min"),
            (60, "1h"),
            (90, "1h e 30min"),
            (105, "1h e 45min"),
            (135, "2h e 15min"),
        })
        {
            yield return [minutes, text];
        }
    }

    public static IEnumerable<object[]> ValidNotes()
    {
        foreach (var note in new List<decimal>()
        {
            0.00M,
            5.67M,
            10.00M,
        })
        {
            yield return [note];
        }
    }

    public static IEnumerable<object[]> InvalidNotes()
    {
        foreach (var note in new List<decimal>()
        {
            -0.01M,
            10.01M,
        })
        {
            yield return [note];
        }
    }

    public static IEnumerable<object[]> ClassActivityValidWeights()
    {
        foreach (var weight in new List<int>()
        {
            0, 1, 10, 60, 99, 100,
        })
        {
            yield return [weight];
        };
    }

    public static IEnumerable<object[]> ClassActivityInvalidWeights()
    {
        foreach (var weight in new List<int>()
        {
            -10, -1, 101, 110,
        })
        {
            yield return [weight];
        };
    }

    public static IEnumerable<int[]> ClassActivityValidWeightsLists()
    {
        yield return new[] { 0 };
        yield return new[] { 1 };
        yield return new[] { 60 };
        yield return new[] { 99 };
        yield return new[] { 100 };
        yield return new[] { 0, 0 };
        yield return new[] { 0, 50 };
        yield return new[] { 0, 100 };
        yield return new[] { 100, 0 };
        yield return new[] { 10, 20 };
        yield return new[] { 70, 30 };
        yield return new[] { 0, 0, 0 };
        yield return new[] { 50, 50, 0 };
        yield return new[] { 50, 10, 40 };
        yield return new[] { 10, 80, 10 };
        yield return new[] { 33, 33, 34 };
        yield return new[] { 0, 0, 0, 0 };
        yield return new[] { 0, 0, 50, 40 };
        yield return new[] { 25, 25, 25, 25 };
    }

    public static IEnumerable<int[]> ClassActivityInvalidWeightsLists()
    {
        yield return new[] { 100, 1 };
        yield return new[] { 100, 1 };
        yield return new[] { 50, 51 };
        yield return new[] { 100, 100 };
        yield return new[] { 99, 2 };
        yield return new[] { 0, 80, 21 };
        yield return new[] { 50, 50, 1 };
        yield return new[] { 90, 5, 6 };
        yield return new[] { 90, 5, 5, 1 };
        yield return new[] { 0, 50, 45, 6 };
    }

    public static Class GetClass()
    {
        var institutionId = Guid.CreateVersion7();
        var disciplineId = Guid.CreateVersion7();
        var teacherId = Guid.CreateVersion7();
        const string period = "2024.2";
        const int vacancies = 40;

        var @class = Class.New(institutionId, disciplineId, teacherId, period, vacancies, []).Success;

        return @class;
    }

    public static Class GetClass(string start, string end, List<Schedule> schedules)
    {
        var institutionId = Guid.CreateVersion7();
        var disciplineId = Guid.CreateVersion7();
        var teacherId = Guid.CreateVersion7();
        const string period = "2024.2";
        const int vacancies = 40;

        var @class = Class.New(institutionId, disciplineId, teacherId, period, vacancies, schedules).Success;
        @class.Period = AcademicPeriod.New(period, institutionId, start.ToDateOnly(), end.ToDateOnly());

        return @class;
    }
}
