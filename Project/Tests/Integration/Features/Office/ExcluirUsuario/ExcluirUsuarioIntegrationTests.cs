using Microsoft.EntityFrameworkCore;

namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_excluir_um_usuario_no_intelligence_e_no_exato_web()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cpf = DataGen.Cpf;
        var email = DataGen.Email;
        var nome = DataGen.UserName;
        var claims = new List<ExatoWebClaims>
        {
            ExatoWebClaims.Consultas,
            ExatoWebClaims.GestaoDeUsuarios,
            ExatoWebClaims.DocCheckApenasLeitura,
        };

        var userId = (await client.CriarUsuario(nome, email, cpf, claims)).Success.Id;

        // Act
        var response = await client.ExcluirUsuario(userId);

        // Assert
        response.ShouldBeSuccess();

        await using var ctx = _back.GetBackDbContext();
        var user = await ctx.PublicUsers.AsNoTracking().FirstAsync(x => x.Id == userId);
        var cliente = await ctx.PublicCliente.AsNoTracking().FirstAsync(x => x.ClienteId == user.ClienteId);
        var organizationUser = await ctx.PublicOrganizationUser.AsNoTracking().FirstAsync(x => x.UserId == user.Id);
        var token = await ctx.PublicTokenAcesso.AsNoTracking().FirstAsync(x => x.UsuarioId == user.Id);

        // Cliente
        cliente.Ativo.Should().BeFalse();
        cliente.ExcluidoPor.Should().Be(7);
        cliente.ExcluidoEm.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));

        // User
        user.Active.Should().BeFalse();
        user.DeletedBy.Should().Be(7);
        user.DeletedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));

        // OrganizationUser
        organizationUser.LeavedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));

        // Token de acesso
        token.ExcluidoPor.Should().Be(7);
        token.ExcluidoEm.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));

        await using var webCtx = _back.GetWebDbContext();
        var webUser = await webCtx.Users.AsNoTracking().FirstAsync(x => x.EmailMain == email);

        // WebUser
        webUser.Active.Should().BeFalse();
        webUser.SoftDelete.Should().BeTrue();
    }

    [Test]
    public async Task Deve_excluir_um_usuario_no_intelligence_e_no_exato_web_quando_o_usuario_possui_vinculo_com_outra_empresa()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cpf = DataGen.Cpf;
        var email = DataGen.Email;
        var nome = DataGen.UserName;
        var userId = (await client.CriarUsuario(nome, email, cpf, [])).Success.Id;

        var cnpj = DataGen.Cnpj;
        var name = DataGen.OrgName;
        var razaoSocial = $"{name} LTDA";
        var empresa = (await client.CriarEmpresa(name, cnpj, razaoSocial)).Success;

        await client.VincularEmpresaUsuario(empresa.Id, userId);

        // Act
        var response = await client.ExcluirUsuario(userId);

        // Assert
        response.ShouldBeSuccess();

        await using var ctx = _back.GetBackDbContext();
        var user = await ctx.PublicUsers.AsNoTracking().FirstAsync(x => x.Id == userId);
        var cliente = await ctx.PublicCliente.AsNoTracking().FirstAsync(x => x.ClienteId == user.ClienteId);
        var orgs = await ctx.PublicOrganizationUser.Where(x => x.UserId == user.Id).AsNoTracking().ToListAsync();
        var tokens = await ctx.PublicTokenAcesso.Where(x => x.UsuarioId == user.Id).AsNoTracking().ToListAsync();
        var empresaVinculada = await ctx.PublicCliente.AsNoTracking().FirstAsync(x => x.ClienteId == empresa.Id);

        // Cliente
        cliente.Ativo.Should().BeFalse();
        cliente.ExcluidoPor.Should().Be(7);
        cliente.ExcluidoEm.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));

        // User
        user.Active.Should().BeFalse();
        user.DeletedBy.Should().Be(7);
        user.DeletedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));

        // OrganizationUser
        orgs.Should().HaveCount(2);
        orgs.Should().AllSatisfy(x => x.LeavedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1)));

        // Token de acesso
        tokens.Should().HaveCount(2);
        tokens.Should().AllSatisfy(x => x.ExcluidoPor.Should().Be(7));
        tokens.Should().AllSatisfy(x => x.ExcluidoEm.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1)));

        // Empresa vinculada
        empresaVinculada.Ativo.Should().BeTrue();
        empresaVinculada.ExcluidoPor.Should().BeNull();
        empresaVinculada.ExcluidoEm.Should().BeNull();

        await using var webCtx = _back.GetWebDbContext();
        var webUser = await webCtx.Users.AsNoTracking().FirstAsync(x => x.EmailMain == email);

        // WebUser
        webUser.Active.Should().BeFalse();
        webUser.SoftDelete.Should().BeTrue();
    }

    [Test]
    public async Task Nao_deve_excluir_um_usuario_quando_ele_nao_existir()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        // Act
        var response = await client.ExcluirUsuario(68486486);

        // Assert
        response.ShouldBeError(UserNotFound.I);
    }

    [Test]
    public async Task Nao_deve_excluir_um_usuario_ja_excluido_anteriormente()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cpf = DataGen.Cpf;
        var email = DataGen.Email;
        var nome = DataGen.UserName;
        var claims = new List<ExatoWebClaims>
        {
            ExatoWebClaims.Consultas,
            ExatoWebClaims.GestaoDeUsuarios,
            ExatoWebClaims.DocCheckApenasLeitura,
        };

        var userId = (await client.CriarUsuario(nome, email, cpf, claims)).Success.Id;
        await client.ExcluirUsuario(userId);

        // Act
        var response = await client.ExcluirUsuario(userId);

        // Assert
        response.ShouldBeError(UserAlreadyDeleted.I);
    }
}
