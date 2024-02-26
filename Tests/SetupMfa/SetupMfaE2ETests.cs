using Syki.Shared;
using Microsoft.Playwright;

namespace Syki.Tests.E2E;

public class SetupMfaE2ETests : E2ETestBase
{
    [Test]
    public async Task Should_setup_mfa()
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
        await Page.GetByRole(AriaRole.Banner).GetByRole(AriaRole.Link).Nth(1).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Img)).ToBeVisibleAsync();

        var code = await GetMfaCode(email);
        await Page.Locator("input[type=\"text\"]").PressSequentiallyAsync(code, new() { Delay = 100 });

        await AssertVisibleText("2FA configurado com sucesso!");
        await ClickOn(Button("Continuar"));
    }

    [Test]
    public async Task Should_not_setup_mfa_with_wrong_code()
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
        await Page.GetByRole(AriaRole.Banner).GetByRole(AriaRole.Link).Nth(1).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Img)).ToBeVisibleAsync();

        var code = Guid.NewGuid().ToHashCode().ToString()[..6];
        await Page.Locator("input[type=\"text\"]").PressSequentiallyAsync(code, new() { Delay = 100 });

        await AssertVisibleText("Código inválido");
    }
}
