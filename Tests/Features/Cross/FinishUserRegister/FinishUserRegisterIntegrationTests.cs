namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_finish_user_register()
    {
        // Arrange
        var client = _back.GetClient();

        var email = TestData.Email;
        await client.CreatePendingUserRegister(email);
        var token = await _back.GetRegisterSetupToken(email);

        // Act
        var response = await client.FinishUserRegister(token!, "Lalala@123");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var ctx = _back.GetDbContext();
        using var userManager = _back.GetUserManager();

        var register = await ctx.UserRegisters.FirstAsync(x => x.Email == email);
        register.TrialStart.Should().Be(DateOnly.FromDateTime(DateTime.Now));
        register.TrialEnd.Should().Be(DateOnly.FromDateTime(DateTime.Now.AddDays(7)));

        var institution = await ctx.Institutions.SingleAsync(x => x.Name.Contains(email));
        institution.Id.Should().NotBeEmpty();

        var user = await userManager.FindByEmailAsync(email);
        user!.Name.Should().Be(email);
        user!.Email.Should().Be(email);
        user!.TwoFactorEnabled.Should().BeFalse();

        var isOnlyInAcademicRole = await userManager.IsOnlyInRole(user!, UserRole.Academic);
        isOnlyInAcademicRole.Should().BeTrue();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidUserRegisterTokens))]
    public async Task Should_not_finish_user_register_with_a_invalid_token(string token)
    {
        // Arrange
        var client = _back.GetClient();

        var email = TestData.Email;
        await client.CreatePendingUserRegister(email);

        // Act
        var response = await client.FinishUserRegister(token, "Lalala@123");

        // Assert
        await response.AssertBadRequest(new InvalidRegistrationToken());

        using var ctx = _back.GetDbContext();
        using var userManager = _back.GetUserManager();

        var register = await ctx.UserRegisters.FirstAsync(x => x.Email == email);
        register.TrialStart.Should().BeNull();
        register.TrialEnd.Should().BeNull();

        var institution = await ctx.Institutions.FirstOrDefaultAsync(x => x.Name.Contains(email));
        institution.Should().BeNull();

        var user = await userManager.FindByEmailAsync(email);
        user.Should().BeNull();
    }

    [Test]
    public async Task Should_not_register_user_twice()
    {
        // Arrange
        var client = _back.GetClient();

        var email = TestData.Email;
        await client.CreatePendingUserRegister(email);

        var token = await _back.GetRegisterSetupToken(email);
        var firstResponse = await client.FinishUserRegister(token!, "Lalala@123");

        // Act
        var secondResponse = await client.FinishUserRegister(token!, "Lalala@123");

        // Assert
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        await secondResponse.AssertBadRequest(new UserAlreadyRegistered());

        using var ctx = _back.GetDbContext();
        using var userManager = _back.GetUserManager();

        var register = await ctx.UserRegisters.FirstAsync(x => x.Email == email);
        register.TrialStart.Should().Be(DateOnly.FromDateTime(DateTime.Now));
        register.TrialEnd.Should().Be(DateOnly.FromDateTime(DateTime.Now.AddDays(7)));

        var institution = await ctx.Institutions.SingleAsync(x => x.Name.Contains(email));
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
        var client = _back.GetClient();

        var email = TestData.Email;
        await client.CreatePendingUserRegister(email);
        var token = await _back.GetRegisterSetupToken(email);

        // Act
        var response = await client.FinishUserRegister(token!, password);

        // Assert
        await response.AssertBadRequest(new WeakPassword());

        using var ctx = _back.GetDbContext();
        using var userManager = _back.GetUserManager();

        var register = await ctx.UserRegisters.FirstAsync(x => x.Email == email);
        register.TrialStart.Should().BeNull();
        register.TrialEnd.Should().BeNull();

        var institution = await ctx.Institutions.FirstOrDefaultAsync(x => x.Name.Contains(email));
        institution.Should().BeNull();

        var user = await userManager.FindByEmailAsync(email);
        user.Should().BeNull();
    }
}
