using Syki.Back.CreatePendingUserRegister;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_a_pending_user_register()
    {
        // Arrange
        var client = _factory.GetClient();

        // Act
        var response = await client.CreatePendingUserRegister(TestData.Email);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task Should_not_create_a_pending_user_register_with_invalid_email()
    {
        // Arrange
        var client = _factory.GetClient();

        // Act
        var response = await client.CreatePendingUserRegister("zaqueu.com");

        // Assert
        await response.AssertBadRequest(Throw.DE016);
    }

    [Test]
    public async Task Should_not_create_a_pending_user_register_with_duplicated_email()
    {
        // Arrange
        var client = _factory.GetClient();
        var email = TestData.Email;

        // Act
        await client.CreatePendingUserRegister(email);
        var response = await client.CreatePendingUserRegister(email);

        // Assert
        await response.AssertBadRequest(Throw.DE017);
    }

    [Test]
    public async Task Should_enqueue_a_send_user_register_email_confirmation_task_on_pending_user_register_creation()
    {
        // Arrange
        var client = _factory.GetClient();
        var email = TestData.Email;

        // Act
        await client.CreatePendingUserRegister(email);

        // Assert
        using var ctx = _factory.GetDbContext();
        var emailFormat = $"%{email}%";
        FormattableString sql = $@"
            SELECT *
            FROM syki.tasks
            WHERE data LIKE {emailFormat}
        ";
        var tasks = await ctx.Database.SqlQuery<SykiTask>(sql).ToListAsync();
        tasks.Should().ContainSingle();
        typeof(SykiTask).Assembly.GetType(tasks[0].Type).Should().Be<SendUserRegisterEmailConfirmation>();
    }

    [Test]
    public async Task Should_return_error_when_user_register_not_found()
    {
        // Arrange
        var task = new SendUserRegisterEmailConfirmation { Email = TestData.Email };
        var handler = _factory.GetService<SendUserRegisterEmailConfirmationHandler>();

        // Act
        Func<Task> act = async () => { await handler.Handle(task); };

		// Assert
		await act.Should().ThrowAsync<DomainException>().WithMessage(Throw.DE024);
    }

    [Test]
    public async Task Should_not_return_error_when_user_register_exists()
    {
        // Arrange
        var client = _factory.GetClient();

        var email = TestData.Email;
        await client.CreatePendingUserRegister(email);

        var task = new SendUserRegisterEmailConfirmation { Email = email};
        var handler = _factory.GetService<SendUserRegisterEmailConfirmationHandler>();

        // Act
        Func<Task> act = async () => { await handler.Handle(task); };

		// Assert
		await act.Should().NotThrowAsync<DomainException>();
    }
}
