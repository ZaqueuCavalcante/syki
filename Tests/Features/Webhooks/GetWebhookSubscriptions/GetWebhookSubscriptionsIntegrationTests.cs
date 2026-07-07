namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Webhooks_GetWebhookSubscriptions_Should_not_get_webhook_subscriptions_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetWebhookSubscriptions();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Webhooks_GetWebhookSubscriptions_Should_not_get_webhook_subscriptions_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetWebhookSubscriptions();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Webhooks_GetWebhookSubscriptions_Should_get_webhook_subscriptions()
    {
        // Arrange
        var client = await _back.LoggedAsDirector("director@webhooks-get-all.com");
        await client.CreateWebhookSubscription(name: "Aluno criado", events: [WebhookEventType.StudentCreated]);
        await client.CreateWebhookSubscription(name: "Atividade publicada", events: [WebhookEventType.ClassActivityCreated]);

        // Act
        var result = await client.GetWebhookSubscriptions();

        // Assert
        var subscriptions = result.Success;
        subscriptions.Total.Should().Be(2);
        subscriptions.Items.Should().HaveCount(2);
        subscriptions.Items.Should().Contain(x => x.Name == "Aluno criado");
        subscriptions.Items.Should().Contain(x => x.Name == "Atividade publicada");
    }

    [Test]
    public async Task Webhooks_GetWebhookSubscriptions_Should_get_empty_list_when_no_webhook_subscriptions_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector("director@webhooks-get-empty.com");

        // Act
        var result = await client.GetWebhookSubscriptions();

        // Assert
        var subscriptions = result.Success;
        subscriptions.Total.Should().Be(0);
        subscriptions.Items.Should().BeEmpty();
    }

    #endregion
}
