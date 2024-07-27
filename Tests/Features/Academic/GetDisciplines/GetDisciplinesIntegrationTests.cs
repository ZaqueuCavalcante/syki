namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_disciplines_ordered_by_name()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        await client.CreateDiscipline("Estrutura de Dados");
        await client.CreateDiscipline("Banco de Dados");
        await client.CreateDiscipline("Programação Orientada a Objetos");
        await client.CreateDiscipline("Informática e Sociedade");
        await client.CreateDiscipline("Projeto Integrador II: Modelagem de Banco de Dados");
        await client.CreateDiscipline("Arquitetura de Computadores e Sistemas Operacionais");

        // Assert
        var disciplines = await client.GetDisciplines();
        disciplines.Should().HaveCount(6);
        disciplines[0].Name.Should().Be("Arquitetura de Computadores e Sistemas Operacionais");
        disciplines[1].Name.Should().Be("Banco de Dados");
        disciplines[2].Name.Should().Be("Estrutura de Dados");
        disciplines[3].Name.Should().Be("Informática e Sociedade");
        disciplines[4].Name.Should().Be("Programação Orientada a Objetos");
        disciplines[5].Name.Should().Be("Projeto Integrador II: Modelagem de Banco de Dados");
    }
}
