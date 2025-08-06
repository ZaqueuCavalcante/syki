namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_webhook_subscription()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.CreateWebhookSubscription();

        // Assert
        response.ShouldBeSuccess();
        CreateWebhookSubscriptionOut responseOut = response.Success;

        var ctx = _api.GetDbContext();
        var webhook = await ctx.Webhooks.Include(x => x.Authentication)
            .FirstAsync(x => x.Id == responseOut.Id);

        webhook.Name.Should().Be("Aluno Criado");
        webhook.Url.Should().Be("https://example.com/webhook");
        webhook.Events.Should().BeEquivalentTo([WebhookEventType.StudentCreated]);
        webhook.Authentication.Type.Should().Be(WebhookAuthenticationType.ApiKey);
        webhook.Authentication.ApiKey.Should().Be("z3Q6uDUJYTDCIo16myBKZrlCS63IvpCUOAE5X");
    }

    [Test]
    public async Task Should_not_create_webhook_subscription_without_events()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.CreateWebhookSubscription(events: []);

        // Assert
        response.ShouldBeError(new InvalidWebhookEventsList());
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public async Task Should_not_create_webhook_subscription_with_empty_api_key(string? apiKey)
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.CreateWebhookSubscription(apiKey: apiKey);

        // Assert
        response.ShouldBeError(new InvalidWebhookAuthentication());
    }
}
