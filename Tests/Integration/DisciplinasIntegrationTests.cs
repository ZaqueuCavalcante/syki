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
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var body = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };

        // Act
        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", body);

        // Assert
        disciplina.Id.Should().NotBeEmpty();
        disciplina.Nome.Should().Be(body.Nome);
        disciplina.CargaHoraria.Should().Be(body.CargaHoraria);
        disciplina.Cursos.Should().BeEquivalentTo(new List<Guid>());
    }

    [Test]
    public async Task Deve_criar_uma_nova_disciplina_vincula_a_um_unico_curso()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var bodyAds = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Bacharelado };
        var curso = await client.PostAsync<CursoOut>("/cursos", bodyAds);

        var body = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [curso.Id] };

        // Act
        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", body);

        // Assert
        disciplina.Nome.Should().Be(body.Nome);
        disciplina.CargaHoraria.Should().Be(body.CargaHoraria);
        disciplina.Cursos.Should().BeEquivalentTo([curso.Id]);
    }

    [Test]
    public async Task Deve_criar_uma_nova_disciplina_vincula_a_mais_de_um_curso()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var bodyAds = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Bacharelado };
        var ads = await client.PostAsync<CursoOut>("/cursos", bodyAds);
        var bodyCC = new CursoIn { Nome = "Ciência da Computação", Tipo = Bacharelado };
        var cc = await client.PostAsync<CursoOut>("/cursos", bodyCC);

        var body = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [ads.Id, cc.Id] };

        // Act
        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", body);

        // Assert
        disciplina.Nome.Should().Be(body.Nome);
        disciplina.CargaHoraria.Should().Be(body.CargaHoraria);
        disciplina.Cursos.Should().BeEquivalentTo([ads.Id, cc.Id]);
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

        await client.Login(userNovaRoma.Email, userNovaRoma.Password);
        var bodyNovaRoma = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Bacharelado };
        var adsNovaRoma = await client.PostAsync<CursoOut>("/cursos", bodyNovaRoma);

        await client.Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Licenciatura };
        var adsUfpe = await client.PostAsync<CursoOut>("/cursos", bodyUfpe);

        await client.Login(userNovaRoma.Email, userNovaRoma.Password);

        var body = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [adsNovaRoma.Id, adsUfpe.Id] };

        // Act
        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", body);

        // Assert
        disciplina.Nome.Should().Be(body.Nome);
        disciplina.CargaHoraria.Should().Be(body.CargaHoraria);
        disciplina.Cursos.Should().BeEquivalentTo([adsNovaRoma.Id]);
    }

    [Test]
    public async Task Deve_criar_varias_disciplinas()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        // Act
        await client.PostAsync("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 });
        await client.PostAsync("/disciplinas", new DisciplinaIn { Nome = "Estrutura de Dados", CargaHoraria = 60 });
        await client.PostAsync("/disciplinas", new DisciplinaIn { Nome = "Programação Orientada a Objetos", CargaHoraria = 55 });

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

        await client.Login(userNovaRoma.Email, userNovaRoma.Password);
        var bodyNovaRoma = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };
        await client.PostAsync<DisciplinaOut>("/disciplinas", bodyNovaRoma);

        await client.Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new DisciplinaIn { Nome = "Estruturas de Dados", CargaHoraria = 80 };
        await client.PostAsync<DisciplinaOut>("/disciplinas", bodyUfpe);

        // Act
        await client.Login(userNovaRoma.Email, userNovaRoma.Password);

        var disciplinas = await client.GetAsync<List<DisciplinaOut>>("/disciplinas");
        disciplinas.Should().HaveCount(1);

        // Assert
        disciplinas[0].Id.Should().NotBeEmpty();
        disciplinas[0].Nome.Should().Be(bodyNovaRoma.Nome);
        disciplinas[0].CargaHoraria.Should().Be(bodyNovaRoma.CargaHoraria);
        disciplinas[0].Cursos.Should().BeEquivalentTo(new List<Guid>());
    }

    [Test]
    public async Task Deve_retornar_as_disciplinas_ordenadas_pelo_nome()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        // Act
        await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Estruturas de Dados", CargaHoraria = 80 });
        await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 50 });
        await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Informática e Sociedade", CargaHoraria = 40 });
        await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Programação Orientada a Objetos", CargaHoraria = 55 });
        await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Projeto Integrador II: Modelagem de Banco de Dados", CargaHoraria = 72 });
        await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Arquitetura de Computadores e Sistemas Operacionais", CargaHoraria = 60 });

        // Assert
        var disciplinas = await client.GetAsync<List<DisciplinaOut>>("/disciplinas");
        disciplinas.Should().HaveCount(6);

        disciplinas[0].Nome.Should().Be("Arquitetura de Computadores e Sistemas Operacionais");
        disciplinas[1].Nome.Should().Be("Banco de Dados");
        disciplinas[2].Nome.Should().Be("Estruturas de Dados");
        disciplinas[3].Nome.Should().Be("Informática e Sociedade");
        disciplinas[4].Nome.Should().Be("Programação Orientada a Objetos");
        disciplinas[5].Nome.Should().Be("Projeto Integrador II: Modelagem de Banco de Dados");
    }

    [Test]
    public async Task Deve_retornar_apenas_as_disciplinas_do_curso_informado()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var bodyAds = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Bacharelado };
        var ads = await client.PostAsync<CursoOut>("/cursos", bodyAds);
        var bodyCC = new CursoIn { Nome = "Ciência da Computação", Tipo = Bacharelado };
        var cc = await client.PostAsync<CursoOut>("/cursos", bodyCC);

        await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 50, Cursos = [ads.Id, cc.Id] });
        await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Informática e Sociedade", CargaHoraria = 40, Cursos = [ads.Id] });
        await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Programação Orientada a Objetos", CargaHoraria = 55, Cursos = [cc.Id] });
        await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Projeto Integrador II: Modelagem de Banco de Dados", CargaHoraria = 72, Cursos = [cc.Id] });

        // Act
        var disciplinas = await client.GetAsync<List<DisciplinaOut>>($"/disciplinas?cursoId={ads.Id}");
        disciplinas.Should().HaveCount(2);

        // Assert
        disciplinas[0].Nome.Should().Be("Banco de Dados");
        disciplinas[1].Nome.Should().Be("Informática e Sociedade");
    }
}
