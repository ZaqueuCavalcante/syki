using Syki.Back.FinishUserRegister;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_finish_user_register()
    {
        // Arrange
        var client = _factory.GetClient();

        var email = TestData.Email;
        await client.CreatePendingUserRegister(email);
        var token = await _factory.GetRegisterSetupToken(email);

        // Act
        var response = await client.FinishUserRegister(token!, "Lalala@123");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidUserRegisterTokens))]
    public async Task Should_not_finish_user_register_with_a_invalid_token(string token)
    {
        // Arrange
        var client = _factory.GetClient();
        await client.CreatePendingUserRegister(TestData.Email);

        // Act
        var response = await client.FinishUserRegister(token, "Lalala@123");

        // Assert
        await response.AssertBadRequest(Throw.DE024);
    }

    [Test]
    public async Task Should_not_register_user_twice()
    {
        // Arrange
        var client = _factory.GetClient();

        var email = TestData.Email;
        await client.CreatePendingUserRegister(email);

        var token = await _factory.GetRegisterSetupToken(email);
        await client.FinishUserRegister(token!, "Lalala@123");

        // Act
        var response = await client.FinishUserRegister(token!, "Lalala@123");

        // Assert
        await response.AssertBadRequest(Throw.DE025);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidPasswords))]
    public async Task Should_not_register_user_with_a_invalid_password(string password)
    {
        // Arrange
        var client = _factory.GetClient();

        var email = TestData.Email;
        await client.CreatePendingUserRegister(email);
        var token = await _factory.GetRegisterSetupToken(email);

        // Act
        var response = await client.FinishUserRegister(token!, password);

        // Assert
        await response.AssertBadRequest(Throw.DE015);
    }

    [Test]
    public async Task Should_create_a_institution_on_user_register()
    {
        // Arrange
        var client = _factory.GetClient();

        var email = TestData.Email;
        await client.CreatePendingUserRegister(email);
        var token = await _factory.GetRegisterSetupToken(email);

        // Act
        var response = await client.FinishUserRegister(token!, "Lalala@123");

        // Assert
        using var ctx = _factory.GetDbContext();
        var institution = await ctx.Institutions.FirstOrDefaultAsync(x => x.Nome.Contains(email));
        institution.Should().NotBeNull();
    }

    [Test]
    public async Task Should_enqueue_seed_institution_data_task_on_user_register()
    {
        // Arrange
        var client = _factory.GetClient();

        var email = TestData.Email;
        await client.CreatePendingUserRegister(email);
        var token = await _factory.GetRegisterSetupToken(email);

        // Act
        var response = await client.FinishUserRegister(token!, "Lalala@123");

        // Assert
        using var ctx = _factory.GetDbContext();
        var institution = await ctx.Institutions.FirstAsync(x => x.Nome.Contains(email));
        await AssertTaskByDataLike<SeedInstitutionData>(institution.Id.ToString());
    }

    [Test]
    public async Task Should_register_user_with_academico_role()
    {
        // Arrange
        var client = _factory.GetClient();

        var email = TestData.Email;
        await client.CreatePendingUserRegister(email);
        var token = await _factory.GetRegisterSetupToken(email);

        // Act
        var response = await client.FinishUserRegister(token!, "Lalala@123");

        // Assert
        using var ctx = _factory.GetDbContext();
        var user = await ctx.Users.FirstOrDefaultAsync(x => x.Email == email);
        user.Should().NotBeNull();
    }
}
