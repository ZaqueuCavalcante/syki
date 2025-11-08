namespace Exato.Tests.Unit.Extensions;

public class DocumentExtensionsUnitTests
{
    [Test]
    [Repeat(10)]
    public void Should_return_true_when_cpf_is_valid()
    {
        // Arrange
        var cpf = DataGen.Cpf;

        // Act
        var result = cpf.IsValidCpf();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    [Repeat(10)]
    public void Should_return_true_when_cnpj_is_valid()
    {
        // Arrange
        var cnpj = DataGen.Cnpj;

        // Act
        var result = cnpj.IsValidCnpj();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    [TestCaseSource(nameof(InvalidCpfs))]
    public void Should_return_false_when_cpf_is_invalid(string cpf)
    {
        // Arrange // Act
        var result = cpf.IsValidCpf();

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(nameof(InvalidCnpjs))]
    public void Should_return_false_when_cnpj_is_invalid(string cnpj)
    {
        // Arrange // Act
        var result = cnpj.IsValidCnpj();

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(nameof(Documents))]
    public void Should_return_as_formated_document(string raw, string formated)
    {
        // Arrange // Act
        var result = raw.AsFormatedDocument();

        // Assert
        result.Should().Be(formated);
    }

    private static IEnumerable<object[]> InvalidCpfs()
    {
        List<string> cpfs = [
            "",
            " ",
            "68161",
            "11924528445",
            "600.300.204.26",
        ];
        foreach (var cpf in cpfs)
        {
            yield return [cpf];
        }
    }

    private static IEnumerable<object[]> InvalidCnpjs()
    {
        List<string> cnpjs = [
            "",
            " ",
            "68161",
            "16224745000170",
            "32.905.911/000-192",
        ];
        foreach (var cnpj in cnpjs)
        {
            yield return [cnpj];
        }
    }

    private static IEnumerable<object[]> Documents()
    {
        foreach (var (raw, formated) in new List<(string, string)>()
        {
            ("29088170800", "290.881.708-00"),
            ("11924528444", "119.245.284-44"),
            ("11.924-52.8444", "119.245.284-44"),
            ("09346601000125", "09.346.601/0001-25"),
            ("093-46.60100-0125", "09.346.601/0001-25"),
        })
        {
            yield return [raw, formated];
        }
    }
}
