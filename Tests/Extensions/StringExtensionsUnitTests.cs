using Bogus;

namespace Syki.Tests.Extensions;

public class StringExtensionsUnitTests
{
    [Test]
    [TestCaseSource(nameof(CamelCaseNames))]
    public void StringExtensions_Should_change_to_snake_case(string camel, string snake)
    {
        // Arrange / Act
        var result = camel.ToSnakeCase();

        // Assert
        result.Should().Be(snake);
    }

    [Test]
    [TestCaseSource(nameof(FormatedStrings))]
    public void StringExtensions_Should_change_to_only_numbers(string text, string numbers)
    {
        // Arrange / Act
        var result = text.OnlyNumbers();

        // Assert
        result.Should().Be(numbers);
    }

    [Test]
    [TestCaseSource(nameof(TextsContains))]
    public void StringExtensions_Should_return_true_because_serch_is_inside_some_text(string text1, string text2, string? search)
    {
        // Arrange / Act
        var result = search.IsIn(text1, text2);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    [TestCaseSource(nameof(TextsNotContains))]
    public void StringExtensions_Should_return_false_because_serch_is_not_inside_some_text(string text1, string text2, string search)
    {
        // Arrange / Act
        var result = search.IsIn(text1, text2);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(nameof(DecimalsStringsForFormat))]
    public void StringExtensions_Should_format_decimal_as_string(decimal number, string text)
    {
        // Arrange / Act
        var result = number.Format();

        // Assert
        result.Should().Be(text);
    }

    [Test]
    [Repeat(100)]
    public void StringExtensions_Should_return_true_when_email_is_valid()
    {
        // Arrange
        var email = new Faker().Internet.Email();

        // Act
        var result = email.IsValidEmail();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    [TestCaseSource(nameof(InvalidEmails))]
    public void StringExtensions_Should_return_false_when_email_is_invalid(string email)
    {
        // Arrange // Act
        var result = email.IsValidEmail();

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(nameof(MinutesForFormat))]
    public void StringExtensions_Should_format_minutes_as_string(int minutes, string text)
    {
        // Arrange / Act
        var result = minutes.MinutesToString();

        // Assert
        result.Should().Be(text);
    }

    private static IEnumerable<object[]> CamelCaseNames()
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

    private static IEnumerable<object[]> FormatedStrings()
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

    private static IEnumerable<object[]> TextsContains()
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

    private static IEnumerable<object[]> TextsNotContains()
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

    private static IEnumerable<object[]> DecimalsStringsForFormat()
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

    private static IEnumerable<object[]> InvalidEmails()
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

    private static IEnumerable<object[]> MinutesForFormat()
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
}
