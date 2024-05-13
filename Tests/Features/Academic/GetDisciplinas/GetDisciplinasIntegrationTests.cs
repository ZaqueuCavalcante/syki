namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test, Ignore("")]
    public async Task Deve_retornar_as_disciplines_ordenadas_pelo_nome()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        await client.CreateDiscipline("Estrutura de Dados");
        await client.CreateDiscipline("Banco de Dados");
        await client.CreateDiscipline("Programação Orientada a Objetos");
        await client.CreateDiscipline("Informática e Sociedade");
        await client.CreateDiscipline("Projeto Integrador II: Modelagem de Banco de Dados");
        await client.CreateDiscipline("Arquitetura de Computadores e Sistemas Operacionais");

        // Assert
        var disciplines = await client.GetAsync<List<DisciplineOut>>("/disciplines");
        disciplines.Should().HaveCount(6);
        disciplines[0].Name.Should().Be("Arquitetura de Computadores e Sistemas Operacionais");
        disciplines[1].Name.Should().Be("Banco de Dados");
        disciplines[2].Name.Should().Be("Estrutura de Dados");
        disciplines[3].Name.Should().Be("Informática e Sociedade");
        disciplines[4].Name.Should().Be("Programação Orientada a Objetos");
        disciplines[5].Name.Should().Be("Projeto Integrador II: Modelagem de Banco de Dados");
    }

    [Test, Ignore("")]
    public async Task Deve_retornar_apenas_as_disciplines_do_curso_informado()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        var ads = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");
        var cc = await client.CreateCourse("Ciência da Computação");

        await client.CreateDiscipline("Banco de Dados", [ads.Id, cc.Id]);
        await client.CreateDiscipline("Informática e Sociedade", [ads.Id]);
        await client.CreateDiscipline("Programação Orientada a Objetos", [cc.Id]);
        await client.CreateDiscipline("Projeto Integrador II: Modelagem de Banco de Dados", [cc.Id]);

        // Act
        var disciplines = await client.GetAsync<List<DisciplineOut>>($"/disciplines?cursoId={ads.Id}");

        // Assert
        disciplines.Should().HaveCount(2);
        disciplines[0].Name.Should().Be("Banco de Dados");
        disciplines[1].Name.Should().Be("Informática e Sociedade");
    }
}
