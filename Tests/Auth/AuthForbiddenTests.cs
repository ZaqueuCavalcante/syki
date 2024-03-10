namespace Syki.Tests.Auth;

public partial class AuthForbiddenTests : AuthTestBase
{
    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_um_novo_aluno_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.PostAsync("/alunos", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAluno))]
    public async Task Nao_deve_retornar_as_disciplinas_do_aluno_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.GetAsync("/alunos/disciplinas");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_os_alunos_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.GetAsync("/alunos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_um_novo_campus_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.PostAsync("/campi", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_editar_um_campus_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.PutAsync("/campi", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_os_campus_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.GetAsync("/campi");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_um_novo_curso_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.PostAsync("/cursos", null);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_os_cursos_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.GetAsync("/cursos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_uma_nova_disciplina_quando_o_usuario_nao_esta_logado(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.PostAsync("/disciplinas", null);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_as_disciplinas_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.GetAsync("/disciplinas");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAdm))]
    public async Task Nao_deve_criar_uma_nova_faculdade_quando_o_usuario_nao_esta_logado(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.PostAsync("/faculdades", null);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAdm))]
    public async Task Nao_deve_retornar_as_faculdades_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.GetAsync("/faculdades");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_uma_nova_grade_quando_o_usuario_nao_esta_logado(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.PostAsync("/grades", null);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_as_grades_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);
        
        // Act
        var response = await _client.GetAsync("/grades");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAdm))]
    public async Task Nao_deve_retornar_os_dados_de_index_adm_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.GetAsync("/index/adm");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_os_dados_de_index_academico_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.GetAsync("/index/academico");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAluno))]
    public async Task Nao_deve_retornar_os_dados_de_index_aluno_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.GetAsync("/index/aluno");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_uma_nova_notificacao_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.PostAsync("/notifications", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_todas_as_notificacoes_da_faculdade_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.GetAsync("/notifications");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_uma_nova_oferta_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.PostAsync("/ofertas", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_as_ofertas_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.GetAsync("/ofertas");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_um_novo_periodo_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.PostAsync("/periodos", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_os_periodos_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.GetAsync("/periodos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_um_novo_professor_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.PostAsync("/professores", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_os_professores_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.GetAsync("/professores");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_uma_nova_turma_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.PostAsync("/turmas", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_as_turmas_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.GetAsync("/turmas");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAdm))]
    public async Task Nao_deve_criar_um_novo_usuario_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.PostAsync("/users", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAdm))]
    public async Task Nao_deve_retornar_os_usuarios_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.GetAsync("/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
