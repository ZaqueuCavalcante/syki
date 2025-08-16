namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_webhook_subscription()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CreateWebhookSubscriptionOut webhookSubscription = await client.CreateWebhookSubscription();

        // Act
        var response = await client.GetWebhookSubscription(webhookSubscription.Id);

        // Assert
        response.ShouldBeSuccess();
        GetWebhookSubscriptionOut webhook = response.Success;
        webhook.Name.Should().Be("Aluno Criado");
        webhook.Url.Should().Be("https://example.com/webhook");
        webhook.EventsCount.Should().Be(1);
        webhook.CallsCount.Should().Be(0);
        webhook.AuthenticationType.Should().Be(WebhookAuthenticationType.ApiKey);
    }
}
