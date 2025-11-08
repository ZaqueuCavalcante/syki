using Exato.Web.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_editar_as_claims_do_usuario()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var cpf = DataGen.Cpf;
        var email = DataGen.Email;
        var nome = DataGen.UserName;
        var claims = new List<ExatoWebClaims>
        {
            ExatoWebClaims.Consultas,
            ExatoWebClaims.DocCheckApenasLeitura,
        };

        var user = await client.CriarUsuario(nome, email, cpf, claims);

        var newClaims = new List<ExatoWebClaims>
        {
            ExatoWebClaims.DocCheck,
            ExatoWebClaims.BradescoRH,
            ExatoWebClaims.ReaproveitamentoDeConsultas,
        };

        // Act
        var response = await client.EditarClaimsDoUsuario(user.Success.Id, newClaims);

        // Assert
        response.IsSuccess.Should().BeTrue();

        await using var webCtx = _back.GetWebDbContext();
        var webUser = await webCtx.Users.AsNoTracking().FirstAsync(x => x.EmailMain == email);
        webUser.ExtraClaims.Should().BeEquivalentTo(newClaims.ToPermissions(), options => options.WithStrictOrdering());
    }
}
