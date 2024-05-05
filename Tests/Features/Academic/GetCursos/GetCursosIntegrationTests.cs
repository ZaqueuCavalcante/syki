using static Syki.Shared.CourseType;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_get_all_courses_ordered_by_name()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        await client.CreateCurso("Direito", Tecnologo);
        await client.CreateCurso("Pedagogia", Mestrado);
        await client.CreateCurso("Ciência da Computação", Especializacao);
        await client.CreateCurso("Administração", Doutorado);
        await client.CreateCurso("Análise e Desenvolvimento de Sistemas", Bacharelado);

        // Assert
        var cursos = await client.GetCursos();
        cursos.Should().HaveCount(5);
        cursos[0].Name.Should().Be("Administração");
        cursos[1].Name.Should().Be("Análise e Desenvolvimento de Sistemas");
        cursos[2].Name.Should().Be("Ciência da Computação");
        cursos[3].Name.Should().Be("Direito");
        cursos[4].Name.Should().Be("Pedagogia");
    }
}
