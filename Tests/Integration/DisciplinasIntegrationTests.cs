using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using static Syki.Shared.TipoDeCurso;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_uma_nova_disciplina_sem_vinculo_com_nenhum_curso()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        var disciplina = await client.NewDisciplina("Banco de Dados", 72);

        // Assert
        disciplina.Id.Should().NotBeEmpty();
        disciplina.Nome.Should().Be("Banco de Dados");
        disciplina.CargaHoraria.Should().Be(72);
        disciplina.Cursos.Should().BeEquivalentTo(new List<Guid>());
    }

    [Test]
    public async Task Deve_criar_uma_nova_disciplina_vincula_a_um_unico_curso()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        var curso = await client.NewCurso("Análise e Desenvolvimento de Sistemas", Bacharelado);

        // Act
        var disciplina = await client.NewDisciplina("Banco de Dados", 72, [curso.Id]);

        // Assert
        disciplina.Nome.Should().Be("Banco de Dados");
        disciplina.CargaHoraria.Should().Be(72);
        disciplina.Cursos.Should().BeEquivalentTo([curso.Id]);
    }

    [Test]
    public async Task Deve_criar_uma_nova_disciplina_vincula_a_mais_de_um_curso()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        var ads = await client.NewCurso("Análise e Desenvolvimento de Sistemas", Bacharelado);
        var cc = await client.NewCurso("Ciência da Computação", Bacharelado);

        // Act
        var disciplina = await client.NewDisciplina("Banco de Dados", 72, [ads.Id, cc.Id]);

        // Assert
        disciplina.Nome.Should().Be("Banco de Dados");
        disciplina.CargaHoraria.Should().Be(72);
        disciplina.Cursos.Should().BeEquivalentTo([cc.Id, ads.Id]);
    }

    [Test]
    public async Task Nao_deve_vincular_uma_disciplina_a_um_curso_de_outra_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();

        var novaRoma = await client.CreateFaculdade("Nova Roma");
        var userNovaRoma = UserIn.New(novaRoma.Id, Academico);
        await client.RegisterUser(userNovaRoma);

        var ufpe = await client.CreateFaculdade("UFPE");
        var userUfpe = UserIn.New(ufpe.Id, Academico);
        await client.RegisterUser(userUfpe);

        await client.Login(userNovaRoma);
        var adsNovaRoma = await client.NewCurso("Análise e Desenvolvimento de Sistemas", Bacharelado);

        await client.Login(userUfpe);
        var adsUfpe = await client.NewCurso("Análise e Desenvolvimento de Sistemas", Bacharelado);

        await client.Login(userNovaRoma);

        // Act
        var disciplina = await client.NewDisciplina("Banco de Dados", 72, [adsNovaRoma.Id, adsUfpe.Id]);

        // Assert
        disciplina.Nome.Should().Be("Banco de Dados");
        disciplina.CargaHoraria.Should().Be(72);
        disciplina.Cursos.Should().BeEquivalentTo([adsNovaRoma.Id]);
    }

    [Test]
    public async Task Deve_criar_varias_disciplinas()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        await client.NewDisciplina("Banco de Dados", 72);
        await client.NewDisciplina("Estrutura de Dados", 60);
        await client.NewDisciplina("Programação Orientada a Objetos", 55);

        // Assert
        var disciplinas = await client.GetAsync<List<DisciplinaOut>>("/disciplinas");
        disciplinas.Should().HaveCount(3);
    }

    [Test]
    public async Task Deve_retornar_apenas_as_disciplinas_da_faculdade_do_usuario_logado()
    {
        // Arrange
        var client = _factory.CreateClient();

        var novaRoma = await client.CreateFaculdade("Nova Roma");
        var userNovaRoma = UserIn.New(novaRoma.Id, Academico);
        await client.RegisterUser(userNovaRoma);

        var ufpe = await client.CreateFaculdade("UFPE");
        var userUfpe = UserIn.New(ufpe.Id, Academico);
        await client.RegisterUser(userUfpe);

        await client.Login(userNovaRoma);
        await client.NewDisciplina("Banco de Dados", 72);

        await client.Login(userUfpe);
        await client.NewDisciplina("Estrutura de Dados", 80);

        // Act
        await client.Login(userNovaRoma);

        // Assert
        var disciplinas = await client.GetAsync<List<DisciplinaOut>>("/disciplinas");
        disciplinas.Should().HaveCount(1);
        disciplinas[0].Id.Should().NotBeEmpty();
        disciplinas[0].Nome.Should().Be("Banco de Dados");
        disciplinas[0].CargaHoraria.Should().Be(72);
        disciplinas[0].Cursos.Should().BeEquivalentTo(new List<Guid>());
    }

    [Test]
    public async Task Deve_retornar_as_disciplinas_ordenadas_pelo_nome()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        await client.NewDisciplina("Estrutura de Dados", 60);
        await client.NewDisciplina("Banco de Dados", 72);
        await client.NewDisciplina("Programação Orientada a Objetos", 55);
        await client.NewDisciplina("Informática e Sociedade", 40);
        await client.NewDisciplina("Projeto Integrador II: Modelagem de Banco de Dados", 72);
        await client.NewDisciplina("Arquitetura de Computadores e Sistemas Operacionais", 55);

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

        var ads = await client.NewCurso("Análise e Desenvolvimento de Sistemas", Bacharelado);
        var cc = await client.NewCurso("Ciência da Computação", Bacharelado);

        await client.NewDisciplina("Banco de Dados", 50, [ads.Id, cc.Id]);
        await client.NewDisciplina("Informática e Sociedade", 40, [ads.Id]);
        await client.NewDisciplina("Programação Orientada a Objetos", 55, [cc.Id]);
        await client.NewDisciplina("Projeto Integrador II: Modelagem de Banco de Dados", 72, [cc.Id]);

        // Act
        var disciplinas = await client.GetAsync<List<DisciplinaOut>>($"/disciplinas?cursoId={ads.Id}");

        // Assert
        disciplinas.Should().HaveCount(2);
        disciplinas[0].Nome.Should().Be("Banco de Dados");
        disciplinas[1].Nome.Should().Be("Informática e Sociedade");
    }
}
