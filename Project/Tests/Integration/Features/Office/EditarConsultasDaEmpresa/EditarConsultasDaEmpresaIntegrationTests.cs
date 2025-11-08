using Microsoft.EntityFrameworkCore;

namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_editar_as_configuracoes_de_consultas_da_empresa()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cnpj = DataGen.Cnpj;
        var nome = DataGen.OrgName;
        var id = (await client.CriarEmpresa(nome, cnpj, $"{nome} LTDA.")).Success.Id;

        // Act
        var response = await client.EditarConsultasDaEmpresa(
            id: id,
            highPerformance: true,
            blockSensitiveDataInQueryString: false,
            dataAccessLevel: DataAccessLevel.DadosMascarados,
            transLimitPerWeek: 125,
            gerarPdfConsultas: false,
            habilitarConsultasPorEmail: false,
            receitaCpfUseSerproAsMainSource: false,
            receitaCpfShouldReturnMinor18AgeData: true
        );

        // Assert
        response.IsSuccess.Should().BeTrue();

        var ctx = _back.GetBackDbContext();
        var empresa = await ctx.PublicCliente.FirstAsync(x => x.ClienteId == id);

        empresa.HighPerformance.Should().BeTrue();
        empresa.BlockSensitiveDataInQueryString.Should().BeFalse();
        empresa.DataAccessLevel.Should().Be(DataAccessLevel.DadosMascarados.ToShort());
        empresa.TransLimitPerWeek.Should().Be(125);

        empresa.GerarPdfConsultas.Should().BeFalse();
        empresa.HabilitarConsultasPorEmail.Should().BeFalse();

        empresa.ReceitaCpfUseSerproAsMainSource.Should().BeFalse();
        empresa.ReceitaCpfShouldReturnMinor18AgeData.Should().BeTrue();
    }

    [Test]
    public async Task Nao_deve_editar_consultas_com_limite_semanal_invalido()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cnpj = DataGen.Cnpj;
        var nome = DataGen.OrgName;
        var id = (await client.CriarEmpresa(nome, cnpj, $"{nome} LTDA.")).Success.Id;

        // Act
        var response = await client.EditarConsultasDaEmpresa(
            id: id,
            highPerformance: true,
            blockSensitiveDataInQueryString: true,
            dataAccessLevel: DataAccessLevel.DadosDeCadastroCompleto,
            transLimitPerWeek: -1,
            gerarPdfConsultas: true,
            habilitarConsultasPorEmail: true,
            receitaCpfUseSerproAsMainSource: true,
            receitaCpfShouldReturnMinor18AgeData: false
        );

        // Assert
        response.ShouldBeError(LimiteDeConsultasSemanalInvalido.I);
    }

    [Test]
    public async Task Nao_deve_editar_consultas_com_nivel_de_acesso_invalido()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cnpj = DataGen.Cnpj;
        var nome = DataGen.OrgName;
        var id = (await client.CriarEmpresa(nome, cnpj, $"{nome} LTDA.")).Success.Id;

        // Act
        var response = await client.EditarConsultasDaEmpresa(
            id: id,
            highPerformance: true,
            blockSensitiveDataInQueryString: true,
            dataAccessLevel: (DataAccessLevel)69,
            transLimitPerWeek: 10,
            gerarPdfConsultas: true,
            habilitarConsultasPorEmail: true,
            receitaCpfUseSerproAsMainSource: true,
            receitaCpfShouldReturnMinor18AgeData: false
        );

        // Assert
        response.ShouldBeError(NivelDeAcessoADadosInvalido.I);
    }
}
