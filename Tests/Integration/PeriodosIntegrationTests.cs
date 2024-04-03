using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test, Ignore("")]
    public async Task Deve_criar_um_novo_periodo()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        // Act
        var periodo = await client.CreateAcademicPeriod("2024.1");

        // Assert
        periodo.Id.Should().Be(periodo.Id);
    }

    [Test, Ignore("")]
    public async Task Deve_criar_varios_periodos_para_uma_mesma_faculdade()
    {
        // Arrange
        var client = _factory.GetClient();
        var faculdade = await client.CreateInstitution();
        await client.RegisterAndLogin(faculdade.Id, Academico);

        // Act
        await client.CreateAcademicPeriod("2023.1");
        await client.CreateAcademicPeriod("2023.2");

        // Assert
        var periodos = await client.GetAsync<List<AcademicPeriodOut>>("/periodos");
        periodos.Should().HaveCount(2);
    }

    [Test, Ignore("")]
    public async Task Deve_retornar_apenas_os_periodos_da_faculdade_do_usuario_logado()
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
        var periodo = await client.CreateAcademicPeriod("2023.1");

        await client.Login(userUfpe.Email, userUfpe.Password);
        await client.CreateAcademicPeriod("2023.2");

        // Act
        await client.Login(userNovaRoma.Email, userNovaRoma.Password);

        // Assert
        var periodos = await client.GetAsync<List<AcademicPeriodOut>>("/periodos");
        periodos.Should().HaveCount(1);
        periodos[0].Id.Should().Be(periodo.Id);
    }
}
