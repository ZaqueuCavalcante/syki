namespace Exato.Shared.Features.Office.EditarSaldoDaEmpresa;

public class EditarSaldoDaEmpresaIn : IApiDto<EditarSaldoDaEmpresaIn>
{
    /// <summary>
    /// Valor em R$ que deve ser creditado ou debitado.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Quantidade de créditos que devem ser adicionados ou removidos.
    /// </summary>
    public int Credits { get; set; }

    public static IEnumerable<(string, EditarSaldoDaEmpresaIn)> GetExamples() =>
    [
        ("Exemplo", new EditarSaldoDaEmpresaIn() { }),
    ];
}
