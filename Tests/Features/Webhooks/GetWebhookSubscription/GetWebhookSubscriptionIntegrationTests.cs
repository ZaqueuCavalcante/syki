namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Webhooks_GetWebhookSubscription_Should_not_get_webhook_subscription_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetWebhookSubscription(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Webhooks_GetWebhookSubscription_Should_not_get_webhook_subscription_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetWebhookSubscription(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Webhooks_GetWebhookSubscription_Should_not_get_webhook_subscription_when_it_does_not_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetWebhookSubscription(999999);

        // Assert
        result.ShouldBeError(WebhookSubscriptionNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Webhooks_GetWebhookSubscription_Should_get_webhook_subscription()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var created = (await client.CreateWebhookSubscription(
            name: "Aluno criado",
            url: "https://webhook.site/my-webhook",
            events: [WebhookEventType.StudentCreated],
            customHeaders: new() { ["Estud-AuthToken"] = "6r4g654rs6g4we6f4qw684f68qwf4" })).Success;

        // Act
        var result = await client.GetWebhookSubscription(created.Id);

        // Assert
        var subscription = result.Success;
        subscription.Id.Should().Be(created.Id);
        subscription.Name.Should().Be("Aluno criado");
        subscription.Url.Should().Be("https://webhook.site/my-webhook");
        subscription.IsActive.Should().BeTrue();
        subscription.Events.Should().BeEquivalentTo([WebhookEventType.StudentCreated]);
        subscription.CustomHeaders.Should().ContainKey("Estud-AuthToken").WhoseValue.Should().Be("6r4g654rs6g4we6f4qw684f68qwf4");
    }

    #endregion
}
