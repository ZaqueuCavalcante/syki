namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Webhooks_UpdateWebhookSubscription_Should_not_update_webhook_subscription_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.UpdateWebhookSubscription(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Webhooks_UpdateWebhookSubscription_Should_not_update_webhook_subscription_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.UpdateWebhookSubscription(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCase("")]
    public async Task Webhooks_UpdateWebhookSubscription_Should_not_update_webhook_subscription_with_invalid_name(string name)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateWebhookSubscription(1, name: name);

        // Assert
        result.ShouldBeError(InvalidWebhookName.I);
    }

    [Test]
    public async Task Webhooks_UpdateWebhookSubscription_Should_not_update_webhook_subscription_when_name_is_too_long()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateWebhookSubscription(1, name: new string('a', 101));

        // Assert
        result.ShouldBeError(InvalidWebhookName.I);
    }

    [Test]
    [TestCase("")]
    [TestCase("not-a-url")]
    public async Task Webhooks_UpdateWebhookSubscription_Should_not_update_webhook_subscription_with_invalid_url(string url)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateWebhookSubscription(1, url: url);

        // Assert
        result.ShouldBeError(InvalidWebhookUrl.I);
    }

    [Test]
    public async Task Webhooks_UpdateWebhookSubscription_Should_not_update_webhook_subscription_with_empty_events()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateWebhookSubscription(1, events: []);

        // Assert
        result.ShouldBeError(InvalidWebhookEvents.I);
    }

    [Test]
    public async Task Webhooks_UpdateWebhookSubscription_Should_not_update_webhook_subscription_with_invalid_custom_headers()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateWebhookSubscription(1, customHeaders: new() { ["Authorization"] = "" });

        // Assert
        result.ShouldBeError(InvalidWebhookCustomHeaders.I);
    }

    [Test]
    public async Task Webhooks_UpdateWebhookSubscription_Should_not_update_webhook_subscription_when_it_does_not_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateWebhookSubscription(999999);

        // Assert
        result.ShouldBeError(WebhookSubscriptionNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Webhooks_UpdateWebhookSubscription_Should_update_webhook_subscription()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var created = (await client.CreateWebhookSubscription()).Success;

        // Act
        var result = await client.UpdateWebhookSubscription(
            created.Id,
            name: "Atividade publicada",
            url: "https://webhook.site/my-other-webhook",
            isActive: false,
            events: [WebhookEventType.ClassActivityCreated],
            customHeaders: new() { ["Estud-AuthToken"] = "new-token-value" });

        // Assert
        var updated = result.Success;
        updated.Id.Should().Be(created.Id);

        var subscription = (await client.GetWebhookSubscription(created.Id)).Success;
        subscription.Name.Should().Be("Atividade publicada");
        subscription.Url.Should().Be("https://webhook.site/my-other-webhook");
        subscription.IsActive.Should().BeFalse();
        subscription.Events.Should().BeEquivalentTo([WebhookEventType.ClassActivityCreated]);
        subscription.CustomHeaders.Should().ContainKey("Estud-AuthToken").WhoseValue.Should().Be("new-token-value");
    }

    #endregion
}
