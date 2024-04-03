using Syki.Back.Features.Cross.CreatePendingUserRegister;

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
        await AssertTaskByDataLike<SendUserRegisterEmailConfirmation>(email);
    }
}
