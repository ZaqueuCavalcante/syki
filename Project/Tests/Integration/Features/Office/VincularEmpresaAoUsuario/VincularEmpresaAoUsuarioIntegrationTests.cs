using Microsoft.EntityFrameworkCore;

namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_vincular_uma_empresa_ao_usuario()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();
        var cnpj = DataGen.Cnpj;
        var name = $"Nubank {DataGen.Numbers}";
        var razaoSocial = $"{name} LTDA";

        var cliente = (await client.CriarEmpresa(name, cnpj, razaoSocial)).Success;
        var userId = (await client.CriarUsuario(DataGen.UserName, DataGen.Email, DataGen.Cpf, [])).Success.Id;

        // Act
        var response = await client.VincularEmpresaUsuario(cliente.Id, userId);

        // Assert
        response.ShouldBeSuccess();

        var ctx = _back.GetBackDbContext();
        var token = await ctx.PublicTokenAcesso.Where(x => x.UsuarioId == userId && x.ClienteId == cliente.Id).SingleAsync();

        var orgUser = await ctx.PublicOrganizationUser.Where(x => x.UserId == userId && x.ClienteId == cliente.Id).SingleAsync();
        orgUser.ItsHisOwn.Should().BeFalse();

        var webCtx = _back.GetWebDbContext();
        var webUserCompany = await webCtx.WebUserCompanies
            .Where(x => x.UserId == userId && x.OrganizationExternalId == cliente.ExternalId.ToString()).SingleAsync();
        webUserCompany.Token.Should().Be(token.Token);
        webUserCompany.UserRole.Should().Be(1);
    }

    [Test]
    public async Task Nao_deve_vincular_uma_empresa_a_um_usuario_que_nao_existe()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cnpj = DataGen.Cnpj;
        var name = $"Nubank {DataGen.Numbers}";
        var razaoSocial = $"{name} LTDA";
        var cliente = (await client.CriarEmpresa(name, cnpj, razaoSocial)).Success;

        // Act
        var response = await client.VincularEmpresaUsuario(68456148, cliente.Id);

        // Assert
        response.ShouldBeError(UserNotFound.I);
    }

    [Test]
    public async Task Nao_deve_vincular_uma_empresa_que_nao_existe_a_um_usuario()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();
        var userId = (await client.CriarUsuario(DataGen.UserName, DataGen.Email, DataGen.Cpf, [])).Success.Id;

        // Act
        var response = await client.VincularEmpresaUsuario(3248946, userId);

        // Assert
        response.ShouldBeError(EmpresaNaoEncontrada.I);
    }

    [Test]
    public async Task Nao_deve_vincular_uma_empresa_ao_usuario_mais_de_uma_vez()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();
        var cnpj = DataGen.Cnpj;
        var name = $"Nubank {DataGen.Numbers}";
        var razaoSocial = $"{name} LTDA";

        var cliente = (await client.CriarEmpresa(name, cnpj, razaoSocial)).Success;
        var userId = (await client.CriarUsuario(DataGen.UserName, DataGen.Email, DataGen.Cpf, [])).Success.Id;

        // Act
        var response01 = await client.VincularEmpresaUsuario(cliente.Id, userId);
        var response02 = await client.VincularEmpresaUsuario(cliente.Id, userId);

        // Assert
        response01.ShouldBeSuccess();
        response02.ShouldBeError(UsuarioJaVinculadoAEmpresaNoIntelligence.I);
    }
}
