namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_retornar_o_token_de_reset_de_senha()
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.SendResetPasswordToken(user.Email);

        // Act
        var token = await _factory.GetResetPasswordToken(user.Email);

        // Assert
        token!.Length.Should().Be(36);
    }

    [Test]
    public async Task Nao_deve_retornar_o_token_de_reset_de_senha_quando_o_usuario_nao_existir()
    {
        // Arrange / Act
        var token = await _factory.GetResetPasswordToken(TestData.Email);

        // Assert
        token.Should().BeNull();
    }

    [Test]
    public async Task Nao_deve_resetar_a_senha_quando_o_token_estiver_errado()
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.SendResetPasswordToken(user.Email);

        client.RemoveAuthToken();

        // Act
        var response = await client.ResetPassword(Guid.NewGuid().ToString(), "My@new@strong@P4ssword");

        // Assert
        await response.AssertBadRequest(Throw.DE019);
    }

    [Test]
    public async Task Deve_resetar_a_senha_do_usuario()
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.SendResetPasswordToken(user.Email);

        client.RemoveAuthToken();
        var token = await _factory.GetResetPasswordToken(user.Email);

        // Act
        var response = await client.ResetPassword(token!, "My@new@strong@P4ssword");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Test]
    public async Task Deve_fazer_login_com_a_nova_senha_apos_resetar_a_senha()
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.SendResetPasswordToken(user.Email);

        client.RemoveAuthToken();
        var token = await _factory.GetResetPasswordToken(user.Email);
        var password = "My@new@strong@P4ssword";
        await client.ResetPassword(token!, password);

        // Act
        var response = await client.Login(user.Email, password);

        // Assert
        response.AccessToken.Should().StartWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.");
    }

    [Test]
    public async Task Nao_deve_fazer_login_com_a_senha_antiga_apos_resetar_a_senha()
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.SendResetPasswordToken(user.Email);

        client.RemoveAuthToken();
        var token = await _factory.GetResetPasswordToken(user.Email);
        var password = "My@new@strong@P4ssword";
        await client.ResetPassword(token!, password);

        // Act
        var response = await client.Login(user.Email, user.Password);

        // Assert
        response.WrongEmailOrPassword.Should().BeTrue();
    }

    [Test]
    public async Task Nao_deve_resetar_a_senha_do_usuario_pela_segunda_vez_usando_o_mesmo_token()
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.SendResetPasswordToken(user.Email);

        client.RemoveAuthToken();
        var token = await _factory.GetResetPasswordToken(user.Email);
        var password = "My@new@strong@P4ssword";
        await client.ResetPassword(token!, password);

        // Act
        var response = await client.ResetPassword(token!, password);

        // Assert
        await response.AssertBadRequest(Throw.DE020);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidPasswords))]
    public async Task Nao_deve_resetar_a_senha_do_usuario_quando_a_senha_for_fraca(string password)
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.SendResetPasswordToken(user.Email);

        client.RemoveAuthToken();
        var token = await _factory.GetResetPasswordToken(user.Email);

        // Act
        var response = await client.ResetPassword(token!, password);

        // Assert
        await response.AssertBadRequest(Throw.DE015);
    }
}
