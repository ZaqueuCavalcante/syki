using Microsoft.EntityFrameworkCore;

namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_editar_um_token_de_acesso()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cnpj = DataGen.Cnpj;
        var nome = DataGen.OrgName;
        var razaoSocial = $"{nome} LTDA";
        var clienteId = (await client.CriarEmpresa(nome, cnpj, razaoSocial)).Success.Id;
        var tokenId = (await client.CriarTokenDeAcesso(clienteId, TokenAcessoKeyType.Type3)).Success.Id;

        var novoNome = "Token atualizado";
        var novaDescricao = "Token atualizado para testes";
        var novoValidoAte = DateTime.Today.AddDays(15);

        // Act
        var response = await client.EditarTokenDeAcesso(
            tokenId,
            name: novoNome,
            description: novaDescricao,
            validoAte: novoValidoAte,
            transLimitPerHour: 5,
            transLimitPerDay: 15,
            transLimitPerWeek: 70,
            transLimitPerMonth: 300,
            creditsLimitPerHour: 10,
            creditsLimitPerDay: 25,
            creditsLimitPerWeek: 80,
            creditsLimitPerMonth: 320
        );

        // Assert
        response.IsSuccess.Should().BeTrue();

        var ctx = _back.GetBackDbContext();
        var token = await ctx.PublicTokenAcesso.FirstAsync(x => x.TokenAcessoId == tokenId);

        token.Name.Should().Be(novoNome);
        token.Description.Should().Be(novaDescricao);
        token.ValidoAte.Should().BeCloseTo(novoValidoAte, TimeSpan.FromSeconds(5));
        token.TransLimitPerHour.Should().Be(5);
        token.TransLimitPerDay.Should().Be(15);
        token.TransLimitPerWeek.Should().Be(70);
        token.TransLimitPerMonth.Should().Be(300);
        token.CreditsLimitPerHour.Should().Be(10);
        token.CreditsLimitPerDay.Should().Be(25);
        token.CreditsLimitPerWeek.Should().Be(80);
        token.CreditsLimitPerMonth.Should().Be(320);
    }

    [Test]
    public async Task Nao_deve_editar_token_com_validade_menor_que_ontem()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cnpj = DataGen.Cnpj;
        var nome = DataGen.OrgName;
        var razaoSocial = $"{nome} LTDA";
        var clienteId = (await client.CriarEmpresa(nome, cnpj, razaoSocial)).Success.Id;
        var tokenId = (await client.CriarTokenDeAcesso(clienteId, TokenAcessoKeyType.Type3)).Success.Id;

        // Act
        var response = await client.EditarTokenDeAcesso(tokenId, validoAte: DateTime.Today.AddDays(-2));

        // Assert
        response.ShouldBeError(ValidadeDeTokenInvalida.I);
    }

    [Test]
    public async Task Token_doc_check_deve_manter_nome_e_descricao()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cnpj = DataGen.Cnpj;
        var nome = DataGen.OrgName;
        var razaoSocial = $"{nome} LTDA";
        var clienteId = (await client.CriarEmpresa(nome, cnpj, razaoSocial)).Success.Id;
        var tokenId = (await client.CriarTokenDeAcesso(
            clienteId,
            TokenAcessoKeyType.Type1,
            "Token DC",
            "Token do Doc Check")).Success.Id;

        // Act
        var response = await client.EditarTokenDeAcesso(tokenId, name: "", description: null);

        // Assert
        response.ShouldBeError(NomeDeTokenInvalido.I);
    }
}
