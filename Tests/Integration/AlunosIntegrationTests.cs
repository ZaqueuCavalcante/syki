using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Database;
using Syki.Back.Exceptions;
using Syki.Back.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class AlunosIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_aluno_o_vinculando_com_uma_oferta()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var campus = await PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) });
        var curso = await PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });
        var grade = await PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, });
        var oferta = await PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id, });

        var body = new AlunoIn { Nome = "Zaqueu", OfertaId = oferta.Id };

        // Act
        var response = await PostAsync<AlunoOut>("/alunos", body);

        // Assert
        response.Id.Should().NotBeEmpty(); 
        response.OfertaId.Should().Be(oferta.Id); 
        response.Nome.Should().Be("Zaqueu"); 
    }

    [Test]
    public async Task Nao_deve_criar_um_aluno_sem_vinculo_com_oferta()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new AlunoIn { Nome = "Zaqueu", OfertaId = Guid.NewGuid() };

        // Act
        var response = await _client.PostAsync("/alunos", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0009);  
    }

    [Test]
    public async Task Deve_retornar_as_disciplinas_cursadas_pelo_aluno()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var userBody = UserIn.New(faculdade.Id, Aluno);
        var userAluno = await PostAsync<UserOut>("/users", userBody);
        await RegisterAndLogin(faculdade.Id, Academico);

        var campus = await PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) });
        var curso = await PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });

        var disciplina01 = await PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [curso.Id] });
        var disciplina02 = await PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Estrutura de Dados", CargaHoraria = 60, Cursos = [curso.Id] });
        var disciplina03 = await PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Programação Orientada a Objetos", CargaHoraria = 55, Cursos = [curso.Id] });
        var disciplinas = new List<GradeDisciplinaIn>() { new() { Id = disciplina01.Id }, new() { Id = disciplina02.Id }, new() { Id = disciplina03.Id } };

        var grade = await PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, Disciplinas = disciplinas });
        var oferta = await PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id });

        var aluno = await PostAsync<AlunoOut>("/alunos", new AlunoIn { Nome = "Zaqueu", OfertaId = oferta.Id });

        // GAMBIARRA
        using var scope = _factory.Services.CreateScope();
        _ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();
        var alunoDb = await _ctx.Alunos.FirstAsync(a => a.Id == aluno.Id);
        alunoDb.UserId = userAluno.Id;
        await _ctx.SaveChangesAsync();
        // GAMBIARRA

        await Login(userBody.Email, userBody.Password);

        // Act
        var response = await GetAsync<List<DisciplinaOut>>("/alunos/disciplinas");

        // Assert
        response.Count.Should().Be(3); 
    }

    [Test]
    public async Task Deve_retornar_os_alunos()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, Academico);

        var campus = await PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) });
        var curso = await PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });
        var grade = await PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, });
        var oferta = await PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id, });

        await PostAsync("/alunos", new AlunoIn { Nome = "Zaqueu", OfertaId = oferta.Id });
        await PostAsync("/alunos", new AlunoIn { Nome = "Maju", OfertaId = oferta.Id });

        // Act
        var response = await GetAsync<List<AlunoOut>>("/alunos");

        // Assert
        response.Count.Should().Be(2); 
    }
}
