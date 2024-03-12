using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[Parallelizable(ParallelScope.All)]
public partial class IntegrationTests : IntegrationTestBase
{
    // [Test]
    public async Task Deve_criar_um_aluno_o_vinculando_com_uma_oferta()
    {
        // Arrange
        var client = _factory.GetClient();
        var faculdade = await client.CreateInstitution();
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });
        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id, });

        var body = new AlunoIn { Nome = "Zaqueu", Email = TestData.Email, OfertaId = oferta.Id };

        // Act
        var response = await client.PostAsync<AlunoOut>("/alunos", body);

        // Assert
        response.Id.Should().NotBeEmpty(); 
        response.OfertaId.Should().Be(oferta.Id); 
        response.Nome.Should().Be("Zaqueu"); 
    }

    // [Test]
    public async Task Nao_deve_criar_um_aluno_nem_seu_usuario_quando_der_erro()
    {
        // Arrange
        var client = _factory.GetClient();
        var faculdade = await client.CreateInstitution();
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });
        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id, });

        var body = new AlunoIn { Nome = "ZA", Email = TestData.Email, OfertaId = oferta.Id };

        // Act
        var response = await client.PostHttpAsync("/alunos", body);

        await client.LoginAsAdm();
        var users = await client.GetAsync<List<CreateUserOut>>("/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        users.FirstOrDefault(u => u.Email == body.Email).Should().BeNull();
    }

    // [Test]
    public async Task Nao_deve_criar_um_aluno_sem_vinculo_com_oferta()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var body = new AlunoIn { Nome = "Zaqueu", Email = TestData.Email, OfertaId = Guid.NewGuid() };

        // Act
        var response = await client.PostHttpAsync("/alunos", body);

        // Assert
        await response.AssertBadRequest(Throw.DE012);
    }

    // [Test]
    public async Task Deve_retornar_as_disciplinas_cursadas_pelo_aluno()
    {
        // Arrange
        var client = _factory.GetClient();
        var faculdade = await client.CreateInstitution();
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });

        var disciplina01 = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", Cursos = [curso.Id] });
        var disciplina02 = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Estrutura de Dados", Cursos = [curso.Id] });
        var disciplina03 = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Programação Orientada a Objetos", Cursos = [curso.Id] });
        var disciplinas = new List<GradeDisciplinaIn>() { new() { Id = disciplina01.Id }, new() { Id = disciplina02.Id }, new() { Id = disciplina03.Id } };

        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, Disciplinas = disciplinas });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id });

        var body = new AlunoIn { Nome = "Zaqueu", Email = TestData.Email, OfertaId = oferta.Id };
        var aluno = await client.PostAsync<AlunoOut>("/alunos", body);

        var token = await _factory.GetResetPasswordToken(aluno.Email);
        var password = await client.ResetPassword(token!);
        await client.Login(body.Email, password);

        // Act
        var response = await client.GetAsync<List<DisciplinaOut>>("/alunos/disciplinas");

        // Assert
        response.Count.Should().Be(3); 
    }

    // [Test]
    public async Task Deve_retornar_os_alunos()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });
        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id, });

        await client.PostAsync("/alunos", new AlunoIn { Nome = "Zaqueu", Email = TestData.Email, OfertaId = oferta.Id });
        await client.PostAsync("/alunos", new AlunoIn { Nome = "Maju", Email = TestData.Email, OfertaId = oferta.Id });

        // Act
        var response = await client.GetAsync<List<AlunoOut>>("/alunos");

        // Assert
        response.Count.Should().Be(2); 
    }
}
