using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;
using static Syki.Shared.TipoDeCurso;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class DisciplinasIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_uma_nova_disciplina_sem_vinculo_com_nenhum_curso()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };

        // Act
        var disciplina = await PostAsync<DisciplinaOut>("/disciplinas", body);

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
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var bodyAds = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Bacharelado };
        var curso = await PostAsync<CursoOut>("/cursos", bodyAds);

        var body = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [curso.Id] };

        // Act
        var disciplina = await PostAsync<DisciplinaOut>("/disciplinas", body);

        // Assert
        disciplina.Nome.Should().Be(body.Nome);
        disciplina.CargaHoraria.Should().Be(body.CargaHoraria);
        disciplina.Cursos.Should().BeEquivalentTo([curso.Id]);
    }

    [Test]
    public async Task Deve_criar_uma_nova_disciplina_vincula_a_mais_de_um_curso()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var bodyAds = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Bacharelado };
        var ads = await PostAsync<CursoOut>("/cursos", bodyAds);
        var bodyCC = new CursoIn { Nome = "Ciência da Computação", Tipo = Bacharelado };
        var cc = await PostAsync<CursoOut>("/cursos", bodyCC);

        var body = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [ads.Id, cc.Id] };

        // Act
        var disciplina = await PostAsync<DisciplinaOut>("/disciplinas", body);

        // Assert
        disciplina.Nome.Should().Be(body.Nome);
        disciplina.CargaHoraria.Should().Be(body.CargaHoraria);
        disciplina.Cursos.Should().BeEquivalentTo([ads.Id, cc.Id]);
    }

    [Test]
    public async Task Nao_deve_vincular_uma_disciplina_a_um_curso_de_outra_faculdade()
    {
        // Arrange
        var novaRoma = await CreateFaculdade("Nova Roma");
        var userNovaRoma = UserIn.New(novaRoma.Id, Academico);
        await RegisterUser(userNovaRoma);

        var ufpe = await CreateFaculdade("UFPE");
        var userUfpe = UserIn.New(ufpe.Id, Academico);
        await RegisterUser(userUfpe);

        await Login(userNovaRoma.Email, userNovaRoma.Password);
        var bodyNovaRoma = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Bacharelado };
        var adsNovaRoma = await PostAsync<CursoOut>("/cursos", bodyNovaRoma);

        await Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Licenciatura };
        var adsUfpe = await PostAsync<CursoOut>("/cursos", bodyUfpe);

        await Login(userNovaRoma.Email, userNovaRoma.Password);

        var body = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [adsNovaRoma.Id, adsUfpe.Id] };

        // Act
        var disciplina = await PostAsync<DisciplinaOut>("/disciplinas", body);

        // Assert
        disciplina.Nome.Should().Be(body.Nome);
        disciplina.CargaHoraria.Should().Be(body.CargaHoraria);
        disciplina.Cursos.Should().BeEquivalentTo([adsNovaRoma.Id]);
    }

    [Test]
    public async Task Deve_criar_varias_disciplinas()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        // Act
        await PostAsync("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 });
        await PostAsync("/disciplinas", new DisciplinaIn { Nome = "Estrutura de Dados", CargaHoraria = 60 });
        await PostAsync("/disciplinas", new DisciplinaIn { Nome = "Programação Orientada a Objetos", CargaHoraria = 55 });

        // Assert
        var disciplinas = await GetAsync<List<DisciplinaOut>>("/disciplinas");
        disciplinas.Should().HaveCount(3);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_uma_nova_disciplina_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, role);

        var body = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };

        // Act
        var response = await _client.PostAsync("/disciplinas", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Deve_retornar_apenas_as_disciplinas_da_faculdade_do_usuario_logado()
    {
        // Arrange
        var novaRoma = await CreateFaculdade("Nova Roma");
        var userNovaRoma = UserIn.New(novaRoma.Id, Academico);
        await RegisterUser(userNovaRoma);

        var ufpe = await CreateFaculdade("UFPE");
        var userUfpe = UserIn.New(ufpe.Id, Academico);
        await RegisterUser(userUfpe);

        await Login(userNovaRoma.Email, userNovaRoma.Password);
        var bodyNovaRoma = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };
        await PostAsync<DisciplinaOut>("/disciplinas", bodyNovaRoma);

        await Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new DisciplinaIn { Nome = "Estruturas de Dados", CargaHoraria = 80 };
        await PostAsync<DisciplinaOut>("/disciplinas", bodyUfpe);

        // Act
        await Login(userNovaRoma.Email, userNovaRoma.Password);

        var disciplinas = await GetAsync<List<DisciplinaOut>>("/disciplinas");
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
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        // Act
        await PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Estruturas de Dados", CargaHoraria = 80 });
        await PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 50 });
        await PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Informática e Sociedade", CargaHoraria = 40 });
        await PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Programação Orientada a Objetos", CargaHoraria = 55 });
        await PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Projeto Integrador II: Modelagem de Banco de Dados", CargaHoraria = 72 });
        await PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Arquitetura de Computadores e Sistemas Operacionais", CargaHoraria = 60 });

        // Assert
        var disciplinas = await GetAsync<List<DisciplinaOut>>("/disciplinas");
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
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var bodyAds = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Bacharelado };
        var ads = await PostAsync<CursoOut>("/cursos", bodyAds);
        var bodyCC = new CursoIn { Nome = "Ciência da Computação", Tipo = Bacharelado };
        var cc = await PostAsync<CursoOut>("/cursos", bodyCC);

        await PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 50, Cursos = [ads.Id, cc.Id] });
        await PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Informática e Sociedade", CargaHoraria = 40, Cursos = [ads.Id] });
        await PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Programação Orientada a Objetos", CargaHoraria = 55, Cursos = [cc.Id] });
        await PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Projeto Integrador II: Modelagem de Banco de Dados", CargaHoraria = 72, Cursos = [cc.Id] });

        // Act
        var disciplinas = await GetAsync<List<DisciplinaOut>>($"/disciplinas?cursoId={ads.Id}");
        disciplinas.Should().HaveCount(2);

        // Assert
        disciplinas[0].Nome.Should().Be("Banco de Dados");
        disciplinas[1].Nome.Should().Be("Informática e Sociedade");
    }
}
