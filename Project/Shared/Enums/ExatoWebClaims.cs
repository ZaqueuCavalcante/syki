namespace Exato.Shared.Enums;

/// <summary>
/// Claims do Exato Web.
/// </summary>
public enum ExatoWebClaims
{
    [Description("Consultas")]
    Consultas = 0,

    [Description("Consulta em Lotes")]
    ConsultaEmLotes = 1,

    [Description("Reaproveitamento de Consultas")]
    ReaproveitamentoDeConsultas = 2,

    [Description("Gestão de Usuários")]
    GestaoDeUsuarios = 3,

    [Description("DocCheck")]
    DocCheck = 4,

    [Description("DocCheck Apenas Leitura")]
    DocCheckApenasLeitura = 5,

    [Description("DocCheck Admin")]
    DocCheckAdmin = 6,

    [Description("DocCheck Dashboard")]
    DocCheckDashboard = 7,

    [Description("DocCheck Gerar URL")]
    DocCheckGerarURL = 8,

    [Description("Bradesco RH")]
    BradescoRH = 100,
}
