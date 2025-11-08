using Bogus;
using Exato.Shared.Features.Office.BuscarEmpresas;

namespace Exato.Tests.Unit.Extensions;

public class StringExtensionsUnitTests
{
    [Test]
    [TestCaseSource(nameof(CamelCaseNames))]
    public void Should_convert_to_snake_case(string camel, string snake)
    {
        // Arrange / Act
        var result = camel.ToSnakeCase();

        // Assert
        result.Should().Be(snake);
    }

    [Test]
    [Repeat(100)]
    public void Should_return_true_when_email_is_valid()
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
    public void Should_return_false_when_email_is_invalid(string email)
    {
        // Arrange // Act
        var result = email.IsValidEmail();

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(nameof(RawTokens))]
    public void Should_return_masked_tokens(string raw, string masked)
    {
        // Arrange // Act
        var result = raw.ToMaskedToken();

        // Assert
        result.Should().Be(masked);
    }

    [Test]
    public void Should_add_query_string()
    {
        // Arrange
        var data = new BuscarEmpresasIn
        {
            Page = 3,
            IsActive = true,
            Search = "uber",
            PaymentMethod = MetodoDePagamento.PrePago,
        };

        // Act
        var result = "office/empresas".AddQueryString(data);

        // Assert
        result.Should().Be("office/empresas?Page=3&IsActive=True&Search=uber&PaymentMethod=PrePago");
    }

    private static IEnumerable<object[]> RawTokens()
    {
        foreach (var (raw, masked) in new List<(string, string)>()
        {
            ("6a5468e6192e4124b6f47691e7924b25", "6a54*****924b25"),
            ("69657192-8d7f-488f-a7bf-ec59d924026b", "6965*****24026b"),
            ("e80001e7-3a5a-4ec7-9b07-e5f15f2bd6c6", "e800*****2bd6c6"),
        })
        {
            yield return [raw, masked];
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
}
