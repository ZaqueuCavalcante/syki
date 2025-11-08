using Microsoft.EntityFrameworkCore;
using Exato.Back.Features.Office.CriarEmpresa;
using Exato.Back.Features.Office.EditarCadastroDaEmpresa;

namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_editar_os_dados_de_cadastro_da_empresa()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var postResponse = await client.CriarEmpresa(
            "Deloitte",
            "51.311.998/0001-86",
            "DELOITTE DO BRASIL LTDA."
        );
        var id = postResponse.Success.Id;

        await client.CreateCompany(postResponse.Success.ExternalId);

        // Act
        var response = await client.EditarCadastroDaEmpresa(
            id: id,
            ativa: false,
            nome: "Deloitte 2",
            cnpj: "51.311.998/0001-86",
            razaoSocial: "DELOITTE 2 DO BRASIL LTDA.",
            matrizId: null,
            nomeFantasia: "DELOITTE NOME FANTASIA",
            slug: "deloitte-2",
            salesContact: "Edmilson Jaguara"
        );

        // Assert
        var ctx = _back.GetBackDbContext();
        var empresa = await ctx.PublicCliente.FirstAsync(x => x.ClienteId == id);

        empresa.Nome.Should().Be("Deloitte 2");
        empresa.Ativo.Should().BeFalse();
        empresa.ExternalDisplayName.Should().Be("Deloitte 2");
        empresa.RazaoSocialRf.Should().Be("DELOITTE 2 DO BRASIL LTDA.");
        empresa.CpfCnpj.Should().Be(51311998000186);
        empresa.QuodSegmentId.Should().BeNull();
        empresa.NomeFantasiaRf.Should().Be("DELOITTE NOME FANTASIA");
        empresa.Slug.Should().Be("deloitte-2");
        empresa.ExatoSalesContact.Should().Be("Edmilson Jaguara");
        empresa.ParentOrganizationId.Should().BeNull();
        empresa.OrganizationSegmentId.Should().BeNull();

        var webCtx = _back.GetWebDbContext();
        var company = await webCtx.Companies.FirstAsync(x => x.ExternalId == empresa.ExternalId);
        company.Active.Should().BeFalse();
        company.Name.Should().Be("Deloitte 2");
        company.Cnpj.Should().Be("51311998000186");

        await AssertSingleDomainEvent<EmpresaCriadaDomainEvent>($"{empresa.ExternalId}");
    }

    [Test]
    [TestCase("")]
    [TestCase(TestStrings.S151)]
    public async Task Deve_editar_os_dados_de_cadastro_da_empresa_com_nome_invalido(string name)
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();
        var id = (await client.CriarEmpresa(DataGen.OrgName)).Success.Id;

        // Act
        var response = await client.EditarCadastroDaEmpresa(
            id: id,
            ativa: false,
            nome: name,
            cnpj: "51.311.998/0001-86",
            razaoSocial: "DELOITTE 2 DO BRASIL LTDA.",
            matrizId: null,
            nomeFantasia: "DELOITTE NOME FANTASIA",
            slug: "deloitte-2",
            salesContact: null
        );

        // Assert
        response.ShouldBeError(NomeDeEmpresaInvalido.I);
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("65464")]
    [TestCase("26873783056")]
    [TestCase("70790734000106")]
    public async Task Deve_editar_os_dados_de_cadastro_da_empresa_com_cnpj_invalido(string cnpj)
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();
        var name = DataGen.OrgName;
        var id = (await client.CriarEmpresa(name)).Success.Id;

        // Act
        var response = await client.EditarCadastroDaEmpresa(
            id: id,
            ativa: false,
            nome: name,
            cnpj: cnpj,
            razaoSocial: "DELOITTE 2 DO BRASIL LTDA.",
            matrizId: null,
            nomeFantasia: "DELOITTE NOME FANTASIA",
            slug: "deloitte-2",
            salesContact: null
        );

        // Assert
        response.ShouldBeError(InvalidCnpj.I);
    }

    [Test]
    public async Task Deve_buscar_os_dados_da_empresa_editada_na_receita_federal()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();
        var name = DataGen.OrgName;
        var id = (await client.CriarEmpresa(name)).Success.Id;

        var response = await client.EditarCadastroDaEmpresa(
            id: id,
            ativa: false,
            nome: name,
            cnpj: "55.330.739/0001-53",
            razaoSocial: "DELOITTE EDITADA DO BRASIL LTDA.",
            matrizId: null,
            nomeFantasia: "DELOITTE EDITADA NOME FANTASIA",
            slug: "deloitte-editada",
            salesContact: null
        );

        // Act
        await _workers.AwaitEventsProcessing();
        await _workers.AwaitCommandsProcessing();

        // Assert
        var ctx = _back.GetBackDbContext();
        var empresa = await ctx.PublicCliente.FirstAsync(x => x.ClienteId == id);
        await ctx.Entry(empresa).ReloadAsync();
        empresa.QuodSegmentId.Should().Be(3);
        empresa.NomeFantasiaRf.Should().Be("DELOITTE RECEITA FEDERAL");
        empresa.RazaoSocialRf.Should().Be("DELOITTE RECEITA FEDERAL S.A.");

        await AssertSingleDomainEvent<CadastroDaEmpresaEditadoDomainEvent>($"{empresa.ExternalId}");
    }
}
