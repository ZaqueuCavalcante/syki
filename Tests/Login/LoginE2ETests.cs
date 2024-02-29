namespace Syki.Tests.Login;

public class LoginE2ETests : E2ETestBase
{
    // [Test]
    public async Task Should_login_into_app()
    {
        await Goto("/register");

        var email = await Fill(TestData.Email);
        await ClickOn(Button("Cadastrar"));
        await AssertVisibleText("Verifique seu email");

        var token = await GetRegisterToken(email);
        await Goto($"/register-setup?token={token}");

        var password = await Fill("Test@123Test@123");

        await ClickOn(Button("Salvar"));
        await ClickOn(Button("Ir pro login"));

        await Page.Locator("input[type=\"email\"]").FillAsync(email);
        await Page.Locator("input[type=\"password\"]").FillAsync(password);

        await ClickOn(Button("Login"));

        await AssertVisibleLink("Insights");
    }
}
