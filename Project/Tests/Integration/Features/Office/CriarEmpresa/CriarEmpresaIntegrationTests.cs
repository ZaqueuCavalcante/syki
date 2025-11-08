using Microsoft.EntityFrameworkCore;
using Exato.Back.Features.Office.CriarEmpresa;

namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_uma_empresa()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        // Act
        var response = await client.CriarEmpresa(
            "Deloitte",
            "51.311.998/0001-86",
            "DELOITTE DO BRASIL LTDA."
        );

        // Assert
        var id = response.Success.Id;
        var ctx = _back.GetBackDbContext();
        var empresa = await ctx.PublicCliente.FirstAsync(x => x.ClienteId == id);

        // Cadastro
        empresa.Nome.Should().Be("Deloitte");
        empresa.Ativo.Should().BeTrue();
        empresa.ExternalDisplayName.Should().Be("Deloitte");
        empresa.RazaoSocialRf.Should().Be("DELOITTE DO BRASIL LTDA.");
        empresa.CpfCnpj.Should().Be(51311998000186);
        empresa.QuodSegmentId.Should().BeNull();
        empresa.NomeFantasiaRf.Should().BeNull();
        empresa.Slug.Should().BeNull();
        empresa.ParentOrganizationId.Should().BeNull();
        empresa.OrganizationSegmentId.Should().BeNull();

        // Consultas
        empresa.HighPerformance.Should().BeFalse();
        empresa.BlockSensitiveDataInQueryString.Should().BeTrue();
        empresa.DataAccessLevel.Should().Be(DataAccessLevel.DadosDeCadastroCompleto.ToShort());
        empresa.TransLimitPerWeek.Should().BeNull();

        empresa.GerarPdfConsultas.Should().BeTrue();
        empresa.ArmazenarPdfConsultas.Should().BeTrue();
        empresa.HabilitarConsultasPorEmail.Should().BeTrue();
        empresa.PdfPassword.Should().Be("513119");

        empresa.ReceitaCpfNeedPdfProof.Should().BeTrue();
        empresa.ReceitaCpfUseSerproAsMainSource.Should().BeTrue();
        empresa.ReceitaCpfShouldReturnMinor18AgeData.Should().BeFalse();

        // Relatórios
        empresa.DossierIdToExecutePf.Should().Be(5040);
        empresa.DossierIdToExecutePfCreditAnalysis.Should().Be(5041);
        empresa.DossierIdToExecutePj.Should().Be(5045);
        empresa.DossierIdToExecutePjCreditAnalysis.Should().Be(5046);

        // Faturamento
        empresa.Saldo.Should().Be(0);
        empresa.BalanceInBrl.Should().Be(0.00M);
        empresa.IsBillingCustomer.Should().BeTrue();
        empresa.BalanceType.Should().Be(BalanceType.Creditos.ToShort());
        empresa.FaturamentoTipoId.Should().Be(MetodoDePagamento.PosPago.ToShort());

        empresa.Interno.Should().BeFalse();
        empresa.PessoaFisica.Should().BeFalse();
        empresa.ClienteEmTeste.Should().BeFalse();
        empresa.StoreTransactionInput.Should().BeTrue();
        empresa.StoreTransactionReturn.Should().BeTrue();

        empresa.QuodLastEnrollment.Should().BeNull();
        empresa.QuodSuccessfullyEnrollmentAt.Should().BeNull();
        empresa.ExternalId.Should().NotBeEmpty();
        empresa.QuodCustomerExternalId.Should().Be(empresa.ExternalId);

        empresa.RealmId.Should().Be(1);
        empresa.Origin.Should().Be("Novo Admin");
        empresa.Origem.Should().Be("Novo Admin");
        empresa.IncluidoPor.Should().Be(7);
        empresa.IncluidoEm.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));

        empresa.PartnerId.Should().BeNull();
        empresa.CrmClientId.Should().BeNull();

        empresa.UseOcrExato.Should().BeFalse();
        empresa.UseSerproDataValidFacial.Should().BeFalse();

        empresa.ExatoSalesContact.Should().BeNull();

        var webCtx = _back.GetWebDbContext();
        var company = await webCtx.Companies.FirstAsync(x => x.ExternalId == empresa.ExternalId);
        company.Active.Should().BeTrue();
        company.Cnpj.Should().Be("51311998000186");
        company.Name.Should().Be(empresa.Nome);
        company.OnboardStatus.Should().Be(ExatoWebOnboardStatus.Completed.ToInt());
        company.OnboardDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));
        company.CompanyUid.Should().Be(empresa.ExternalId.ToString());
        company.PaymentMode.Should().Be(CompanyPaymentMode.PosPago.ToShort());

        await AssertSingleDomainEvent<EmpresaCriadaDomainEvent>($"{empresa.ExternalId}");
    }

    [Test]
    [TestCase("")]
    [TestCase(TestStrings.S151)]
    public async Task Nao_deve_criar_uma_empresa_com_nome_invalido(string name)
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        // Act
        var response = await client.CriarEmpresa(name);

        // Assert
        response.ShouldBeError(NomeDeEmpresaInvalido.I);
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("65464")]
    [TestCase("26873783056")]
    [TestCase("70790734000106")]
    public async Task Nao_deve_criar_uma_empresa_com_cnpj_invalido(string cnpj)
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();
        var name = DataGen.OrgName;

        // Act
        var response = await client.CriarEmpresa(name, cnpj, name);

        // Assert
        response.ShouldBeError(InvalidCnpj.I);
    }

    [Test]
    public async Task Deve_buscar_os_dados_da_nova_empresa_na_receita_federal()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var response = await client.CriarEmpresa(
            "Nubank",
            "30.680.829/0001-43",
            "NUBANK LTDA."
        );

        // Act
        await _workers.AwaitEventsProcessing();
        await _workers.AwaitCommandsProcessing();

        // Assert
        var id = response.Success.Id;
        var ctx = _back.GetBackDbContext();
        var empresa = await ctx.PublicCliente.FirstAsync(x => x.ClienteId == id);
        await ctx.Entry(empresa).ReloadAsync();
        empresa.QuodSegmentId.Should().Be(3);
        empresa.NomeFantasiaRf.Should().Be("BANCO NU");
        empresa.RazaoSocialRf.Should().Be("NU FINANCEIRA S.A.");
    }
}
