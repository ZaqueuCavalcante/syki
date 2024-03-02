using Syki.Shared;
using Syki.Shared.CreateCampus;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_a_notification()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var body = new NotificationIn { Title = "Hello", Description = "Hi", UsersGroup = "Alunos" };

        // Act
        var response = await client.PostAsync<NotificationOut>("/notifications", body);

        // Assert
        response.Id.Should().NotBeEmpty();
        response.Title.Should().Be(body.Title);
        response.Description.Should().Be(body.Description);
    }

    [Test]
    public async Task Deve_marcar_a_notificacao_como_vista_pelo_usuario()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.NewCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });

        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id });

        var bodyAluno = new AlunoIn { Nome = "Zaqueu", Email = TestData.Email, OfertaId = oferta.Id };
        var aluno = await client.PostAsync<AlunoOut>("/alunos", bodyAluno);

        var body = new NotificationIn { Title = "Hello", Description = "Hi", UsersGroup = "Alunos" };
        await client.PostAsync<NotificationOut>("/notifications", body);

        // Act
        var token = await _factory.GetResetPasswordToken(aluno.Id);
        var password = await client.ResetPassword(token!);
        await client.Login(bodyAluno.Email, password);
        var response = await client.PutAsync("/notifications/user", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        using var ctx = _factory.GetDbContext();
        var notification = await ctx.UserNotifications.FirstAsync(n => n.UserId == aluno.Id);
        notification.ViewedAt.Should().NotBeNull();
    }

    [Test]
    public async Task Deve_retornar_todas_as_notificacoes()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        await client.PostAsync<NotificationOut>("/notifications", new NotificationIn { Title = "Hello", Description = "Hi", UsersGroup = "Alunos" });
        await client.PostAsync<NotificationOut>("/notifications", new NotificationIn { Title = "Ola", Description = "O", UsersGroup = "Alunos" });

        // Act
        var notifications = await client.GetAsync<List<NotificationOut>>("/notifications");

        // Assert
        notifications.Count.Should().Be(2);
    }

    [Test]
    public async Task Deve_retornar_todas_as_notificacoes_do_usuario_logado()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.NewCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });

        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id });

        var bodyAluno = new AlunoIn { Nome = "Zaqueu", Email = TestData.Email, OfertaId = oferta.Id };
        var aluno = await client.PostAsync<AlunoOut>("/alunos", bodyAluno);

        var body = new NotificationIn { Title = "Hello", Description = "Hi", UsersGroup = "Alunos" };
        await client.PostAsync<NotificationOut>("/notifications", body);

        // Act
        var token = await _factory.GetResetPasswordToken(aluno.Id);
        var password = await client.ResetPassword(token!);
        await client.Login(bodyAluno.Email, password);
        var response = await client.GetAsync<List<UserNotificationOut>>("/notifications/user");

        // Assert
        response.Count.Should().Be(1);
    }
}
