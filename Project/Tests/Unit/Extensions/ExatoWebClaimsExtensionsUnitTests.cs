using Exato.Web.Extensions;

namespace Exato.Tests.Unit.Extensions;

public class ExatoWebClaimsExtensionsUnitTests
{
    [Test]
    public void Should_return_empty_list_when_input_is_empty()
    {
        // Arrange
        List<ExatoWebClaims> input = [];

        // Act
        var result = input.ToPermissions();

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    [TestCaseSource(nameof(ClaimsMap))]
    public void Should_convert_claims_to_permissions(List<ExatoWebClaims> input, List<string> expected)
    {
        // Arrange / Act
        var result = input.ToPermissions();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    [TestCaseSource(nameof(PermissionsMap))]
    public void Should_convert_permissions_to_claims(List<string> input, List<ExatoWebClaims> expected)
    {
        // Arrange / Act
        var result = input.ToClaims();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    private static IEnumerable<object[]> ClaimsMap()
    {
        foreach (var (input, expected) in new List<(List<ExatoWebClaims>, List<string>)>()
        {
            ([ExatoWebClaims.Consultas], ["consultas"]),
            ([ExatoWebClaims.ConsultaEmLotes], ["consultas_lote", "consultas"]),
            ([ExatoWebClaims.ReaproveitamentoDeConsultas], ["consultas_reaproveitamento", "consultas"]),
            ([ExatoWebClaims.GestaoDeUsuarios], ["consultas_usuarios"]),
            ([ExatoWebClaims.DocCheck], ["doccheck"]),
            ([ExatoWebClaims.DocCheckApenasLeitura], ["doccheck_readonly", "doccheck"]),
            ([ExatoWebClaims.DocCheckAdmin], ["doccheck_admin", "doccheck"]),
            ([ExatoWebClaims.DocCheckDashboard], ["doccheck_dashboard", "doccheck"]),
            ([ExatoWebClaims.DocCheckGerarURL], ["doccheck_gerar_url", "doccheck"]),

            ([ExatoWebClaims.Consultas, ExatoWebClaims.ConsultaEmLotes], ["consultas", "consultas_lote"]),
            ([ExatoWebClaims.Consultas, ExatoWebClaims.ConsultaEmLotes, ExatoWebClaims.ReaproveitamentoDeConsultas], ["consultas", "consultas_lote", "consultas_reaproveitamento"]),
            ([ExatoWebClaims.Consultas, ExatoWebClaims.BradescoRH, ExatoWebClaims.DocCheckAdmin], ["consultas", "bradescorh", "doccheck_admin", "doccheck"]),

            ([ExatoWebClaims.BradescoRH], ["bradescorh"]),

            (Enum.GetValues<ExatoWebClaims>().ToList(), [
                "consultas",
                "consultas_lote",
                "consultas_reaproveitamento",
                "consultas_usuarios",
                "doccheck",
                "doccheck_readonly",
                "doccheck_admin",
                "doccheck_dashboard",
                "doccheck_gerar_url",
                "bradescorh",
            ]),
        })
        {
            yield return [input, expected];
        }
    }

    private static IEnumerable<object[]> PermissionsMap()
    {
        foreach (var (input, expected) in new List<(List<string>, List<ExatoWebClaims>)>()
        {
            (["consultas"], [ExatoWebClaims.Consultas]),
            (["consultas_lote", "consultas"], [ExatoWebClaims.Consultas, ExatoWebClaims.ConsultaEmLotes]),

            (["consultas_reaproveitamento", "consultas"], [ExatoWebClaims.Consultas, ExatoWebClaims.ReaproveitamentoDeConsultas]),
            (["bradescorh"], [ExatoWebClaims.BradescoRH]),
            (["doccheck"], [ExatoWebClaims.DocCheck]),
            (["doccheck_readonly", "doccheck"], [ExatoWebClaims.DocCheckApenasLeitura, ExatoWebClaims.DocCheck]),
     
            ([
                "consultas",
                "consultas_lote",
                "consultas_reaproveitamento",
                "consultas_usuarios",
                "doccheck",
                "doccheck_readonly",
                "doccheck_admin",
                "doccheck_dashboard",
                "doccheck_gerar_url",
                "bradescorh",
            ],
            Enum.GetValues<ExatoWebClaims>().ToList()),
        })
        {
            yield return [input, expected];
        }
    }
}
