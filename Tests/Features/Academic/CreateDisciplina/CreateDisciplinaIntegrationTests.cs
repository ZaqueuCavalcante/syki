namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_uma_nova_disciplina_sem_vinculo_com_nenhum_curso()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        var disciplina = await client.CreateDisciplina("Banco de Dados");

        // Assert
        disciplina.Id.Should().NotBeEmpty();
        disciplina.Name.Should().Be("Banco de Dados");
        disciplina.Cursos.Should().BeEquivalentTo(new List<Guid>());
    }

    [Test]
    public async Task Deve_criar_uma_nova_disciplina_vincula_a_um_unico_curso()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        var curso = await client.CreateCurso("Análise e Desenvolvimento de Sistemas");

        // Act
        var disciplina = await client.CreateDisciplina("Banco de Dados", [curso.Id]);

        // Assert
        disciplina.Name.Should().Be("Banco de Dados");
        disciplina.Cursos.Should().BeEquivalentTo([curso.Id]);
    }

    [Test]
    public async Task Deve_criar_uma_nova_disciplina_vincula_a_mais_de_um_curso()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        var ads = await client.CreateCurso("Análise e Desenvolvimento de Sistemas");
        var cc = await client.CreateCurso("Ciência da Computação");

        // Act
        var disciplina = await client.CreateDisciplina("Banco de Dados", [ads.Id, cc.Id]);

        // Assert
        disciplina.Name.Should().Be("Banco de Dados");
        disciplina.Cursos.Should().BeEquivalentTo([cc.Id, ads.Id]);
    }

    [Test]
    public async Task Deve_criar_varias_disciplinas()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        await client.CreateDisciplina("Banco de Dados");
        await client.CreateDisciplina("Estrutura de Dados");
        await client.CreateDisciplina("Programação Orientada a Objetos");

        // Assert
        var disciplinas = await client.GetAsync<List<DisciplinaOut>>("/disciplinas");
        disciplinas.Should().HaveCount(3);
    }
}
