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
        campi.Total.Should().Be(0);
        campi.Items.Should().BeEmpty();
    }

    [Test]
    public async Task Should_get_campi_ordered_by_name()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        CreateCampusOut suassunaOut = await client.CreateCampus("Suassuna I", BrazilState.PE, "Recife", 1_500);
        CreateCampusOut agresteOut = await client.CreateCampus("Agreste I", BrazilState.PE, "Caruaru", 280);

        // Act
        var campi = await client.GetCampi();

        // Assert
        campi.Total.Should().Be(2);
        campi.Items[0].Should().BeEquivalentTo(agresteOut);
        campi.Items[1].Should().BeEquivalentTo(suassunaOut);
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
        campi.Total.Should().Be(1);
        var campus = campi.Items.Single();
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

        CreateCampusOut campusA = await clientA.CreateCampus("Agreste I", BrazilState.PE, "Caruaru");
        await clientB.CreateCampus("Suassuna I", BrazilState.PE, "Recife");

        // Act
        var campi = await clientA.GetCampi();

        // Assert
        campi.Items.Should().BeEquivalentTo([campusA]);
    }
}
