using Microsoft.EntityFrameworkCore;

namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_desvincular_uma_empresa_do_usuario()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();
        var cnpj = DataGen.Cnpj;
        var name = DataGen.OrgName;
        var razaoSocial = $"{name} LTDA";

        var cliente = (await client.CriarEmpresa(name, cnpj, razaoSocial)).Success;
        var userId = (await client.CriarUsuario(DataGen.UserName, DataGen.Email, DataGen.Cpf, [])).Success.Id;

        await client.VincularEmpresaUsuario(cliente.Id, userId);

        // Act
        var response = await client.DesvincularEmpresaUsuario(cliente.Id, userId);

        // Assert
        response.ShouldBeSuccess();

        var ctx = _back.GetBackDbContext();
        var token = await ctx.PublicTokenAcesso.Where(x => x.UsuarioId == userId && x.ClienteId == cliente.Id).FirstAsync();
        token.ExcluidoEm.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        token.ExcluidoPor.Should().Be(7);

        var orgUser = await ctx.PublicOrganizationUser.Where(x => x.UserId == userId && x.ClienteId == cliente.Id).FirstAsync();
        orgUser.LeavedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));

        var webCtx = _back.GetWebDbContext();
        var webUserCompany = await webCtx.WebUserCompanies
            .Where(x => x.UserId == userId && x.OrganizationExternalId == cliente.ExternalId.ToString()).FirstOrDefaultAsync();
        webUserCompany.Should().BeNull();
    }
}
