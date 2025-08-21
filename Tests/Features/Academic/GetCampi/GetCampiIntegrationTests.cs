namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_empty_list_when_has_no_campi()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var campi = await client.GetCampi();

        // Assert
        campi.Should().BeEmpty();
    }

    [Test]
    public async Task Should_get_campi_ordered_by_name()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        CampusOut suassunaOut = await client.CreateCampus("Suassuna I", BrazilState.PE, "Recife", 1_500);
        CampusOut agresteOut = await client.CreateCampus("Agreste I", BrazilState.PE, "Caruaru", 280);

        // Act
        var campi = await client.GetCampi();

        // Assert
        campi.Should().HaveCount(2);

        campi[0].Should().BeEquivalentTo(agresteOut);
        campi[1].Should().BeEquivalentTo(suassunaOut);
    }

    [Test]
    public async Task Should_get_campi_with_student_metrics()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        await client.CreateStudent(data.AdsCourseOffering.Id);
        await client.CreateStudent(data.AdsCourseOffering.Id);

        // Act
        var campi = await client.GetCampi();

        // Assert
        var campus = campi.Single();
        campus.Students.Should().Be(2);
        campus.Capacity.Should().Be(100);
        campus.FillRate.Should().Be(2.00M);
    }

    [Test]
    public async Task Should_get_only_institution_campus()
    {
        // Arrange
        var clientA = await _api.LoggedAsAcademic();
        var clientB = await _api.LoggedAsAcademic();

        CampusOut campusA = await clientA.CreateCampus("Agreste I", BrazilState.PE, "Caruaru");
        await clientB.CreateCampus("Suassuna I", BrazilState.PE, "Recife");

        // Act
        var campi = await clientA.GetCampi();

        // Assert
        campi.Should().BeEquivalentTo([campusA]);
    }
}
