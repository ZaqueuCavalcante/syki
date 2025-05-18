using Syki.Back.Emails;
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
        register.Email.Should().Be(email);
        register.InstitutionId.Should().NotBeEmpty();
        register.Status.Should().Be(UserRegisterStatus.Pending);

        await AssertDomainEvent<PendingUserRegisterCreatedDomainEvent>(register.Id.ToString());
    }

    [Test]
    public async Task Should_not_create_a_pending_user_register_with_invalid_email()
    {
        // Arrange
        var client = _api.GetClient();
        var email = TestData.InvalidEmails().PickRandom().First().ToString()!;

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
        var registers = await ctx.UserRegisters.Where(x => x.Email == email).ToListAsync();
        registers.Should().ContainSingle();
    }

    [Test]
    public async Task Should_not_create_a_pending_user_register_with_duplicated_case_insensitive_email()
    {
        // Arrange
        var client = _api.GetClient();
        var email = TestData.Email;
        var emailLow = $"a{email}";
        var emailUpper = $"A{email}";

        // Act
        var firstResponse = await client.CreatePendingUserRegister(emailLow);
        var secondResponse = await client.CreatePendingUserRegister(emailUpper);

        // Assert
        firstResponse.ShouldBeSuccess();
        secondResponse.ShouldBeError(new EmailAlreadyUsed());

        using var ctx = _api.GetDbContext();
        var registers = await ctx.UserRegisters.Where(x => x.Email == emailLow).ToListAsync();
        registers.Should().ContainSingle();
    }

    [Test]
    public async Task Should_send_a_email_confirmation_after_create_a_pending_user_register()
    {
        // Arrange
        var client = _api.GetClient();
        var email = TestData.Email;

        // Act
        var response = await client.CreatePendingUserRegister(email);

        // Assert
        response.ShouldBeSuccess();

        await _daemon.AwaitEventsProcessing();
        await _daemon.AwaitCommandsProcessing();

        var service = _daemon.GetService<IEmailsService>() as FakeEmailsService;
        service!.UserRegisterEmailConfirmationEmails.Should().ContainSingle(x => x.Contains(email));
    }
}
