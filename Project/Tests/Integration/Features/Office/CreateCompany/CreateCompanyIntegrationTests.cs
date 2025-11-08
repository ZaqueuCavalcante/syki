using Microsoft.EntityFrameworkCore;

namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_company()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cnpj = DataGen.Cnpj;
        var name = DataGen.OrgName;
        var razaoSocial = $"{name} LTDA";
        var cliente = (await client.CriarEmpresa(name, cnpj, razaoSocial)).Success;

        // Act
        var response = await client.CreateCompany(cliente.ExternalId);

        // Assert
        response.ShouldBeSuccess();
        var companyId = response.Success.Id;
        var ctx = _back.GetWebDbContext();
        var company = await ctx.Companies.FirstAsync(x => x.Id == companyId);

        company.Name.Should().Be(name);
        company.Active.Should().BeTrue();
        company.Cnpj.Should().Be(cnpj.OnlyNumbers());
        company.ExternalId.Should().Be(cliente.ExternalId);
        company.PaymentMode.Should().Be(CompanyPaymentMode.PosPago.ToShort());
    }

    [Test]
    public async Task Should_not_create_company_when_empresa_not_exists()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        // Act
        var response = await client.CreateCompany(Guid.NewGuid());

        // Assert
        response.ShouldBeError(EmpresaNaoEncontrada.I);
    }
}
