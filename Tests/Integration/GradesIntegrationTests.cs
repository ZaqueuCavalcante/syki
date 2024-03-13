using Syki.Shared;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    // [Test]
    public async Task Deve_criar_uma_nova_grade_mesmo_sem_disciplinas()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var curso = await client.CreateCurso("Análise e Desenvolvimento de Sistemas");

        // Act
        var grade = await client.NewGrade("Grade de ADS 1.0", curso.Id);

        // Assert
        grade.Id.Should().NotBeEmpty();
        grade.Nome.Should().Be("Grade de ADS 1.0");
        grade.CursoId.Should().Be(curso.Id);
        grade.CursoNome.Should().Be(curso.Nome);
        grade.Disciplinas.Should().HaveCount(0);        
    }

    // [Test]
    public async Task Deve_criar_uma_nova_grade_com_disciplinas()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var curso = await client.CreateCurso("Análise e Desenvolvimento de Sistemas");

        var bd = await client.NewDisciplina("Banco de Dados", [curso.Id]);
        var ed = await client.NewDisciplina("Estrutura de Dados", [curso.Id]);
        var poo = await client.NewDisciplina("Programação Orientada a Objetos", [curso.Id]);
        var disciplinas = new List<GradeDisciplinaIn> { new(bd.Id, 1, 10, 70), new(ed.Id, 2, 8, 55), new(poo.Id, 3, 12, 60) };

        // Act
        var grade = await client.NewGrade("Grade de ADS 1.0", curso.Id, disciplinas);

        // Assert
        grade.Id.Should().NotBeEmpty();
        grade.Nome.Should().Be("Grade de ADS 1.0");
        grade.CursoId.Should().Be(curso.Id);
        grade.CursoNome.Should().Be(curso.Nome);
        grade.Disciplinas.Should().HaveCount(3);        
    }

    // [Test]
    public async Task Deve_criar_uma_nova_grade_com_disciplinas_que_tem_carga_horaria_diferente_da_padrao()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var curso = await client.CreateCurso("Análise e Desenvolvimento de Sistemas");

        var bd = await client.NewDisciplina("Banco de Dados", [curso.Id]);
        var disciplinas = new List<GradeDisciplinaIn> { new(bd.Id, 1, 10, 80) };

        // Act
        var grade = await client.NewGrade("Grade de ADS 1.0", curso.Id, disciplinas);

        // Assert
        grade.Id.Should().NotBeEmpty();
        grade.Nome.Should().Be("Grade de ADS 1.0");
        grade.CursoId.Should().Be(curso.Id);
        grade.CursoNome.Should().Be(curso.Nome);
        grade.Disciplinas.Should().HaveCount(1);
        grade.Disciplinas[0].CargaHoraria.Should().Be(80);
    }

    // [Test]
    public async Task Nao_deve_criar_uma_nova_grade_sem_vinculo_com_curso()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var body = new GradeIn { Nome = "Grade de ADS 1.0" };

        // Act
        var response = await client.PostHttpAsync("/grades", body);
        
        // Assert
        await response.AssertBadRequest(Throw.DE002);
    }

    // [Test]
    public async Task Nao_deve_criar_uma_nova_grade_com_curso_que_nao_existe()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var body = new GradeIn { Nome = "Grade de ADS 1.0", CursoId = Guid.NewGuid() };

        // Act
        var response = await client.PostHttpAsync("/grades", body);

        // Assert
        await response.AssertBadRequest(Throw.DE002);
    }

    // [Test]
    public async Task Nao_deve_criar_uma_nova_grade_com_curso_de_outra_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var userNovaRoma = await client.NewAcademico("Nova Roma");
        var userUfpe = await client.NewAcademico("UFPE");

        await client.Login(userUfpe);
        var curso = await client.CreateCurso("Análise e Desenvolvimento de Sistemas");

        await client.Login(userNovaRoma);
        var body = new GradeIn { Nome = "Grade de ADS 1.0", CursoId = curso.Id };

        // Act
        var response = await client.PostHttpAsync("/grades", body);
        
        // Assert
        await response.AssertBadRequest(Throw.DE002);
    }

    // [Test]
    public async Task Nao_deve_criar_uma_grade_vinculando_disciplinas_que_nao_sao_do_curso_escolhido()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        var ads = await client.CreateCurso("Análise e Desenvolvimento de Sistemas");
        var direito = await client.CreateCurso("Direito");

        var bd = await client.NewDisciplina("Banco de Dados", [ads.Id]);

        var body = new GradeIn {
            Nome = "Grade de Direito 1.0",
            CursoId = direito.Id,
            Disciplinas = [ new(bd.Id, 1, 10, 70) ],
        };

        // Act
        var response = await client.PostHttpAsync("/grades", body);

        // Assert
        await response.AssertBadRequest(Throw.DE003);
    }

    // [Test]
    public async Task Nao_deve_criar_uma_nova_grade_com_disciplina_de_outra_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var userNovaRoma = await client.NewAcademico("Nova Roma");
        var userUfpe = await client.NewAcademico("UFPE");

        await client.Login(userNovaRoma);
        await client.NewDisciplina("Banco de Dados");

        await client.Login(userUfpe);
        var disciplinaUfpe = await client.NewDisciplina("Banco de Dados");

        var curso = await client.CreateCurso("Análise e Desenvolvimento de Sistemas");

        var body = new GradeIn
        {
            Nome = "Grade de ADS 1.0",
            CursoId = curso.Id,
            Disciplinas = [ new(disciplinaUfpe.Id, 1, 10, 70) ],
        };

        // Act
        var response = await client.PostHttpAsync("/grades", body);

        // Assert
        await response.AssertBadRequest(Throw.DE003);
    }

    // [Test]
    public async Task Nao_deve_criar_uma_grade_com_disciplinas_repetidas()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var curso = await client.CreateCurso("Análise e Desenvolvimento de Sistemas");

        var bd = await client.NewDisciplina("Banco de Dados", [curso.Id]);
        var ed = await client.NewDisciplina("Estrutura de Dados", [curso.Id]);
        var poo = await client.NewDisciplina("Programação Orientada a Objetos", [curso.Id]);

        var body = new GradeIn {
            Nome = "Grade de Direito 1.0",
            CursoId = curso.Id,
            Disciplinas = [
                new(bd.Id, 1, 10, 70),
                new(ed.Id, 2, 8, 55),
                new(ed.Id, 2, 8, 55),
                new(poo.Id, 3, 12, 60),
            ],
        };

        // Act
        var response = await client.PostHttpAsync("/grades", body);

        // Assert
        await response.AssertBadRequest(Throw.DE003);
    }

    // [Test]
    public async Task Nao_deve_criar_uma_grade_com_disciplinas_que_nao_existem()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var curso = await client.CreateCurso("Análise e Desenvolvimento de Sistemas");

        var bd = await client.NewDisciplina("Banco de Dados", [curso.Id]);
        var poo = await client.NewDisciplina("Programação Orientada a Objetos", [curso.Id]);
        var disciplinas = new List<GradeDisciplinaIn> { new(bd.Id, 1, 10, 70), new(poo.Id, 3, 12, 60), new(Guid.NewGuid(), 2, 9, 55) };

        var body = new GradeIn { Nome = "Grade de Direito 1.0", CursoId = curso.Id, Disciplinas = disciplinas };

        // Act
        var response = await client.PostHttpAsync("/grades", body);

        // Assert
        await response.AssertBadRequest(Throw.DE003);     
    }

    // [Test]
    public async Task Deve_retornar_todas_as_grades_apenas_daquela_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var userNovaRoma = await client.NewAcademico("Nova Roma");
        var userUfpe = await client.NewAcademico("UFPE");

        await client.Login(userNovaRoma);
        var cursoNovaRoma = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" });
        var bodyNovaRoma = new GradeIn {
            Nome = "NR - Grade de ADS - 1.0",
            CursoId = cursoNovaRoma.Id,
        };
        var gradeNovaRoma = await client.PostAsync<GradeOut>("/grades", bodyNovaRoma);

        await client.Login(userUfpe);
        var cursoUfpe = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" });
        var bodyUfpe = new GradeIn {
            Nome = "UFPE - Grade de ADS - 1.0",
            CursoId = cursoUfpe.Id,
        };
        await client.PostAsync<GradeOut>("/grades", bodyUfpe);

        // Act
        await client.Login(userNovaRoma);
        var grades = await client.GetAsync<List<GradeOut>>("/grades");

        // Assert
        grades.Should().HaveCount(1);
        grades[0].Id.Should().Be(gradeNovaRoma.Id);
        grades[0].Nome.Should().Be(gradeNovaRoma.Nome);
    }

    // [Test]
    public async Task Deve_retornar_todas_as_grades_ordenadas_por_nome()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        var direito = await client.CreateCurso("Direito");
        var gradeDireito = await client.NewGrade("Grade de Direito 1.0", direito.Id);

        var ads = await client.CreateCurso("Análise e Desenvolvimento de Sistemas");
        var gradeAds = await client.NewGrade("Grade de ADS 1.0", ads.Id);

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
