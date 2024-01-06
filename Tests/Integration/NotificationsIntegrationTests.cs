using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class NotificationsIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_a_notification()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, Academico);

        var body = new NotificationIn { Title = "Hello", Description = "Hi", UsersGroup = "Alunos" };

        // Act
        var response = await PostAsync<NotificationOut>("/notifications", body);

        // Assert
        response.Id.Should().NotBeEmpty();
        response.Title.Should().Be(body.Title);
        response.Description.Should().Be(body.Description);
    }

    [Test]
    public async Task Deve_marcar_a_notificacao_como_vista_pelo_usuario()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var userBody = UserIn.New(faculdade.Id, Aluno);
        var userAluno = await PostAsync<UserOut>("/users", userBody);
        await RegisterAndLogin(faculdade.Id, Academico);

        var campus = await PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) });
        var curso = await PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });

        var grade = await PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id });
        var oferta = await PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id });
        var aluno = await PostAsync<AlunoOut>("/alunos", new AlunoIn { Nome = "Zaqueu", OfertaId = oferta.Id });

        // GAMBIARRA
        using var scope = _factory.Services.CreateScope();
        _ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();
        var alunoDb = await _ctx.Alunos.FirstAsync(a => a.Id == aluno.Id);
        alunoDb.UserId = userAluno.Id;
        await _ctx.SaveChangesAsync();
        // GAMBIARRA

        var body = new NotificationIn { Title = "Hello", Description = "Hi", UsersGroup = "Alunos" };
        await PostAsync<NotificationOut>("/notifications", body);

        // Act
        await Login(userBody.Email, userBody.Password);
        var response = await _client.PutAsync("/notifications/user", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var notification = await _ctx.UserNotifications.FirstAsync(n => n.UserId == userAluno.Id);
        notification.ViewedAt.Should().NotBeNull();
    }

    [Test]
    public async Task Deve_retornar_todas_as_notificacoes()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, Academico);

        await PostAsync<NotificationOut>("/notifications", new NotificationIn { Title = "Hello", Description = "Hi", UsersGroup = "Alunos" });
        await PostAsync<NotificationOut>("/notifications", new NotificationIn { Title = "Ola", Description = "O", UsersGroup = "Alunos" });

        // Act
        var notifications = await GetAsync<List<NotificationOut>>("/notifications");

        // Assert
        notifications.Count.Should().Be(2);
    }

    [Test]
    public async Task Deve_retornar_todas_as_notificacoes_do_usuario_logado()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var userBody = UserIn.New(faculdade.Id, Aluno);
        var userAluno = await PostAsync<UserOut>("/users", userBody);
        await RegisterAndLogin(faculdade.Id, Academico);

        var campus = await PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) });
        var curso = await PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });

        var grade = await PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id });
        var oferta = await PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id });
        var aluno = await PostAsync<AlunoOut>("/alunos", new AlunoIn { Nome = "Zaqueu", OfertaId = oferta.Id });

        // GAMBIARRA
        using var scope = _factory.Services.CreateScope();
        _ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();
        var alunoDb = await _ctx.Alunos.FirstAsync(a => a.Id == aluno.Id);
        alunoDb.UserId = userAluno.Id;
        await _ctx.SaveChangesAsync();
        // GAMBIARRA

        var body = new NotificationIn { Title = "Hello", Description = "Hi", UsersGroup = "Alunos" };
        await PostAsync<NotificationOut>("/notifications", body);

        // Act
        await Login(userBody.Email, userBody.Password);
        var response = await GetAsync<List<UserNotificationOut>>("/notifications/user");

        // Assert
        response.Count.Should().Be(1);
    }
}
