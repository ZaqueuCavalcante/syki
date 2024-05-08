namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_get_academic_insights()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        await client.CreateCampus();
        await client.CreateDisciplina();
        await client.CreateCurso();

        // Act
        var response = await client.GetAcademicInsights();

        // Assert
        response.Campus.Should().Be(1);
        response.Cursos.Should().Be(1);
        response.Disciplinas.Should().Be(1);
        response.Grades.Should().Be(0);
        response.Ofertas.Should().Be(0);
        response.Turmas.Should().Be(0);
        response.Professores.Should().Be(0);
        response.Alunos.Should().Be(0);
        response.Notifications.Should().Be(0);
    }
}
