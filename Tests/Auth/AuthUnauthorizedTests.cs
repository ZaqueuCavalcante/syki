using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;

namespace Syki.Tests.Auth;

public class AuthUnauthorizedTests : AuthTestBase
{
    [Test]
    public async Task Nao_deve_criar_um_novo_aluno_quando_o_usuario_esta_deslogado()
    {
        // Arrange
        var body = new AlunoIn { Nome = "Zaqueu" };

        // Act
        var response = await _client.PostAsync("/alunos", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_as_disciplinas_do_aluno_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/alunos/disciplinas");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_os_alunos_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/alunos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_criar_um_novo_campus_quando_o_usuario_esta_deslogado()
    {
        // Arrange
        var body = new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" };

        // Act
        var response = await _client.PostAsync("/campi", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_editar_um_campus_quando_o_usuario_esta_deslogado()
    {
        // Arrange
        var body = new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" };

        // Act
        var response = await _client.PutAsync("/campi", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_os_campus_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/campi");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_criar_um_novo_curso_quando_o_usuario_esta_deslogado()
    {
        // Arrange
        var body = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" };

        // Act
        var response = await _client.PostAsync("/cursos", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_os_cursos_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/cursos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_criar_uma_nova_disciplina_quando_o_usuario_nao_esta_logado()
    {
        // Arrange
        var body = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };

        // Act
        var response = await _client.PostAsync("/disciplinas", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_as_disciplinas_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/disciplinas");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_criar_uma_nova_faculdade_quando_o_usuario_nao_esta_logado()
    {
        // Arrange
        var body = new FaculdadeIn { Nome = "Nova Roma" };

        // Act
        var response = await _client.PostAsync("/faculdades", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_as_faculdades_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/faculdades");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_criar_uma_nova_grade_quando_o_usuario_nao_esta_logado()
    {
        // Arrange
        var body = new GradeIn { Nome = "Grade ADS - 1.0" };

        // Act
        var response = await _client.PostAsync("/grades", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_as_grades_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/grades");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_os_dados_de_index_adm_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/index/adm");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_os_dados_de_index_academico_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/index/academico");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_os_dados_de_index_aluno_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/index/aluno");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_criar_um_novo_livro_quando_o_usuario_esta_deslogado()
    {
        // Arrange
        var body = new LivroIn { Titulo = "Manual de DevOps" };

        // Act
        var response = await _client.PostAsync("/livros", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_os_livros_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/livros");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_criar_uma_nova_notificacao_quando_o_usuario_esta_deslogado()
    {
        // Arrange
        var body = new NotificationIn { Title = "Hello", Description = "Hi", UsersGroup = "Alunos" };

        // Act
        var response = await _client.PostAsync("/notifications", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_marcar_uma_notificacao_como_vista_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.PutAsync("/notifications/user", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_todas_as_notificacoes_da_faculdade_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/notifications");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_as_notificacoes_do_usuario_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/notifications/user");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_criar_uma_nova_oferta_quando_o_usuario_esta_deslogado()
    {
        // Arrange
        var body = new OfertaIn { };

        // Act
        var response = await _client.PostAsync("/ofertas", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_as_ofertas_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/ofertas");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_criar_um_novo_periodo_quando_o_usuario_esta_deslogado()
    {
        // Arrange
        var body = new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) };

        // Act
        var response = await _client.PostAsync("/periodos", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_os_periodos_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/periodos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_criar_um_novo_professor_quando_o_usuario_esta_deslogado()
    {
        // Arrange
        var body = new ProfessorIn { Nome = "Chico" };

        // Act
        var response = await _client.PostAsync("/professores", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_os_professores_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/professores");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_criar_uma_nova_turma_quando_o_usuario_esta_deslogado()
    {
        // Arrange
        var body = new TurmaIn { Periodo = "2024.1" };

        // Act
        var response = await _client.PostAsync("/turmas", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_as_turmas_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/turmas");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_criar_um_novo_usuario_quando_o_usuario_esta_deslogado()
    {
        // Arrange
        var body = new UserIn { Name = "Zaqueu" };

        // Act
        var response = await _client.PostAsync("/users", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_a_mfa_key_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/users/mfa-key");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_realizar_o_setup_da_mfa__quando_o_usuario_esta_deslogado()
    {
        // Arrange
        var body = new MfaSetupIn { Token = "651681" };

        // Act
        var response = await _client.PostAsync("/users/mfa-setup", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_os_usuarios_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
