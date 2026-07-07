namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Webhooks_CreateWebhookSubscription_Should_not_create_webhook_subscription_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateWebhookSubscription();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Webhooks_CreateWebhookSubscription_Should_not_create_webhook_subscription_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateWebhookSubscription();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCase("")]
    public async Task Webhooks_CreateWebhookSubscription_Should_not_create_webhook_subscription_with_invalid_name(string name)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateWebhookSubscription(name: name);

        // Assert
        result.ShouldBeError(InvalidWebhookName.I);
    }

    [Test]
    public async Task Webhooks_CreateWebhookSubscription_Should_not_create_webhook_subscription_when_name_is_too_long()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateWebhookSubscription(name: new string('a', 101));

        // Assert
        result.ShouldBeError(InvalidWebhookName.I);
    }

    [Test]
    [TestCase("")]
    [TestCase("not-a-url")]
    public async Task Webhooks_CreateWebhookSubscription_Should_not_create_webhook_subscription_with_invalid_url(string url)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateWebhookSubscription(url: url);

        // Assert
        result.ShouldBeError(InvalidWebhookUrl.I);
    }

    [Test]
    public async Task Webhooks_CreateWebhookSubscription_Should_not_create_webhook_subscription_with_empty_events()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateWebhookSubscription(events: []);

        // Assert
        result.ShouldBeError(InvalidWebhookEvents.I);
    }

    [Test]
    public async Task Webhooks_CreateWebhookSubscription_Should_not_create_webhook_subscription_with_invalid_custom_headers()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateWebhookSubscription(customHeaders: new() { ["Authorization"] = "" });

        // Assert
        result.ShouldBeError(InvalidWebhookCustomHeaders.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Webhooks_CreateWebhookSubscription_Should_create_webhook_subscription()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateWebhookSubscription(
            name: "Aluno criado",
            url: "https://webhook.site/my-webhook",
            events: [WebhookEventType.StudentCreated, WebhookEventType.ClassActivityCreated],
            customHeaders: new() { ["Exato-AuthToken"] = "6r4g654rs6g4we6f4qw684f68qwf4" });

        // Assert
        var subscription = result.Success;
        subscription.Id.Should().BeGreaterThan(0);
    }

    #endregion
}
