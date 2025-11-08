namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_buscar_empresa_por_id()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cnpj = "00367599000104";
        var nome = DataGen.OrgName;
        var razaoSocial = $"{nome} LTDA";
        var id = (await client.CriarEmpresa(nome, cnpj, razaoSocial)).Success.Id;

        // Act
        var response = await client.BuscarEmpresa(id);

        // Assert
        response.ShouldBeSuccess();
        var empresa = response.Success;

        empresa.Id.Should().Be(id);
        empresa.Ativa.Should().BeTrue();
        empresa.CNPJ.Should().Be(cnpj);
        empresa.Nome.Should().Be(nome);
        empresa.RazaoSocial.Should().Be(razaoSocial);
        empresa.MatrizId.Should().BeNull();
        empresa.Matriz.Should().Be("-");
        empresa.NomeFantasia.Should().BeNull();
        empresa.Slug.Should().BeNull();

        empresa.HighPerformance.Should().BeFalse();
        empresa.BlockSensitiveDataInQueryString.Should().BeTrue();
        empresa.DataAccessLevel.Should().Be(DataAccessLevel.DadosDeCadastroCompleto);
        empresa.TransLimitPerWeek.Should().BeNull();

        empresa.GerarPdfConsultas.Should().BeTrue();
        empresa.HabilitarConsultasPorEmail.Should().BeTrue();

        empresa.ReceitaCpfUseSerproAsMainSource.Should().BeTrue();
        empresa.ReceitaCpfShouldReturnMinor18AgeData.Should().BeFalse();

        empresa.RelatorioPF.Id.Should().Be(5040);
        empresa.RelatorioPFQuod.Id.Should().Be(5041);
        empresa.RelatorioPJ.Id.Should().Be(5045);
        empresa.RelatorioPJQuod.Id.Should().Be(5046);

        empresa.RelatorioPF.Id.Should().Be(5040);
        empresa.RelatorioPF.Nome.Should().Be("Exato - Dossiê Pessoa Física - Full - V2");
        empresa.RelatorioPFQuod.Id.Should().Be(5041);
        empresa.RelatorioPFQuod.Nome.Should().Be("Exato - Dossiê Pessoa Física com Análise de Crédito - V2");
        empresa.RelatorioPJ.Id.Should().Be(5045);
        empresa.RelatorioPJ.Nome.Should().Be("Exato - Dossiê Empresa - Full - V2");
        empresa.RelatorioPJQuod.Id.Should().Be(5046);
        empresa.RelatorioPJQuod.Nome.Should().Be("Exato - Relatório de Pessoa Jurídica com Análise de Crédito - V2");

        empresa.IsBillingCustomer.Should().BeTrue();
        empresa.Creditos.Should().Be(0);
        empresa.BalanceInBrl.Should().Be(0);
        empresa.MetodoDePagamento.Should().Be(MetodoDePagamento.PosPago);
        empresa.BalanceType.Should().Be(BalanceType.Creditos);

        empresa.CriadaEm.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
    }

    [Test]
    public async Task Nao_deve_buscar_empresa_inexistente()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        // Act
        var response = await client.BuscarEmpresa(123456789);

        // Assert
        response.ShouldBeError(EmpresaNaoEncontrada.I);
    }
}
