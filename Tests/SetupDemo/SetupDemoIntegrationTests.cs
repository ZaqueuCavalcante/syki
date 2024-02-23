using Syki.Shared.SetupDemo;
using Syki.Shared.CreatePendingDemo;

namespace Syki.Tests.SetupDemo;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_setup_demo()
    {
        // Arrange
        var client = _factory.CreateClient();
        var email = TestData.Email;
        await client.PostHttpAsync("/demos", new CreatePendingDemoIn { Email = email });

        using var ctx = _factory.GetDbContext();
        var token = await ctx.Demos.Where(d => d.Email == email).Select(d => d.Id).FirstAsync();

        var body = new SetupDemoIn { Token = token.ToString(), Password = "Lalala@123Lalala@123" };

        // Act
        var response = await client.PostHttpAsync("/demos/setup", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidSetupDemoTokens))]
    public async Task Should_not_setup_demo_with_a_invalid_token(string? token)
    {
        // Arrange
        var client = _factory.CreateClient();
        await client.PostHttpAsync("/demos", new CreatePendingDemoIn { Email = TestData.Email });

        var body = new SetupDemoIn { Token = token, Password = "Lalala@123Lalala@123" };

        // Act
        var response = await client.PostHttpAsync("/demos/setup", body);

        // Assert
        await response.AssertBadRequest(Throw.DE024);
    }

    [Test]
    public async Task Should_not_setup_demo_twice()
    {
        // Arrange
        var client = _factory.CreateClient();
        var email = TestData.Email;
        await client.PostHttpAsync("/demos", new CreatePendingDemoIn { Email = email });

        using var ctx = _factory.GetDbContext();
        var token = await ctx.Demos.Where(d => d.Email == email).Select(d => d.Id).FirstAsync();

        var body = new SetupDemoIn { Token = token.ToString(), Password = "Lalala@123Lalala@123" };
        await client.PostHttpAsync("/demos/setup", body);

        // Act
        var response = await client.PostHttpAsync("/demos/setup", body);

        // Assert
        await response.AssertBadRequest(Throw.DE025);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidPasswords))]
    public async Task Should_not_setup_demo_with_a_invalid_password(string password)
    {
        // Arrange
        var client = _factory.CreateClient();
        var email = TestData.Email;
        await client.PostHttpAsync("/demos", new CreatePendingDemoIn { Email = email });

        using var ctx = _factory.GetDbContext();
        var token = await ctx.Demos.Where(d => d.Email == email).Select(d => d.Id).FirstAsync();

        var body = new SetupDemoIn { Token = token.ToString(), Password = password };
        await client.PostHttpAsync("/demos/setup", body);

        // Act
        var response = await client.PostHttpAsync("/demos/setup", body);

        // Assert
        await response.AssertBadRequest(Throw.DE015);
    }

    [Test]
    public async Task Should_create_a_institution_on_setup_demo()
    {
        // Arrange
        var client = _factory.CreateClient();
        var email = TestData.Email;
        await client.PostHttpAsync("/demos", new CreatePendingDemoIn { Email = email });

        using var ctx = _factory.GetDbContext();
        var token = await ctx.Demos.Where(d => d.Email == email).Select(d => d.Id).FirstAsync();

        var body = new SetupDemoIn { Token = token.ToString(), Password = "Lalala@123Lalala@123" };

        // Act
        await client.PostHttpAsync("/demos/setup", body);

        // Assert
        var institution = await ctx.Faculdades.FirstOrDefaultAsync(x => x.Nome.Contains(email));
        institution.Should().NotBeNull();
    }

    [Test]
    public async Task Should_enqueue_a_seed_institution_demo_data_task_on_setup_demo()
    {
        // Arrange
        var client = _factory.CreateClient();
        var email = TestData.Email;
        await client.PostHttpAsync("/demos", new CreatePendingDemoIn { Email = email });

        using var ctx = _factory.GetDbContext();
        var token = await ctx.Demos.Where(d => d.Email == email).Select(d => d.Id).FirstAsync();

        var body = new SetupDemoIn { Token = token.ToString(), Password = "Lalala@123Lalala@123" };

        // Act
        await client.PostHttpAsync("/demos/setup", body);

        // Assert
        var institution = await ctx.Faculdades.FirstAsync(x => x.Nome.Contains(email));
        var id = $"%{institution.Id}%";
        FormattableString sql = $@"
            SELECT *
            FROM syki.tasks
            WHERE data LIKE {id}
        ";
        var tasks = await ctx.Database.SqlQuery<SykiTask>(sql).ToListAsync();
        tasks.Should().ContainSingle();
        typeof(SykiTask).Assembly.GetType(tasks[0].Type).Should().Be<SeedInstitutionDemoData>();
    }

    [Test]
    public async Task Should_create_a_user_academico_on_setup_demo()
    {
        // Arrange
        var client = _factory.CreateClient();
        var email = TestData.Email;
        await client.PostHttpAsync("/demos", new CreatePendingDemoIn { Email = email });

        using var ctx = _factory.GetDbContext();
        var token = await ctx.Demos.Where(d => d.Email == email).Select(d => d.Id).FirstAsync();

        var body = new SetupDemoIn { Token = token.ToString(), Password = "Lalala@123Lalala@123" };

        // Act
        await client.PostHttpAsync("/demos/setup", body);

        // Assert
        var user = await ctx.Users.FirstOrDefaultAsync(x => x.Email == email);
        user.Should().NotBeNull();
    }
}
