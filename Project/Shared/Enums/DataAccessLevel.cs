namespace Exato.Shared.Enums;

/// <summary>
/// Níveis de acesso a dados do Intelligence.
/// </summary>
public enum DataAccessLevel
{
    [Description("Dados Mascarados")]
    DadosMascarados = 0,

    [Description("Dados de Cadastro Básico")]
    DadosDeCadastroBasico = 1,

    [Description("Dados de Cadastro Completo")]
    DadosDeCadastroCompleto = 2,
}
