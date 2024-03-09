using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_professor()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var body = new ProfessorIn { Nome = "Chico", Email = TestData.Email };

        // Act
        var professor = await client.PostAsync<ProfessorOut>("/professores", body);

        // Assert
        professor.Id.Should().NotBeEmpty();
        professor.Nome.Should().Be(body.Nome);
    }

    [Test]
    public async Task Nao_deve_criar_um_professor_nem_seu_usuario_quando_der_erro()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var body = new ProfessorIn { Nome = "CC", Email = TestData.Email };

        var response = await client.PostHttpAsync("/professores", body);

        await client.LoginAsAdm();
        var users = await client.GetAsync<List<CreateUserOut>>("/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        users.FirstOrDefault(u => u.Email == body.Email).Should().BeNull();
    }

    [Test]
    public async Task Deve_criar_varios_professores_para_uma_mesma_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        // Act
        await client.PostAsync<ProfessorOut>("/professores", new ProfessorIn { Nome = "Chico", Email = TestData.Email });
        await client.PostAsync<ProfessorOut>("/professores", new ProfessorIn { Nome = "Maju", Email = TestData.Email });

        // Assert
        var professores = await client.GetAsync<List<ProfessorOut>>("/professores");
        professores.Should().HaveCount(2);
    }

    [Test]
    public async Task Deve_retornar_apenas_os_professores_da_faculdade_do_usuario_logado()
    {
        // Arrange
        var client = _factory.CreateClient();
        var novaRoma = await client.CreateInstitution("Nova Roma");
        var userNovaRoma = CreateUserIn.New(novaRoma.Id, Academico);
        await client.RegisterUser(userNovaRoma);

        var ufpe = await client.CreateInstitution("UFPE");
        var userUfpe = CreateUserIn.New(ufpe.Id, Academico);
        await client.RegisterUser(userUfpe);

        await client.Login(userNovaRoma.Email, userNovaRoma.Password);
        var bodyNovaRoma = new ProfessorIn { Nome = "Chico", Email = TestData.Email };
        var professor = await client.PostAsync<CampusOut>("/professores", bodyNovaRoma);

        await client.Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new ProfessorIn { Nome = "Maju", Email = TestData.Email };
        await client.PostAsync<CampusOut>("/professores", bodyUfpe);

        // Act
        await client.Login(userNovaRoma.Email, userNovaRoma.Password);

        // Assert
        var professores = await client.GetAsync<List<ProfessorOut>>("/professores");
        professores.Should().HaveCount(1);
        professores[0].Id.Should().Be(professor.Id);
        professores[0].Nome.Should().Be(bodyNovaRoma.Nome);
    }
}
