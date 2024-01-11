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

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_a_notification()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
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
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) });
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });

        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id });

        var bodyAluno = new AlunoIn { Nome = "Zaqueu", Email = "zaqueu4@aluno.com", OfertaId = oferta.Id };
        var aluno = await client.PostAsync<AlunoOut>("/alunos", bodyAluno);

        var body = new NotificationIn { Title = "Hello", Description = "Hi", UsersGroup = "Alunos" };
        await client.PostAsync<NotificationOut>("/notifications", body);

        // Act
        var password = await _factory.ResetPassword(aluno.UserId);
        await client.Login(bodyAluno.Email, password);
        var response = await client.PutAsync("/notifications/user", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        using var scope = _factory.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();
        var notification = await ctx.UserNotifications.FirstAsync(n => n.UserId == aluno.UserId);
        notification.ViewedAt.Should().NotBeNull();
    }

    [Test]
    public async Task Deve_retornar_todas_as_notificacoes()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
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
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) });
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });

        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id });

        var bodyAluno = new AlunoIn { Nome = "Zaqueu", Email = "zaqueu5@aluno.com", OfertaId = oferta.Id };
        var aluno = await client.PostAsync<AlunoOut>("/alunos", bodyAluno);

        var body = new NotificationIn { Title = "Hello", Description = "Hi", UsersGroup = "Alunos" };
        await client.PostAsync<NotificationOut>("/notifications", body);

        // Act
        var password = await _factory.ResetPassword(aluno.UserId);
        await client.Login(bodyAluno.Email, password);
        var response = await client.GetAsync<List<UserNotificationOut>>("/notifications/user");

        // Assert
        response.Count.Should().Be(1);
    }
}
