using Syki.Back.Settings;
using Syki.Back.Database;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace Syki.Tests.E2E;

public class HomePageE2ETests : PageTest
{
    [Test]
    public async Task Should_create_a_pending_register()
    {
        await Page.GotoAsync("https://localhost:6001");

        var tryNowButton = Page.GetByRole(AriaRole.Link, new() { Name = "Experimente agora" });
        await Expect(tryNowButton).ToBeVisibleAsync();
        await tryNowButton.ClickAsync();

        await Page.GetByRole(AriaRole.Textbox).FillAsync(TestData.Email);

        var registerButton = Page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" });
        await Expect(registerButton).ToBeVisibleAsync();
        await registerButton.ClickAsync();

        var message = Page.GetByText("Verifique seu email e utilize o link para definir sua senha de acesso.");
        await Expect(message).ToBeVisibleAsync();
    }

    [Test]
    public async Task Should_finish_a_pending_register_by_creating_user_password()
    {
        await Page.GotoAsync("https://localhost:6001");

        var tryNowButton = Page.GetByRole(AriaRole.Link, new() { Name = "Experimente agora" });
        await Expect(tryNowButton).ToBeVisibleAsync();
        await tryNowButton.ClickAsync();

        var email = TestData.Email;
        await Page.GetByRole(AriaRole.Textbox).FillAsync(email);

        var registerButton = Page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" });
        await Expect(registerButton).ToBeVisibleAsync();
        await registerButton.ClickAsync();

        var message = Page.GetByText("Verifique seu email e utilize o link para definir sua senha de acesso.");
        await Expect(message).ToBeVisibleAsync();

        var settings = new DatabaseSettings { ConnectionString = "UserID=postgres;Password=postgres;Host=localhost;Port=5432;Database=syki-db;Pooling=true;" };
        using var ctx = new SykiDbContext(new DbContextOptions<SykiDbContext>(), settings);
        var demo = await ctx.Demos.FirstAsync(d => d.Email == email);
        var token = demo.Id.ToString();

        await Page.GotoAsync($"https://localhost:6001/demo-setup?token={token}");

        await Page.GetByRole(AriaRole.Textbox).FillAsync("Test@123Test@123");

        var savePasswordButton = Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" });
        await Expect(savePasswordButton).ToBeVisibleAsync();
        await savePasswordButton.ClickAsync();

        var goToLoginButton = Page.GetByRole(AriaRole.Button, new() { Name = "Ir pro login" });
        await Expect(goToLoginButton).ToBeVisibleAsync();
        await goToLoginButton.ClickAsync();

        var loginButton = Page.GetByRole(AriaRole.Button, new() { Name = "Login" });
        await Expect(loginButton).ToBeVisibleAsync();
    }
}
