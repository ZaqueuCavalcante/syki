using Syki.Back.Tasks;
using Syki.Back.Exceptions;
using Microsoft.EntityFrameworkCore;
using Syki.Shared.CreatePendingDemo;

namespace Syki.Tests.CreatePendingDemo;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_a_pending_demo()
    {
        // Arrange
        var client = _factory.CreateClient();
        var body = new CreatePendingDemoIn { Email = "demo00.facul@syki.com" };

        // Act
        var response = await client.PostHttpAsync("/demos", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task Should_not_create_a_pending_demo_with_invalid_email()
    {
        // Arrange
        var client = _factory.CreateClient();
        var body = new CreatePendingDemoIn { Email = "demo01.faculsyki.com" };

        // Act
        var response = await client.PostHttpAsync("/demos", body);

        // Assert
        await response.AssertBadRequest(Throw.DE016);
    }

    [Test]
    public async Task Should_not_create_a_pending_demo_with_duplicated_email()
    {
        // Arrange
        var client = _factory.CreateClient();
        var body = new CreatePendingDemoIn { Email = "demo02.facul@syki.com" };
        await client.PostHttpAsync("/demos", body);

        // Act
        var response = await client.PostHttpAsync("/demos", body);

        // Assert
        await response.AssertBadRequest(Throw.DE017);
    }

    [Test]
    public async Task Should_enqueue_a_send_demo_email_confirmation_task_on_pending_demo_creation()
    {
        // Arrange
        var client = _factory.CreateClient();
        var body = new CreatePendingDemoIn { Email = "demo03.facul@syki.com" };

        // Act
        await client.PostHttpAsync("/demos", body);

        // Assert
        using var ctx = _factory.GetDbContext();
        FormattableString sql = $@"
            SELECT *
            FROM syki.tasks
            WHERE data LIKE '%demo03.facul@syki.com%'
        ";
        var tasks = await ctx.Database.SqlQuery<SykiTask>(sql).ToListAsync();
        tasks.Should().ContainSingle();
        typeof(SykiTask).Assembly.GetType(tasks[0].Type).Should().Be<SendDemoEmailConfirmation>();
    }

    [Test]
    public async Task Should_return_error_when_demo_not_found()
    {
        // Arrange
        var task = new SendDemoEmailConfirmation { Email = "demo04.facul@syki.com" };
        var handler = _factory.GetService<SendDemoEmailConfirmationHandler>();

        // Act
        Func<Task> act = async () => { await handler.Handle(task); };

		// Assert
		await act.Should().ThrowAsync<DomainException>().WithMessage(Throw.DE024);
    }

    [Test]
    public async Task Should_not_return_error_when_demo_exists()
    {
        // Arrange
        var client = _factory.CreateClient();
        var body = new CreatePendingDemoIn { Email = "demo05.facul@syki.com" };
        await client.PostHttpAsync("/demos", body);

        var task = new SendDemoEmailConfirmation { Email = "demo05.facul@syki.com" };
        var handler = _factory.GetService<SendDemoEmailConfirmationHandler>();

        // Act
        Func<Task> act = async () => { await handler.Handle(task); };

		// Assert
		await act.Should().NotThrowAsync<DomainException>();
    }
}
