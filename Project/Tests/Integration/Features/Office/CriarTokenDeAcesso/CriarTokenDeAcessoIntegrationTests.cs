using Microsoft.EntityFrameworkCore;

namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_token_de_acesso_do_tipo_api()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cnpj = "00367599000104";
        var nome = DataGen.OrgName;
        var razaoSocial = $"{nome} LTDA";
        var clienteId = (await client.CriarEmpresa(nome, cnpj, razaoSocial)).Success.Id;
        var validoAte = DateTime.Now.AddDays(90);

        // Act
        var response = await client.CriarTokenDeAcesso(clienteId, TokenAcessoKeyType.Type3, validoAte: validoAte);

        // Assert
        var id = response.Success.Id;
        var ctx = _back.GetBackDbContext();
        var token = await ctx.PublicTokenAcesso.FirstAsync(x => x.TokenAcessoId == id);

        token.ClienteId.Should().Be(clienteId);
        token.AcessoTotal.Should().BeTrue();
        token.KeyType.Should().Be(TokenAcessoKeyType.Type3.ToShort());

        token.Token.Should().HaveLength(32);
        token.KeyId.Should().BeNull();
        token.SecretHash.Should().BeNull();

        token.Name.Should().BeNull();
        token.Description.Should().BeNull();

        token.ValidoAte.Should().BeCloseTo(validoAte, TimeSpan.FromSeconds(5));
    }

    [Test]
    public async Task Deve_criar_um_token_de_acesso_do_tipo_doc_check()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cnpj = "00367599000104";
        var nome = DataGen.OrgName;
        var razaoSocial = $"{nome} LTDA";
        var clienteId = (await client.CriarEmpresa(nome, cnpj, razaoSocial)).Success.Id;

        // Act
        var response = await client.CriarTokenDeAcesso(
            clienteId,
            TokenAcessoKeyType.Type1,
            "Token DC",
            "Token do Doc Check"
        );

        // Assert
        var id = response.Success.Id;
        var ctx = _back.GetBackDbContext();
        var token = await ctx.PublicTokenAcesso.FirstAsync(x => x.TokenAcessoId == id);

        token.ClienteId.Should().Be(clienteId);
        token.AcessoTotal.Should().BeTrue();
        token.KeyType.Should().Be(TokenAcessoKeyType.Type1.ToShort());

        token.Token.Should().HaveLength(32);
        token.KeyId.Should().NotBeNull();
        token.SecretHash.Should().HaveLength(36);

        token.Name.Should().Be("Token DC");
        token.Description.Should().Be("Token do Doc Check");

        var setting = await ctx.PublicDoccheckTokenSettings.FirstOrDefaultAsync(x => x.KeyId == token.KeyId);
        setting.Should().NotBeNull();
    }
}
