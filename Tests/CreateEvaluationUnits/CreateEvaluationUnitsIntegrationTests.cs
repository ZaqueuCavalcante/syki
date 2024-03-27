namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_not_create_evaluation_units_with_invalid_turma()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var professor = await client.CreateProfessor();
        var password = "My@new@strong@P4ssword";

        client.RemoveAuthToken();

        await client.SendResetPasswordToken(professor.Email);
        var token = await _factory.GetResetPasswordToken(professor.Email);
        await client.ResetPassword(token!, password);
        await client.Login(professor.Email, password);

        var turmaId = Guid.NewGuid();

        // Act
        var response = await client.CreateEvaluationUnits(turmaId, []);

        // Assert
        await response.AssertBadRequest(Throw.DE029);
    }
}
