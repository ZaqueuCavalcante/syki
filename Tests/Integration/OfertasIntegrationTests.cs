using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class OfertasIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_uma_nova_oferta_de_curso()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        // Campus
        // Curso
        // Grade
        // Periodo
        // Turno

        var curso = await PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" });

        var body = new GradeIn {
            Nome = "Grade de ADS - 1.0",
            CursoId = curso.Id,
        };

        // Act
        var grade = await PostAsync<GradeOut>("/grades", body);

        // Assert
        grade.Id.Should().NotBeEmpty();
        grade.Nome.Should().Be(body.Nome);
        grade.CursoId.Should().Be(curso.Id);
        grade.CursoNome.Should().Be(curso.Nome);
        grade.Disciplinas.Should().HaveCount(0);        
    }
}
