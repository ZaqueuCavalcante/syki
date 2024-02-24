using Syki.Shared.CreateCampus;
using Syki.Shared.UpdateCampus;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_atualizar_um_campus()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        var campus = await client.NewCampus("Agreste I", "Caruaru - PE");
        var body = new UpdateCampusIn { Id = campus.Id, Name = "Agreste II", City = "Bonito - PE" };

        // Act
        var updatedCampus = await client.PutAsync<CampusOut>("/campi", body);

        // Assert
        updatedCampus.Id.Should().Be(campus.Id);
        updatedCampus.Name.Should().Be(body.Name);
        updatedCampus.City.Should().Be(body.City);
    }
}
