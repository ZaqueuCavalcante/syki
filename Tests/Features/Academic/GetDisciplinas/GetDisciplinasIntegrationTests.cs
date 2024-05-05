namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_retornar_as_disciplinas_ordenadas_pelo_nome()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        await client.CreateDisciplina("Estrutura de Dados");
        await client.CreateDisciplina("Banco de Dados");
        await client.CreateDisciplina("Programação Orientada a Objetos");
        await client.CreateDisciplina("Informática e Sociedade");
        await client.CreateDisciplina("Projeto Integrador II: Modelagem de Banco de Dados");
        await client.CreateDisciplina("Arquitetura de Computadores e Sistemas Operacionais");

        // Assert
        var disciplinas = await client.GetAsync<List<DisciplinaOut>>("/disciplinas");
        disciplinas.Should().HaveCount(6);
        disciplinas[0].Name.Should().Be("Arquitetura de Computadores e Sistemas Operacionais");
        disciplinas[1].Name.Should().Be("Banco de Dados");
        disciplinas[2].Name.Should().Be("Estrutura de Dados");
        disciplinas[3].Name.Should().Be("Informática e Sociedade");
        disciplinas[4].Name.Should().Be("Programação Orientada a Objetos");
        disciplinas[5].Name.Should().Be("Projeto Integrador II: Modelagem de Banco de Dados");
    }

    [Test]
    public async Task Deve_retornar_apenas_as_disciplinas_do_curso_informado()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var ads = await client.CreateCurso("Análise e Desenvolvimento de Sistemas");
        var cc = await client.CreateCurso("Ciência da Computação");

        await client.CreateDisciplina("Banco de Dados", [ads.Id, cc.Id]);
        await client.CreateDisciplina("Informática e Sociedade", [ads.Id]);
        await client.CreateDisciplina("Programação Orientada a Objetos", [cc.Id]);
        await client.CreateDisciplina("Projeto Integrador II: Modelagem de Banco de Dados", [cc.Id]);

        // Act
        var disciplinas = await client.GetAsync<List<DisciplinaOut>>($"/disciplinas?cursoId={ads.Id}");

        // Assert
        disciplinas.Should().HaveCount(2);
        disciplinas[0].Name.Should().Be("Banco de Dados");
        disciplinas[1].Name.Should().Be("Informática e Sociedade");
    }
}
