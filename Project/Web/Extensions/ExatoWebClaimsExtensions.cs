using Exato.Shared.Enums;
using System.Collections.Immutable;

namespace Exato.Web.Extensions;

public static class ExatoWebClaimsExtensions
{
    public static readonly ImmutableDictionary<ExatoWebClaims, List<string>> Dict =
        new Dictionary<ExatoWebClaims, List<string>>
        {
            [ExatoWebClaims.Consultas] = ["consultas"],
            [ExatoWebClaims.ConsultaEmLotes] = ["consultas_lote", "consultas"],
            [ExatoWebClaims.ReaproveitamentoDeConsultas] = ["consultas_reaproveitamento", "consultas"],
            [ExatoWebClaims.GestaoDeUsuarios] = ["consultas_usuarios"],
            [ExatoWebClaims.DocCheck] = ["doccheck"],
            [ExatoWebClaims.DocCheckApenasLeitura] = ["doccheck_readonly", "doccheck"],
            [ExatoWebClaims.DocCheckAdmin] = ["doccheck_admin", "doccheck"],
            [ExatoWebClaims.DocCheckDashboard] = ["doccheck_dashboard", "doccheck"],
            [ExatoWebClaims.DocCheckGerarURL] = ["doccheck_gerar_url", "doccheck"],
            [ExatoWebClaims.BradescoRH] = ["bradescorh"],
        }
        .ToImmutableDictionary();

    public static List<string> ToPermissions(this List<ExatoWebClaims> claims)
    {
        if (claims == null || claims.Count == 0) return [];

        var result = new List<string>();

        foreach (var claim in claims)
        {
            result.AddRange(Dict[claim]);
        }

        return result.Distinct().ToList();
    }

    public static List<ExatoWebClaims> ToClaims(this List<string> permissions)
    {
        if (permissions == null || permissions.Count == 0) return [];

        var set = permissions.ToHashSet();

        var result = Dict
            .Where(kvp => kvp.Value.All(alias => set.Contains(alias)))
            .OrderBy(kvp => kvp.Value.Count)
            .Select(kvp => kvp.Key)
            .ToList();

        return result;
    }
}
