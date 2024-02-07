using System.Net;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;

namespace Syki.Tests.Auth;

public partial class AuthUnauthorizedTests : AuthTestBase
{
    [Test]
    public async Task Nao_deve_criar_um_novo_periodo_de_matricula_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.PostAsync("/matriculas/periodos", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_os_periodos_de_matricula_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/matriculas/periodos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Nao_deve_retornar_as_opcoes_de_turmas_para_matricula_quando_o_usuario_esta_deslogado()
    {
        // Arrange / Act
        var response = await _client.GetAsync("/matriculas/turmas");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
