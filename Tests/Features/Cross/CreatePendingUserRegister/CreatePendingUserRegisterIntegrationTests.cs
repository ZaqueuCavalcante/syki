using Syki.Back.Features.Cross.CreatePendingUserRegister;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_a_pending_user_register()
    {
        // Arrange
        var client = _api.GetClient();
        var email = TestData.Email;

        // Act
        var response = await client.CreatePendingUserRegister(email);

        // Assert
        response.ShouldBeSuccess();

        await using var ctx = _api.GetDbContext();
        var register = await ctx.UserRegisters.FirstAsync(x => x.Email == email);
        register.Id.Should().NotBeEmpty();
        register.TrialStart.Should().BeNull();
        register.TrialEnd.Should().BeNull();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidEmails))]
    public async Task Should_not_create_a_pending_user_register_with_invalid_email(string email)
    {
        // Arrange
        var client = _api.GetClient();

        // Act
        var response = await client.CreatePendingUserRegister(email);

        // Assert
        response.ShouldBeError(new InvalidEmail());

        await using var ctx = _api.GetDbContext();
        var register = await ctx.UserRegisters.FirstOrDefaultAsync(x => x.Email == email);
        register.Should().BeNull();
    }

    [Test]
    public async Task Should_not_create_a_pending_user_register_with_duplicated_email()
    {
        // Arrange
        var client = _api.GetClient();
        var email = TestData.Email;

        // Act
        var firstResponse = await client.CreatePendingUserRegister(email);
        var secondResponse = await client.CreatePendingUserRegister(email);

        // Assert
        firstResponse.ShouldBeSuccess();
        secondResponse.ShouldBeError(new EmailAlreadyUsed());

        await using var ctx = _api.GetDbContext();
        var register = await ctx.UserRegisters.SingleAsync(x => x.Email == email);
        register.TrialStart.Should().BeNull();
        register.TrialEnd.Should().BeNull();
    }

    [Test]
    public async Task Should_not_create_a_pending_user_register_with_duplicated_case_insensitive_email()
    {
        // Arrange
        var client = _api.GetClient();
        const string email = "zaqueu.648618168711@syki.com";
        const string email2 = "ZaqueU.648618168711@syki.com";

        // Act
        var firstResponse = await client.CreatePendingUserRegister(email);
        var secondResponse = await client.CreatePendingUserRegister(email2);

        // Assert
        firstResponse.ShouldBeSuccess();
        secondResponse.ShouldBeError(new EmailAlreadyUsed());

        using var ctx = _api.GetDbContext();
        var register = await ctx.UserRegisters.SingleAsync(x => x.Email == email);
        register.TrialStart.Should().BeNull();
        register.TrialEnd.Should().BeNull();
    }

    [Test]
    public async Task Should_enqueue_a_send_user_register_email_confirmation_task_on_pending_user_registration()
    {
        // Arrange
        var client = _api.GetClient();
        var email = TestData.Email;

        // Act
        var response = await client.CreatePendingUserRegister(email);

        // Assert
        response.ShouldBeSuccess();
        await AssertTaskByDataLike<SendUserRegisterEmailConfirmation>(email);
    }
}
