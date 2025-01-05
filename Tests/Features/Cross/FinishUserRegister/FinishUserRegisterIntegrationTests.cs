using Syki.Back.Features.Cross.CreateInstitution;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_finish_user_register()
    {
        // Arrange
        var client = _api.GetClient();

        var email = TestData.Email;
        await client.CreatePendingUserRegister(email);
        var token = await _api.GetRegisterSetupToken(email);

        // Act
        var response = await client.FinishUserRegister(token!, "Lalala@123");

        // Assert
        response.ShouldBeSuccess();

        await using var ctx = _api.GetDbContext();
        using var userManager = _api.GetUserManager();

        var register = await ctx.UserRegisters.FirstAsync(x => x.Email == email);
        register.Status.Should().Be(UserRegisterStatus.Finished);

        var institution = await ctx.Institutions.SingleAsync(x => x.Id == response.GetSuccess().InstitutionId);
        institution.Id.Should().NotBeEmpty();

        var user = await userManager.FindByEmailAsync(email);
        user!.Name.Should().Be(email);
        user!.Email.Should().Be(email);
        user!.TwoFactorEnabled.Should().BeFalse();

        var isOnlyInAcademicRole = await userManager.IsOnlyInRole(user!, UserRole.Academic);
        isOnlyInAcademicRole.Should().BeTrue();

        await AssertDomainEvent<InstitutionCreatedDomainEvent>(institution.Id.ToString());
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidUserRegisterTokens))]
    public async Task Should_not_finish_user_register_with_a_invalid_token(string token)
    {
        // Arrange
        var client = _api.GetClient();

        var email = TestData.Email;
        await client.CreatePendingUserRegister(email);

        // Act
        var response = await client.FinishUserRegister(token, "Lalala@123");

        // Assert
        response.ShouldBeError(new InvalidRegistrationToken());

        using var ctx = _api.GetDbContext();
        using var userManager = _api.GetUserManager();

        var register = await ctx.UserRegisters.FirstAsync(x => x.Email == email);
        register.Status.Should().Be(UserRegisterStatus.Pending);

        var institution = await ctx.Institutions.FirstOrDefaultAsync(x => x.Name.Contains(email));
        institution.Should().BeNull();

        var user = await userManager.FindByEmailAsync(email);
        user.Should().BeNull();
    }

    [Test]
    public async Task Should_not_register_user_twice()
    {
        // Arrange
        var client = _api.GetClient();

        var email = TestData.Email;
        await client.CreatePendingUserRegister(email);

        var token = await _api.GetRegisterSetupToken(email);
        var firstResponse = await client.FinishUserRegister(token!, "Lalala@123");

        // Act
        var secondResponse = await client.FinishUserRegister(token!, "Lalala@123");

        // Assert
        firstResponse.ShouldBeSuccess();
        secondResponse.ShouldBeError(new UserAlreadyRegistered());

        using var ctx = _api.GetDbContext();
        using var userManager = _api.GetUserManager();

        var register = await ctx.UserRegisters.FirstAsync(x => x.Email == email);
        register.Status.Should().Be(UserRegisterStatus.Finished);

        var institution = await ctx.Institutions.SingleAsync(x => x.Id == firstResponse.GetSuccess().InstitutionId);
        institution.Id.Should().NotBeEmpty();

        var user = await userManager.FindByEmailAsync(email);
        user!.Name.Should().Be(email);
        user!.Email.Should().Be(email);
        user!.TwoFactorEnabled.Should().BeFalse();

        var isOnlyInAcademicRole = await userManager.IsOnlyInRole(user!, UserRole.Academic);
        isOnlyInAcademicRole.Should().BeTrue();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidPasswords))]
    public async Task Should_not_register_user_with_a_invalid_password(string password)
    {
        // Arrange
        var client = _api.GetClient();

        var email = TestData.Email;
        await client.CreatePendingUserRegister(email);
        var token = await _api.GetRegisterSetupToken(email);

        // Act
        var response = await client.FinishUserRegister(token!, password);

        // Assert
        response.ShouldBeError(new WeakPassword());

        using var ctx = _api.GetDbContext();
        using var userManager = _api.GetUserManager();

        var register = await ctx.UserRegisters.FirstAsync(x => x.Email == email);
        register.Status.Should().Be(UserRegisterStatus.Pending);

        var institution = await ctx.Institutions.FirstOrDefaultAsync(x => x.Name.Contains(email));
        institution.Should().BeNull();

        var user = await userManager.FindByEmailAsync(email);
        user.Should().BeNull();
    }
}
