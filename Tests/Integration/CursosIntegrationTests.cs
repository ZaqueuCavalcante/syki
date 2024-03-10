using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using static Syki.Shared.TipoDeCurso;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    // [Test]
    public async Task Deve_criar_um_novo_curso()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        var curso = await client.NewCurso("Análise e Desenvolvimento de Sistemas", Bacharelado);

        // Assert
        curso.Id.Should().NotBeEmpty();
        curso.Nome.Should().Be("Análise e Desenvolvimento de Sistemas");
        curso.Tipo.Should().Be(Bacharelado);
    }

    // [Test]
    public async Task Deve_criar_varios_cursos()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        await client.NewCurso("Análise e Desenvolvimento de Sistemas", Bacharelado);
        await client.NewCurso("Direito", Licenciatura);
        
        // Assert
        var cursos = await client.GetAsync<List<CursoOut>>("/cursos");
        cursos.Should().HaveCount(2);
    }

    // [Test]
    public async Task Deve_retornar_apenas_os_cursos_da_faculdade_do_usuario_logado()
    {
        // Arrange
        var client = _factory.CreateClient();
        var userNovaRoma = await client.NewAcademico("Nova Roma");
        var userUfpe = await client.NewAcademico("UFPE");

        await client.Login(userNovaRoma);
        await client.NewCurso("Análise e Desenvolvimento de Sistemas", Bacharelado);

        await client.Login(userUfpe);
        await client.NewCurso("Direito", Licenciatura);

        // Act
        await client.Login(userNovaRoma);

        var cursos = await client.GetAsync<List<CursoOut>>("/cursos");
        cursos.Should().HaveCount(1);

        // Assert
        cursos[0].Id.Should().NotBeEmpty();
        cursos[0].Nome.Should().Be("Análise e Desenvolvimento de Sistemas");
        cursos[0].Tipo.Should().Be(Bacharelado);
    }

    // [Test]
    public async Task Deve_retornar_os_cursos_ordenados_pelo_nome()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        await client.NewCurso("Direito", Tecnologo);
        await client.NewCurso("Pedagogia", Mestrado);
        await client.NewCurso("Ciência da Computação", Especializacao);
        await client.NewCurso("Administração", Doutorado);
        await client.NewCurso("Análise e Desenvolvimento de Sistemas", Bacharelado);

        // Assert
        var cursos = await client.GetAsync<List<CursoOut>>("/cursos");
        cursos.Should().HaveCount(5);
        cursos[0].Nome.Should().Be("Administração");
        cursos[1].Nome.Should().Be("Análise e Desenvolvimento de Sistemas");
        cursos[2].Nome.Should().Be("Ciência da Computação");
        cursos[3].Nome.Should().Be("Direito");
        cursos[4].Nome.Should().Be("Pedagogia");
    }
}
