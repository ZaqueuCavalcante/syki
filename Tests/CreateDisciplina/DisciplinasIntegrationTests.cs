namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_uma_nova_disciplina_sem_vinculo_com_nenhum_curso()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        var disciplina = await client.CreateDisciplina("Banco de Dados");

        // Assert
        disciplina.Id.Should().NotBeEmpty();
        disciplina.Nome.Should().Be("Banco de Dados");
        disciplina.Cursos.Should().BeEquivalentTo(new List<Guid>());
    }

    [Test]
    public async Task Deve_criar_uma_nova_disciplina_vincula_a_um_unico_curso()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var curso = await client.CreateCurso("Análise e Desenvolvimento de Sistemas");

        // Act
        var disciplina = await client.CreateDisciplina("Banco de Dados", "BD", [curso.Id]);

        // Assert
        disciplina.Nome.Should().Be("Banco de Dados");
        disciplina.Cursos.Should().BeEquivalentTo([curso.Id]);
    }

    [Test]
    public async Task Deve_criar_uma_nova_disciplina_vincula_a_mais_de_um_curso()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var ads = await client.CreateCurso("Análise e Desenvolvimento de Sistemas");
        var cc = await client.CreateCurso("Ciência da Computação");

        // Act
        var disciplina = await client.CreateDisciplina("Banco de Dados", "BD", [ads.Id, cc.Id]);

        // Assert
        disciplina.Nome.Should().Be("Banco de Dados");
        disciplina.Cursos.Should().BeEquivalentTo([cc.Id, ads.Id]);
    }

    [Test]
    public async Task Deve_criar_varias_disciplinas()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        await client.CreateDisciplina("Banco de Dados");
        await client.CreateDisciplina("Estrutura de Dados");
        await client.CreateDisciplina("Programação Orientada a Objetos");

        // Assert
        var disciplinas = await client.GetAsync<List<DisciplinaOut>>("/disciplinas");
        disciplinas.Should().HaveCount(3);
    }

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
        disciplinas[0].Nome.Should().Be("Arquitetura de Computadores e Sistemas Operacionais");
        disciplinas[1].Nome.Should().Be("Banco de Dados");
        disciplinas[2].Nome.Should().Be("Estrutura de Dados");
        disciplinas[3].Nome.Should().Be("Informática e Sociedade");
        disciplinas[4].Nome.Should().Be("Programação Orientada a Objetos");
        disciplinas[5].Nome.Should().Be("Projeto Integrador II: Modelagem de Banco de Dados");
    }

    [Test]
    public async Task Deve_retornar_apenas_as_disciplinas_do_curso_informado()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var ads = await client.CreateCurso("Análise e Desenvolvimento de Sistemas");
        var cc = await client.CreateCurso("Ciência da Computação");

        await client.CreateDisciplina("Banco de Dados", "BD", [ads.Id, cc.Id]);
        await client.CreateDisciplina("Informática e Sociedade", "IS", [ads.Id]);
        await client.CreateDisciplina("Programação Orientada a Objetos", "POO", [cc.Id]);
        await client.CreateDisciplina("Projeto Integrador II: Modelagem de Banco de Dados", "PI-MBD", [cc.Id]);

        // Act
        var disciplinas = await client.GetAsync<List<DisciplinaOut>>($"/disciplinas?cursoId={ads.Id}");

        // Assert
        disciplinas.Should().HaveCount(2);
        disciplinas[0].Nome.Should().Be("Banco de Dados");
        disciplinas[1].Nome.Should().Be("Informática e Sociedade");
    }
}
