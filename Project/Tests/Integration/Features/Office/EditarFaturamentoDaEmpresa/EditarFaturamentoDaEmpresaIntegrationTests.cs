using Microsoft.EntityFrameworkCore;

namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_habilitar_o_faturamento_da_empresa()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cnpj = DataGen.Cnpj;
        var nome = DataGen.OrgName;
        var empresaOut = (await client.CriarEmpresa(nome, cnpj, $"{nome} LTDA.")).Success;
        var id = empresaOut.Id;

        await client.CreateCompany(empresaOut.ExternalId);

        // Act
        var response = await client.EditarFaturamentoDaEmpresa(
            id: id,
            habilitado: true,
            metodoDePagamento: MetodoDePagamento.PosPago
        );

        // Assert
        var ctx = _back.GetBackDbContext();
        var empresa = await ctx.PublicCliente.FirstAsync(x => x.ClienteId == id);

        empresa.IsBillingCustomer.Should().BeTrue();
        empresa.FaturamentoTipoId.Should().Be(MetodoDePagamento.PosPago.ToShort());
        empresa.BalanceType.Should().Be(BalanceType.Creditos.ToShort());

        var config = await ctx.FaturamentoClienteConfig.FirstAsync(x => x.ClienteId == id);
        config.FranquiaMinima.Should().Be(495);
        config.FaturamentoPorFaixa.Should().BeFalse();
        config.PlanosDoccheckId.Should().Be(1);
        config.FaturamentoPorRateio.Should().BeFalse();
        config.DetalharRelatorios.Should().BeFalse();
        config.ExibirNaoConsumidores.Should().BeFalse();

        var webCtx = _back.GetWebDbContext();
        var company = await webCtx.Companies.FirstAsync(x => x.ExternalId == empresa.ExternalId);
        company.PaymentMode.Should().Be(CompanyPaymentMode.PosPago.ToShort());
    }

    [Test]
    [TestCase(MetodoDePagamento.PrePago, BalanceType.Reais)]
    [TestCase(MetodoDePagamento.PosPago, BalanceType.Creditos)]
    public async Task Deve_desabilitar_o_faturamento_da_empresa(MetodoDePagamento metodoDePagamento, BalanceType balanceType)
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cnpj = DataGen.Cnpj;
        var nome = DataGen.OrgName;
        var empresaOut = (await client.CriarEmpresa(nome, cnpj, $"{nome} LTDA.")).Success;
        var id = empresaOut.Id;

        await client.CreateCompany(empresaOut.ExternalId);

        // Act
        var response = await client.EditarFaturamentoDaEmpresa(
            id: id,
            habilitado: false,
            metodoDePagamento: metodoDePagamento
        );

        // Assert
        var ctx = _back.GetBackDbContext();
        var empresa = await ctx.PublicCliente.FirstAsync(x => x.ClienteId == id);

        empresa.IsBillingCustomer.Should().BeFalse();
        empresa.FaturamentoTipoId.Should().Be(metodoDePagamento.ToShort());
        empresa.BalanceType.Should().Be(balanceType.ToShort());

        var webCtx = _back.GetWebDbContext();
        var company = await webCtx.Companies.FirstAsync(x => x.ExternalId == empresa.ExternalId);
        company.PaymentMode.Should().Be(empresa.FaturamentoTipoId.Value - 1);
    }

    [Test]
    public async Task Nao_deve_editar_os_dados_de_faturamento_da_empresa_com_metodo_de_pagamento_invalido()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cnpj = DataGen.Cnpj;
        var nome = DataGen.OrgName;
        var id = (await client.CriarEmpresa(nome, cnpj, $"{nome} LTDA.")).Success.Id;

        // Act
        var response = await client.EditarFaturamentoDaEmpresa(
            id: id,
            habilitado: true,
            metodoDePagamento: (MetodoDePagamento)69
        );

        // Assert
        response.ShouldBeError(MetodoDePagamentoInvalido.I);
    }

    [Test]
    public async Task Nao_deve_editar_os_dados_de_faturamento_da_empresa_quando_ela_nao_existe()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        // Act
        var response = await client.EditarFaturamentoDaEmpresa(
            id: 123456789,
            habilitado: true,
            metodoDePagamento: MetodoDePagamento.PrePago
        );

        // Assert
        response.ShouldBeError(EmpresaNaoEncontrada.I);
    }

    [Test]
    public async Task Nao_deve_habilitar_o_faturamento_de_uma_filial()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();
        var cnpj = DataGen.Cnpj;
        var name = $"Dunder Mifflin";
        var razaoSocial = $"{name} LTDA";

        var matriz = await client.CriarEmpresa(name, cnpj, razaoSocial);
        var filial = await client.CriarEmpresa(name, cnpj, razaoSocial);
        await client.EditarCadastroDaEmpresa(filial.Success.Id, true, name, cnpj, razaoSocial, matriz.Success.Id, null, null, null);

        // Act
        var response = await client.EditarFaturamentoDaEmpresa(
            id: filial.Success.Id,
            habilitado: true,
            metodoDePagamento: MetodoDePagamento.PosPago
        );

        // Assert
        response.ShouldBeError(ApenasMatrizesPodemSerFaturadas.I);
    }
}
