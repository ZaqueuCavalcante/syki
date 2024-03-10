namespace Syki.Tests.Auth;

public partial class AuthForbiddenTests : AuthTestBase
{
    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_um_novo_periodo_de_matricula_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.PostAsync("/enrollment-periods", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_os_periodos_de_matricula_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.GetAsync("/enrollment-periods");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // [Test]
    // [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAluno))]
    public async Task Nao_deve_retornar_as_opcoes_de_turmas_para_matricula_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.GetAsync("/matriculas/turmas");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
