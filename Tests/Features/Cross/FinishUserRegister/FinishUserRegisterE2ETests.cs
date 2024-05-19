namespace Syki.Tests.Features.Cross.FinishUserRegister;

public class FinishUserRegisterE2ETests : E2ETestBase
{
    // [Test]
    public async Task Should_finish_a_user_register_by_creating_user_password()
    {
        await Goto("/register");

        var email = await Fill(TestData.Email);
        await ClickOn(Button("Cadastrar"));
        await AssertVisibleText("Verifique seu email");

        var token = await GetRegisterToken(email);
        await Goto($"/register-setup?token={token}");

        await Fill("Test@123Test@123");

        await ClickOn(Button("Salvar"));
        await ClickOn(Button("Ir pro login"));

        await AssertVisibleButton("Login");
    }
}
