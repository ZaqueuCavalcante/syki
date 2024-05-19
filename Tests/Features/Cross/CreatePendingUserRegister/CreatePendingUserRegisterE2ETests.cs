namespace Syki.Tests.Features.Cross.CreatePendingUserRegister;

public class CreatePendingUserRegisterE2ETests : E2ETestBase
{
    // [Test]
    public async Task Should_create_a_pending_register()
    {
        await Goto("/");

        await ClickOn(Link("Experimente agora"));

        await Fill(TestData.Email);
        await ClickOn(Button("Cadastrar"));

        await AssertVisibleText("Verifique seu email");
    }
}
