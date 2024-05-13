namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_uma_nova_discipline_sem_vinculo_com_nenhum_curso()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        var discipline = await client.CreateDiscipline("Banco de Dados");

        // Assert
        discipline.Id.Should().NotBeEmpty();
        discipline.Name.Should().Be("Banco de Dados");
        discipline.Courses.Should().BeEquivalentTo(new List<Guid>());
    }

    [Test]
    public async Task Deve_criar_uma_nova_discipline_vincula_a_um_unico_curso()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        var curso = await client.CreateCurso("Análise e Desenvolvimento de Sistemas");

        // Act
        var discipline = await client.CreateDiscipline("Banco de Dados", [curso.Id]);

        // Assert
        discipline.Name.Should().Be("Banco de Dados");
        discipline.Courses.Should().BeEquivalentTo([curso.Id]);
    }

    [Test]
    public async Task Deve_criar_uma_nova_discipline_vincula_a_mais_de_um_curso()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        var ads = await client.CreateCurso("Análise e Desenvolvimento de Sistemas");
        var cc = await client.CreateCurso("Ciência da Computação");

        // Act
        var discipline = await client.CreateDiscipline("Banco de Dados", [ads.Id, cc.Id]);

        // Assert
        discipline.Name.Should().Be("Banco de Dados");
        discipline.Courses.Should().BeEquivalentTo([cc.Id, ads.Id]);
    }

    [Test]
    public async Task Deve_criar_varias_disciplines()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        await client.CreateDiscipline("Banco de Dados");
        await client.CreateDiscipline("Estrutura de Dados");
        await client.CreateDiscipline("Programação Orientada a Objetos");

        // Assert
        var disciplines = await client.GetAsync<List<DisciplineOut>>("/disciplines");
        disciplines.Should().HaveCount(3);
    }
}
