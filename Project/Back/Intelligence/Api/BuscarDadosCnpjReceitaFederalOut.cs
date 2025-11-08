namespace Exato.Back.Intelligence.Api;

public class BuscarDadosCnpjReceitaFederalOut
{
    public TransactionResultType TransactionResultType { get; set; }
    public BuscarDadosCnpjReceitaFederalResultOut Result { get; set; }
}

public class BuscarDadosCnpjReceitaFederalResultOut
{
    public string? NomeFantasia { get; set; }
    public string? NomeEmpresarial { get; set; }
    public string? AtividadeEconomicaPrincipalCodigoNumeroNorm { get; set; }
}
