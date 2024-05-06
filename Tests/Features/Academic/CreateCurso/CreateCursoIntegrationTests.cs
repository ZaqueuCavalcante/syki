using static Syki.Shared.CourseType;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_a_bew_course()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        var curso = await client.CreateCurso("Análise e Desenvolvimento de Sistemas", Bacharelado);

        // Assert
        curso.Id.Should().NotBeEmpty();
        curso.Name.Should().Be("Análise e Desenvolvimento de Sistemas");
        curso.Type.Should().Be(Bacharelado);
    }

    [Test]
    public async Task Should_create_many_courses()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        await client.CreateCurso("Análise e Desenvolvimento de Sistemas", Bacharelado);
        await client.CreateCurso("Direito", Licenciatura);
        
        // Assert
        var cursos = await client.GetCursos();
        cursos.Should().HaveCount(2);
    }
}
