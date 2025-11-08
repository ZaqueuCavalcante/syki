using Microsoft.EntityFrameworkCore;

namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_adicionar_creditos_ao_saldo_da_empresa()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cnpj = DataGen.Cnpj;
        var nome = DataGen.OrgName;
        var id = (await client.CriarEmpresa(nome, cnpj, $"{nome} LTDA.")).Success.Id;

        var ctx = _back.GetBackDbContext();
        await ctx.Database.ExecuteSqlAsync($"UPDATE public.cliente SET faturamento_tipo_id = 1 WHERE cliente_id = {id}");

        // Act
        var response = await client.EditarSaldoDaEmpresa(id, 0, 1234);

        // Assert
        var empresa = await ctx.PublicCliente.FirstAsync(x => x.ClienteId == id);

        empresa.Saldo.Should().Be(1234);
        empresa.BalanceInBrl.Should().Be(0);
        empresa.BalanceType.Should().Be(BalanceType.Creditos.ToShort());
        empresa.FaturamentoTipoId.Should().Be(MetodoDePagamento.PrePago.ToShort());
    }

    [Test]
    public async Task Deve_adicionar_reais_ao_saldo_da_empresa()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cnpj = DataGen.Cnpj;
        var nome = DataGen.OrgName;
        var id = (await client.CriarEmpresa(nome, cnpj, $"{nome} LTDA.")).Success.Id;

        await client.EditarFaturamentoDaEmpresa(
            id: id,
            habilitado: false,
            metodoDePagamento: MetodoDePagamento.PrePago
        );

        // Act
        var response = await client.EditarSaldoDaEmpresa(id, 85.49M, 0);

        // Assert
        var ctx = _back.GetBackDbContext();
        var empresa = await ctx.PublicCliente.FirstAsync(x => x.ClienteId == id);

        empresa.Saldo.Should().Be(0);
        empresa.BalanceInBrl.Should().Be(85.49M);
        empresa.BalanceType.Should().Be(BalanceType.Reais.ToShort());
        empresa.FaturamentoTipoId.Should().Be(MetodoDePagamento.PrePago.ToShort());
    }

    [Test]
    public async Task Nao_deve_editar_o_saldo_da_empresa_quando_ela_nao_existir()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        // Act
        var response = await client.EditarSaldoDaEmpresa(1561561, 85.49M, 0);

        // Assert
        response.ShouldBeError(EmpresaNaoEncontrada.I);
    }

    [Test]
    public async Task Deve_adicionar_creditos_ao_saldo_da_empresa_em_requests_concorrentes()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cnpj = DataGen.Cnpj;
        var nome = DataGen.OrgName;
        var id = (await client.CriarEmpresa(nome, cnpj, $"{nome} LTDA.")).Success.Id;

        await client.EditarFaturamentoDaEmpresa(
            id: id,
            habilitado: false,
            metodoDePagamento: MetodoDePagamento.PrePago
        );

        // Act
        var response01 = client.EditarSaldoDaEmpresa(id, 85.49M, 0);
        var response02 = client.EditarSaldoDaEmpresa(id, 5.13M, 0);
        var response03 = client.EditarSaldoDaEmpresa(id, 2.00M, 0);

        await Task.WhenAll(response01, response02, response03);

        // Assert
        var ctx = _back.GetBackDbContext();
        var empresa = await ctx.PublicCliente.FirstAsync(x => x.ClienteId == id);

        empresa.Saldo.Should().Be(0);
        empresa.BalanceInBrl.Should().Be(92.62M);
        empresa.BalanceType.Should().Be(BalanceType.Reais.ToShort());
        empresa.FaturamentoTipoId.Should().Be(MetodoDePagamento.PrePago.ToShort());
    }
}
