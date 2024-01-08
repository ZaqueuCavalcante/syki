using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Exceptions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_uma_nova_grade_mesmo_sem_disciplinas()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" });

        var body = new GradeIn {
            Nome = "Grade de ADS - 1.0",
            CursoId = curso.Id,
        };

        // Act
        var grade = await client.PostAsync<GradeOut>("/grades", body);

        // Assert
        grade.Id.Should().NotBeEmpty();
        grade.Nome.Should().Be(body.Nome);
        grade.CursoId.Should().Be(curso.Id);
        grade.CursoNome.Should().Be(curso.Nome);
        grade.Disciplinas.Should().HaveCount(0);        
    }

    [Test]
    public async Task Deve_criar_uma_nova_grade_com_disciplinas()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" });

        var bd = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [curso.Id] });
        var ed = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Estrutura de Dados", CargaHoraria = 60, Cursos = [curso.Id] });
        var poo = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Programação Orientada a Objetos", CargaHoraria = 55, Cursos = [curso.Id] });

        var body = new GradeIn {
            Nome = "Grade de ADS - 1.0",
            CursoId = curso.Id,
            Disciplinas = [
                new(bd.Id, 1, 10, 70),
                new(ed.Id, 2, 8, 55),
                new(poo.Id, 3, 12, 60),
            ],
        };

        // Act
        var grade = await client.PostAsync<GradeOut>("/grades", body);

        // Assert
        grade.Id.Should().NotBeEmpty();
        grade.Nome.Should().Be(body.Nome);
        grade.CursoId.Should().Be(curso.Id);
        grade.CursoNome.Should().Be(curso.Nome);
        grade.Disciplinas.Should().HaveCount(3);        
    }

    [Test]
    public async Task Deve_criar_uma_nova_grade_com_disciplinas_que_tem_carga_horaria_diferente_da_padrao()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" });

        var bd = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [curso.Id] });

        var body = new GradeIn {
            Nome = "Grade de ADS - 1.0",
            CursoId = curso.Id,
            Disciplinas = [
                new(bd.Id, 1, 10, 80),
            ],
        };

        // Act
        var grade = await client.PostAsync<GradeOut>("/grades", body);

        // Assert
        grade.Id.Should().NotBeEmpty();
        grade.Nome.Should().Be(body.Nome);
        grade.CursoId.Should().Be(curso.Id);
        grade.CursoNome.Should().Be(curso.Nome);
        grade.Disciplinas.Should().HaveCount(1);
        grade.Disciplinas[0].CargaHoraria.Should().Be(80);
    }

    [Test]
    public async Task Nao_deve_criar_uma_nova_grade_sem_vinculo_com_curso()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var body = new GradeIn { Nome = "Grade de ADS - 1.0" };

        // Act
        var response = await client.PostAsync("/grades", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0001);
    }

    [Test]
    public async Task Nao_deve_criar_uma_nova_grade_com_curso_que_nao_existe()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var body = new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = Guid.NewGuid(), };

        // Act
        var response = await client.PostAsync("/grades", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0001);
    }

    [Test]
    public async Task Nao_deve_criar_uma_nova_grade_com_curso_de_outra_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var novaRoma = await client.CreateFaculdade("Nova Roma");
        var userNovaRoma = UserIn.New(novaRoma.Id, Academico);
        await client.RegisterUser(userNovaRoma);

        var ufpe = await client.CreateFaculdade("UFPE");
        var userUfpe = UserIn.New(ufpe.Id, Academico);
        await client.RegisterUser(userUfpe);

        await client.Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = TipoDeCurso.Bacharelado };
        var curso = await client.PostAsync<CursoOut>("/cursos", bodyUfpe);

        await client.Login(userNovaRoma.Email, userNovaRoma.Password);

        var body = new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, };

        // Act
        var response = await client.PostAsync("/grades", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0001);
    }

    [Test]
    public async Task Nao_deve_criar_uma_grade_vinculando_disciplinas_que_nao_sao_do_curso_escolhido()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var ads = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" });
        var direito = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Direito" });

        var bd = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [ads.Id] });

        var body = new GradeIn {
            Nome = "Grade de Direito - 1.0",
            CursoId = direito.Id,
            Disciplinas = [ new(bd.Id, 1, 10, 70) ],
        };

        // Act
        var response = await client.PostAsync("/grades", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0002);      
    }

    [Test]
    public async Task Nao_deve_criar_uma_nova_grade_com_disciplina_de_outra_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var novaRoma = await client.CreateFaculdade("Nova Roma");
        var userNovaRoma = UserIn.New(novaRoma.Id, Academico);
        await client.RegisterUser(userNovaRoma);

        var ufpe = await client.CreateFaculdade("UFPE");
        var userUfpe = UserIn.New(ufpe.Id, Academico);
        await client.RegisterUser(userUfpe);

        await client.Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };
        var disciplinaUfpe = await client.PostAsync<DisciplinaOut>("/disciplinas", bodyUfpe);

        await client.Login(userNovaRoma.Email, userNovaRoma.Password);
        var bodyNovaRoma = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };
        var disciplinaNovaRoma = await client.PostAsync<DisciplinaOut>("/disciplinas", bodyNovaRoma);

        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" });

        var body = new GradeIn
        {
            Nome = "Grade de ADS - 1.0",
            CursoId = curso.Id,
            Disciplinas = [ new(disciplinaUfpe.Id, 1, 10, 70) ],
        };

        // Act
        var response = await client.PostAsync("/grades", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0002);
    }

    [Test]
    public async Task Nao_deve_criar_uma_grade_com_disciplinas_repetidas()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" });

        var bd = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [curso.Id] });
        var ed = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Estrutura de Dados", CargaHoraria = 60, Cursos = [curso.Id] });
        var poo = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Programação Orientada a Objetos", CargaHoraria = 55, Cursos = [curso.Id] });

        var body = new GradeIn {
            Nome = "Grade de Direito - 1.0",
            CursoId = curso.Id,
            Disciplinas = [
                new(bd.Id, 1, 10, 70),
                new(ed.Id, 2, 8, 55),
                new(ed.Id, 2, 8, 55),
                new(poo.Id, 3, 12, 60),
            ],
        };

        // Act
        var response = await client.PostAsync("/grades", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0002);      
    }

    [Test]
    public async Task Nao_deve_criar_uma_grade_com_disciplinas_que_nao_existem()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" });

        var bd = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [curso.Id] });
        var ed = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Estrutura de Dados", CargaHoraria = 60, Cursos = [curso.Id] });
        var poo = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Programação Orientada a Objetos", CargaHoraria = 55, Cursos = [curso.Id] });

        var body = new GradeIn {
            Nome = "Grade de Direito - 1.0",
            CursoId = curso.Id,
            Disciplinas = [
                new(bd.Id, 1, 10, 70),
                new(poo.Id, 3, 12, 60),
                new(Guid.NewGuid(), 2, 9, 55),
            ],
        };

        // Act
        var response = await client.PostAsync("/grades", body.ToStringContent());
        
        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0002);      
    }

    [Test]
    public async Task Deve_retornar_todas_as_grades_apenas_daquela_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var novaRoma = await client.CreateFaculdade("Nova Roma");
        var userNovaRoma = UserIn.New(novaRoma.Id, Academico);
        await client.RegisterUser(userNovaRoma);

        await client.Login(userNovaRoma.Email, userNovaRoma.Password);
        var cursoNovaRoma = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" });
        var bodyNovaRoma = new GradeIn {
            Nome = "NR - Grade de ADS - 1.0",
            CursoId = cursoNovaRoma.Id,
        };
        var gradeNovaRoma = await client.PostAsync<GradeOut>("/grades", bodyNovaRoma);

        var ufpe = await client.CreateFaculdade("UFPE");
        var userUfpe = UserIn.New(ufpe.Id, Academico);
        await client.RegisterUser(userUfpe);

        await client.Login(userUfpe.Email, userUfpe.Password);
        var cursoUfpe = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" });
        var bodyUfpe = new GradeIn {
            Nome = "UFPE - Grade de ADS - 1.0",
            CursoId = cursoUfpe.Id,
        };
        var gradeUfpe = await client.PostAsync<GradeOut>("/grades", bodyUfpe);

        // Act
        await client.Login(userNovaRoma.Email, userNovaRoma.Password);
        var grades = await client.GetAsync<List<GradeOut>>("/grades");

        // Assert
        grades.Should().HaveCount(1);
        grades[0].Id.Should().Be(gradeNovaRoma.Id);
        grades[0].Nome.Should().Be(gradeNovaRoma.Nome);
    }

    [Test]
    public async Task Deve_retornar_todas_as_grades_ordenadas_por_nome()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var direito = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Direito" });
        var gradeDireito = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de Direito - 1.0", CursoId = direito.Id, });

        var ads = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" });
        var gradeAds = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = ads.Id, });

        // Act
        var grades = await client.GetAsync<List<GradeOut>>("/grades");

        // Assert
        grades.Should().HaveCount(2);
        grades[0].Id.Should().Be(gradeAds.Id);
        grades[0].Nome.Should().Be(gradeAds.Nome);
        grades[1].Id.Should().Be(gradeDireito.Id);
        grades[1].Nome.Should().Be(gradeDireito.Nome);
    }
}
