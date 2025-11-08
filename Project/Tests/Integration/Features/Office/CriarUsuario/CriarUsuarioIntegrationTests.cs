using Exato.Web.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_usuario_no_intelligence_e_no_exato_web()
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

        // Act
        var response = await client.CriarUsuario(nome, email, cpf, claims);

        // Assert
        response.ShouldBeSuccess();
        var userId = response!.Success.Id;

        await using var ctx = _back.GetBackDbContext();
        var user = await ctx.PublicUsers.AsNoTracking().FirstAsync(x => x.Id == userId);
        var cliente = await ctx.PublicCliente.AsNoTracking().FirstAsync(x => x.ClienteId == user.ClienteId);
        var organizationUser = await ctx.PublicOrganizationUser.AsNoTracking().FirstAsync(x => x.UserId == user.Id);
        var token = await ctx.PublicTokenAcesso.AsNoTracking().FirstAsync(x => x.UsuarioId == user.Id);

        // Cliente
        cliente.Nome.Should().Be(nome);
        cliente.Ativo.Should().BeTrue();
        cliente.PessoaFisica.Should().BeTrue();
        cliente.ExternalDisplayName.Should().Be(nome);
        cliente.CpfCnpj.Should().Be(long.Parse(cpf.OnlyNumbers()));
        cliente.GerarPdfConsultas.Should().BeTrue();
        cliente.ArmazenarPdfConsultas.Should().BeTrue();
        cliente.HabilitarConsultasPorEmail.Should().BeTrue();
        cliente.DataAccessLevel.Should().Be(DataAccessLevel.DadosMascarados.ToShort());
        cliente.Saldo.Should().Be(0);
        cliente.BalanceInBrl.Should().Be(0.00M);
        cliente.IsBillingCustomer.Should().BeFalse();
        cliente.BalanceType.Should().Be(BalanceType.Reais.ToShort());
        cliente.FaturamentoTipoId.Should().Be(MetodoDePagamento.PrePago.ToShort());
        cliente.Interno.Should().BeFalse();
        cliente.ClienteEmTeste.Should().BeFalse();
        cliente.RealmId.Should().Be(1);
        cliente.Origin.Should().Be("Novo Admin");
        cliente.Origem.Should().Be("Novo Admin");
        cliente.IncluidoPor.Should().Be(7);
        cliente.IncluidoEm.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));
        cliente.UnauthorizedDatasources.Should().NotBeNull();
        cliente.UnauthorizedDatasources.Should().ContainSingle().Which.Should().Be(13100);
        cliente.TransLimitPerDay.Should().Be(100);
        cliente.ExternalId.Should().NotBeEmpty();

        // User
        user.ClienteId.Should().Be(cliente.ClienteId);
        user.FullName.Should().Be(nome);
        user.Email.Should().Be(email);
        user.Cpf.Should().Be(long.Parse(cpf.OnlyNumbers()));
        user.Active.Should().BeTrue();
        user.Internal.Should().BeFalse();
        user.Visible.Should().BeTrue();
        user.CreatedBy.Should().Be(1);
        user.RealmId.Should().Be(1);
        user.ExternalId.Should().NotBeNull();
        user.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));
        user.UpdatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));

        // OrganizationUser
        organizationUser.ClienteId.Should().Be(cliente.ClienteId);
        organizationUser.UserId.Should().Be(user.Id);
        organizationUser.JoinedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));
        organizationUser.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));
        organizationUser.CreatedBy.Should().Be(7);
        organizationUser.ItsHisOwn.Should().BeTrue();

        // Token de acesso
        token.ClienteId.Should().Be(cliente.ClienteId);
        token.UsuarioId.Should().Be(user.Id);
        token.AcessoTotal.Should().BeTrue();
        token.Token.Should().HaveLength(32);
        token.KeyType.Should().Be(TokenAcessoKeyType.Type3.ToShort());
        token.IncluidoPor.Should().Be(7);
        token.IncluidoEm.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));

        await using var webCtx = _back.GetWebDbContext();
        var webUser = await webCtx.Users.AsNoTracking().FirstAsync(x => x.EmailMain == email);
        var webUserEmail = await webCtx.UserEmails.AsNoTracking().FirstAsync(x => x.UserId == user.Id);
        var webUserCompany = await webCtx.WebUserCompanies.AsNoTracking().FirstAsync(x => x.UserId == user.Id);

        // WebUser
        webUser.Active.Should().BeFalse();
        webUser.EmailMain.Should().Be(email);
        webUser.Cpf.Should().Be(cpf.OnlyNumbers());
        webUser.UserUid.Should().NotBeNullOrEmpty();
        webUser.UserUid.Should().HaveLength(36);
        webUser.PaymentMode.Should().Be(0);
        webUser.WrongPasswordAttempts.Should().Be(0);
        webUser.CreationDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));
        webUser.SoftDelete.Should().BeFalse();
        webUser.RetryAttemptsQuiz.Should().Be(0);
        webUser.Password.Should().Be("$2a$12$G63gTJRJvRKTeT9pZAxIduRWn1T2MVwtRrW2wM/PWtwq9LGiTcvhO");
        webUser.PasswordSeed.Should().Be("$2a$12$G63gTJRJvRKTeT9pZAxIdu");
        webUser.PasswordAlgorithm.Should().BeNull();
        webUser.ExtraClaims.Should().BeEquivalentTo(claims.ToPermissions(), options => options.WithStrictOrdering());

        // UserEmail
        webUserEmail.UserId.Should().Be(user.Id);
        webUserEmail.Email.Should().Be(email);
        webUserEmail.Main.Should().BeTrue();
        webUserEmail.Verified.Should().BeTrue();

        // WebUserCompany
        webUserCompany.UserId.Should().Be(user.Id);
        webUserCompany.UserExternalId.Should().Be(user.ExternalId!.Value.ToString());
        webUserCompany.OrganizationExternalId.Should().Be(cliente.ExternalId.ToString());
        webUserCompany.Token.Should().Be(token.Token);
        webUserCompany.UserRole.Should().Be(1);
        webUserCompany.IndicationCode.Should().NotBeNullOrEmpty();
        webUserCompany.IndicationCode.Should().HaveLength(10);
    }

    [Test]
    [TestCase("")]
    [TestCase(TestStrings.S151)]
    public async Task Nao_deve_criar_um_usuario_com_nome_invalido(string nome)
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        // Act
        var response = await client.CriarUsuario(nome, DataGen.Email, null, []);

        // Assert
        response.ShouldBeError(NomeDeUsuarioInvalido.I);
    }

    [Test]
    [TestCase(" ")]
    [TestCase("65464")]
    [TestCase("11924528445")]
    public async Task Nao_deve_criar_um_usuario_com_cpf_invalido(string cpf)
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        // Act
        var response = await client.CriarUsuario(DataGen.UserName, DataGen.Email, cpf, []);

        // Assert
        response.ShouldBeError(InvalidCpf.I);
    }
}
