using static Syki.Shared.TipoDeCurso;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_curso()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        var curso = await client.CreateCurso("Análise e Desenvolvimento de Sistemas", Bacharelado);

        // Assert
        curso.Id.Should().NotBeEmpty();
        curso.Nome.Should().Be("Análise e Desenvolvimento de Sistemas");
        curso.Tipo.Should().Be(Bacharelado);
    }

    [Test]
    public async Task Deve_criar_varios_cursos()
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

    [Test]
    public async Task Deve_retornar_os_cursos_ordenados_pelo_nome()
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
        cursos[0].Nome.Should().Be("Administração");
        cursos[1].Nome.Should().Be("Análise e Desenvolvimento de Sistemas");
        cursos[2].Nome.Should().Be("Ciência da Computação");
        cursos[3].Nome.Should().Be("Direito");
        cursos[4].Nome.Should().Be("Pedagogia");
    }
}
