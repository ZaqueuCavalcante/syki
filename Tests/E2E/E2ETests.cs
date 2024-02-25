namespace Syki.Tests.E2E;

public class E2ETests : E2ETestBase
{
    [Test]
    public async Task Should_create_a_pending_register()
    {
        await Goto("/");

        await ClickOn(Link("Experimente agora"));

        await Fill(TestData.Email);

        await ClickOn(Button("Cadastrar"));

        await AssertVisibleText("Verifique seu email e utilize o link para definir sua senha de acesso.");
    }

    [Test]
    public async Task Should_finish_a_pending_register_by_creating_user_password()
    {
        await Goto("/");

        await ClickOn(Link("Experimente agora"));

        var email = await Fill(TestData.Email);

        await ClickOn(Button("Cadastrar"));
        await AssertVisibleText("Verifique seu email e utilize o link para definir sua senha de acesso.");

        var token = await GetDemoToken(email);
        await Goto($"/demo-setup?token={token}");

        await Fill("Test@123Test@123");

        await ClickOn(Button("Salvar"));
        await ClickOn(Button("Ir pro login"));

        await AssertVisibleButton("Login");
    }

    [Test]
    public async Task Should_login_into_app()
    {
        await Goto("/");

        await ClickOn(Link("Experimente agora"));

        var email = await Fill(TestData.Email);

        await ClickOn(Button("Cadastrar"));
        await AssertVisibleText("Verifique seu email e utilize o link para definir sua senha de acesso.");

        var token = await GetDemoToken(email);
        await Goto($"/demo-setup?token={token}");

        var password = await Fill("Test@123Test@123");

        await ClickOn(Button("Salvar"));
        await ClickOn(Button("Ir pro login"));

        await Page.Locator("input[type=\"text\"]").FillAsync(email);
        await Page.Locator("input[type=\"password\"]").FillAsync(password);

        await ClickOn(Button("Login"));

        await AssertVisibleLink("Insights");
    }
}
