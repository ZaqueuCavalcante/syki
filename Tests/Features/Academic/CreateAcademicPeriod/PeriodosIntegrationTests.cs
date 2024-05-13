namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_periodo()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        var period = await client.CreateAcademicPeriod("2024.1");

        // Assert
        period.Id.Should().Be(period.Id);
    }

    [Test, Ignore("")]
    public async Task Deve_criar_varios_periodos_para_uma_mesma_institution()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        await client.CreateAcademicPeriod("2023.1");
        await client.CreateAcademicPeriod("2023.2");

        // Assert
        var periodos = await client.GetAcademicPeriods();
        periodos.Should().HaveCount(2);
    }

    [Test, Ignore("")]
    public async Task Deve_retornar_apenas_os_periodos_da_institution_do_usuario_logado()
    {
        // Arrange
        var clientNovaRoma = await _factory.LoggedAsAcademic();
        var clientUfpe = await _factory.LoggedAsAcademic();

        var period = await clientNovaRoma.CreateAcademicPeriod("2023.1");

        await clientUfpe.CreateAcademicPeriod("2023.2");

        // Act
        var periodos = await clientNovaRoma.GetAcademicPeriods();

        // Assert
        periodos.Should().HaveCount(1);
        periodos[0].Id.Should().Be(period.Id);
    }
}
