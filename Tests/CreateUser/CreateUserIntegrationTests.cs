using Syki.Back.CreateUser;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAdm))]
    public async Task Should_create_a_user(string role)
    {
        // Arrange
        var client = _factory.CreateClient();
        var institution = await client.CreateInstitution();

        var userIn = new CreateUserIn
        {
            InstitutionId = institution.Id,
            Name = "Zaqueu",
            Email = TestData.Email,
            Password = "My@new@strong@P4ssword",
            Role = role,
        };

        var service = _factory.GetService<CreateUserService>();

        // Act
        var user = await service.Create(userIn);

        // Assert
        user.Id.Should().NotBeEmpty();
        user.InstitutionId.Should().Be(userIn.InstitutionId);
        user.Nome.Should().Be(userIn.Name);
        user.Email.Should().Be(userIn.Email);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAdm))]
    public async Task Should_create_a_user_with_right_role(string role)
    {
        // Arrange
        var client = _factory.CreateClient();
        var institution = await client.CreateInstitution();

        var userIn = new CreateUserIn
        {
            InstitutionId = institution.Id,
            Name = "Zaqueu",
            Email = TestData.Email,
            Password = "My@new@strong@P4ssword",
            Role = role,
        };

        var service = _factory.GetService<CreateUserService>();

        // Act
        var userOut = await service.Create(userIn);

        // Assert
        using var ctx = _factory.GetDbContext();
        var userRoles = await ctx.UserRoles.Where(u => u.UserId == userOut.Id).ToListAsync();
        userRoles.Should().ContainSingle();
        var userRole = await ctx.Roles.FirstAsync(r => r.Id == userRoles[0].RoleId);
        userRole.Name.Should().Be(role);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.InvalidRoles))]
    public async Task Should_not_create_a_user_with_invalid_role(string role)
    {
        // Arrange
        var client = _factory.CreateClient();
        var institution = await client.CreateInstitution();

        var userIn = new CreateUserIn
        {
            InstitutionId = institution.Id,
            Name = "Zaqueu",
            Email = TestData.Email,
            Password = "My@new@strong@P4ssword",
            Role = role,
        };

        var service = _factory.GetService<CreateUserService>();

        // Act
        Func<Task> act = async () => { await service.Create(userIn); };

		// Assert
		await act.Should().ThrowAsync<DomainException>().WithMessage(Throw.DE013);
    }

    // [Test]
    public async Task Should_not_create_a_user_without_institution()
    {
        // Arrange
        var userIn = new CreateUserIn
        {
            InstitutionId = Guid.NewGuid(),
            Name = "Zaqueu",
            Email = TestData.Email,
            Password = "My@new@strong@P4ssword",
            Role = Academico,
        };

        var service = _factory.GetService<CreateUserService>();

        // Act
        Func<Task> act = async () => { await service.Create(userIn); };

		// Assert
		await act.Should().ThrowAsync<DomainException>().WithMessage(Throw.DE014);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.InvalidEmails))]
    public async Task Should_not_create_a_user_with_invalid_email(string email)
    {
        // Arrange
        var client = _factory.CreateClient();
        var institution = await client.CreateInstitution();

        var userIn = new CreateUserIn
        {
            InstitutionId = institution.Id,
            Name = "Zaqueu",
            Email = email,
            Password = "My@new@strong@P4ssword",
            Role = Academico,
        };

        var service = _factory.GetService<CreateUserService>();

        // Act
        Func<Task> act = async () => { await service.Create(userIn); };

		// Assert
		await act.Should().ThrowAsync<DomainException>().WithMessage(Throw.DE016);
    }

    // [Test]
    public async Task Should_not_create_a_user_with_duplicated_email()
    {
        // Arrange
        var client = _factory.CreateClient();
        var institution = await client.CreateInstitution();

        var userIn = new CreateUserIn
        {
            InstitutionId = institution.Id,
            Name = "Zaqueu",
            Email = TestData.Email,
            Password = "My@new@strong@P4ssword",
            Role = Academico,
        };

        var service = _factory.GetService<CreateUserService>();
        await service.Create(userIn);

        // Act
        Func<Task> act = async () => { await service.Create(userIn); };

		// Assert
		await act.Should().ThrowAsync<DomainException>().WithMessage(Throw.DE017);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.InvalidPasswords))]
    public async Task Should_not_create_a_user_with_invalid_password(string password)
    {
        // Arrange
        var client = _factory.CreateClient();
        var institution = await client.CreateInstitution();

        var userIn = new CreateUserIn
        {
            InstitutionId = institution.Id,
            Name = "Zaqueu",
            Email = TestData.Email,
            Password = password,
            Role = Academico,
        };

        var service = _factory.GetService<CreateUserService>();

        // Act
        Func<Task> act = async () => { await service.Create(userIn); };

		// Assert
		await act.Should().ThrowAsync<DomainException>().WithMessage(Throw.DE015);
    }
}
