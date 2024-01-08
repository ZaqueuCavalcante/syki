using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Database;
using Syki.Back.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[Parallelizable(ParallelScope.All)]
public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_aluno_o_vinculando_com_uma_oferta()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) });
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });
        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id, });

        var body = new AlunoIn { Nome = "Zaqueu", OfertaId = oferta.Id };

        // Act
        var response = await client.PostAsync<AlunoOut>("/alunos", body);

        // Assert
        response.Id.Should().NotBeEmpty(); 
        response.OfertaId.Should().Be(oferta.Id); 
        response.Nome.Should().Be("Zaqueu"); 
    }

    [Test]
    public async Task Nao_deve_criar_um_aluno_sem_vinculo_com_oferta()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var body = new AlunoIn { Nome = "Zaqueu", OfertaId = Guid.NewGuid() };

        // Act
        var response = await client.PostAsync("/alunos", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0009);  
    }

    [Test]
    public async Task Deve_retornar_as_disciplinas_cursadas_pelo_aluno()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        var userBody = UserIn.New(faculdade.Id, Aluno);
        var userAluno = await client.PostAsync<UserOut>("/users", userBody);
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) });
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });

        var disciplina01 = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [curso.Id] });
        var disciplina02 = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Estrutura de Dados", CargaHoraria = 60, Cursos = [curso.Id] });
        var disciplina03 = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Programação Orientada a Objetos", CargaHoraria = 55, Cursos = [curso.Id] });
        var disciplinas = new List<GradeDisciplinaIn>() { new() { Id = disciplina01.Id }, new() { Id = disciplina02.Id }, new() { Id = disciplina03.Id } };

        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, Disciplinas = disciplinas });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id });

        var aluno = await client.PostAsync<AlunoOut>("/alunos", new AlunoIn { Nome = "Zaqueu", OfertaId = oferta.Id });

        // GAMBIARRA
        using var scope = _factory.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();
        var alunoDb = await ctx.Alunos.FirstAsync(a => a.Id == aluno.Id);
        alunoDb.UserId = userAluno.Id;
        await ctx.SaveChangesAsync();
        // GAMBIARRA

        await client.Login(userBody.Email, userBody.Password);

        // Act
        var response = await client.GetAsync<List<DisciplinaOut>>("/alunos/disciplinas");

        // Assert
        response.Count.Should().Be(3); 
    }

    [Test]
    public async Task Deve_retornar_os_alunos()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) });
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });
        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id, });

        await client.PostAsync("/alunos", new AlunoIn { Nome = "Zaqueu", OfertaId = oferta.Id });
        await client.PostAsync("/alunos", new AlunoIn { Nome = "Maju", OfertaId = oferta.Id });

        // Act
        var response = await client.GetAsync<List<AlunoOut>>("/alunos");

        // Assert
        response.Count.Should().Be(2); 
    }
}
