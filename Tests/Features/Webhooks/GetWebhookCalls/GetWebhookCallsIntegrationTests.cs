namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Webhooks_GetWebhookCalls_Should_not_get_webhook_calls_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetWebhookCalls();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Webhooks_GetWebhookCalls_Should_not_get_webhook_calls_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetWebhookCalls();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Webhooks_GetWebhookCalls_Should_get_empty_list_when_no_webhook_calls_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector("director@webhook-calls-empty.com");

        // Act
        var result = await client.GetWebhookCalls();

        // Assert
        var calls = result.Success;
        calls.Total.Should().Be(0);
        calls.Page.Should().Be(1);
        calls.PageSize.Should().Be(20);
        calls.Items.Should().BeEmpty();
    }

    [Test]
    public async Task Webhooks_GetWebhookCalls_Should_get_webhook_calls()
    {
        // Arrange
        var client = await _back.LoggedAsDirector("director@webhook-calls-get.com");

        await client.CreateWebhookSubscription(
            url: $"{MocksFactory.Url}/webhooks/target",
            events: [WebhookEventType.StudentCreated]);

        await client.CreateStudent(DataGen.UserName, DataGen.Email);

        await _back.AwaitWebhookCallsProcessing();

        // Act
        var result = await client.GetWebhookCalls();

        // Assert
        var calls = result.Success;
        calls.Total.Should().Be(1);
        calls.Items.Should().HaveCount(1);

        var call = calls.Items.Single();
        call.Id.Should().BePositive();
        call.EventType.Should().Be(WebhookEventType.StudentCreated);
        call.Status.Should().Be(WebhookCallStatus.Success);
        call.AttemptsCount.Should().Be(1);
        call.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(5));
    }

    [Test]
    public async Task Webhooks_GetWebhookCalls_Should_get_webhook_calls_paginated_ordered_by_created_at_desc()
    {
        // Arrange
        var client = await _back.LoggedAsDirector("director@webhook-calls-paginated.com");

        await client.CreateWebhookSubscription(
            url: $"{MocksFactory.Url}/webhooks/target",
            events: [WebhookEventType.StudentCreated]);

        await client.CreateStudent(DataGen.UserName, DataGen.Email);
        await client.CreateStudent(DataGen.UserName, DataGen.Email);
        await client.CreateStudent(DataGen.UserName, DataGen.Email);

        await _back.AwaitWebhookCallsProcessing();

        // Act
        var firstPage = (await client.GetWebhookCalls(page: 1, pageSize: 2)).Success;
        var secondPage = (await client.GetWebhookCalls(page: 2, pageSize: 2)).Success;

        // Assert
        firstPage.Total.Should().Be(3);
        firstPage.Items.Should().HaveCount(2);

        secondPage.Total.Should().Be(3);
        secondPage.Items.Should().HaveCount(1);

        var allIds = firstPage.Items.Concat(secondPage.Items).Select(x => x.Id).ToList();
        allIds.Should().OnlyHaveUniqueItems();

        firstPage.Items.Should().BeInDescendingOrder(x => x.CreatedAt);
    }

    [Test]
    public async Task Webhooks_GetWebhookCalls_Should_get_only_own_institution_webhook_calls()
    {
        // Arrange
        var client1 = await _back.LoggedAsDirector("director@webhook-calls-tenant-1.com");
        await client1.CreateWebhookSubscription(
            url: $"{MocksFactory.Url}/webhooks/target",
            events: [WebhookEventType.StudentCreated]);
        await client1.CreateStudent(DataGen.UserName, DataGen.Email);

        var client2 = await _back.LoggedAsDirector("director@webhook-calls-tenant-2.com");
        await client2.CreateWebhookSubscription(
            url: $"{MocksFactory.Url}/webhooks/target",
            events: [WebhookEventType.StudentCreated]);
        await client2.CreateStudent(DataGen.UserName, DataGen.Email);

        await _back.AwaitWebhookCallsProcessing();

        // Act
        var result = await client2.GetWebhookCalls();

        // Assert
        var calls = result.Success;
        calls.Total.Should().Be(1);
        calls.Items.Should().HaveCount(1);
    }

    #endregion
}
