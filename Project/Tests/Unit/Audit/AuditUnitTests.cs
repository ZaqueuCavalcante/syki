using Audit.Core;
using Exato.Back.Audit;
using Exato.Back.Extensions;
using Audit.EntityFramework;
using Exato.Back.Intelligence.Domain.Public;

namespace Exato.Tests.Unit.Audit;

public class AuditUnitTests
{
    [Test]
    public void Should_create_audit_change_with_correct_values()
    {
        // Arrange
        var change = new EventEntryChange
        {
            ColumnName = "nome",
            OriginalValue = "Adeco",
            NewValue = "Adecco",
        };

        // Act
        var audit = new AuditChange(change);

        // Assert
        audit.Column.Should().Be("nome");
        audit.Old.Should().Be("Adeco");
        audit.New.Should().Be("Adecco");
    }

    [Test]
    public void Should_create_audit_data_with_correct_values()
    {
        // Arrange / Act
        var audit = AuditData.NewAsJson(_eventEntry);

        // Assert
        audit.RootElement.GetProperty("Name").ToString().Should().Be(_eventEntry.Name);
        audit.RootElement.GetProperty("Table").ToString().Should().Be(_eventEntry.Table);
        audit.RootElement.GetProperty("Schema").ToString().Should().Be(_eventEntry.Schema);
    }

    [Test]
    public void Should_create_audit_trail_with_correct_values()
    {
        // Arrange
        var evt = new AuditEvent { CustomFields = [] };
        var userId = Guid.NewGuid();
        var organizationId = 54684;
        var operation = "CriarEmpresaController";
        var activityId = "00-8177e2f84b3d256202b144bce64a81a2-9acf95a5fcb17673-01";
        evt.CustomFields[AuditExtensions.UserId] = userId;
        evt.CustomFields[AuditExtensions.OrganizationId] = organizationId;
        evt.CustomFields[AuditExtensions.Operation] = operation;
        evt.CustomFields[AuditExtensions.ActivityId] = activityId;

        // Act
        var audit = new AuditTrail();
        audit.Fill(evt, _eventEntry);

        // Assert
        audit.Id.Should().NotBeEmpty();
        audit.EntityId.Should().Be("159753");
        audit.EntityType.Should().Be("Cliente");
        audit.Action.Should().Be("Insert");
        audit.Operation.Should().Be(operation);
        audit.ActivityId.Should().Be(activityId);
        audit.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        audit.UserId.Should().Be(userId);
        audit.OrganizationId.Should().Be(organizationId);
    }

    private static EventEntry _eventEntry = new()
    {
        Name = "Cliente",
        Table = "cliente",
        Action = "Insert",
        Schema = "public",
        EntityType = typeof(Cliente),
        PrimaryKey = new Dictionary<string, object>()
        {
            { "ClienteId", "159753" },
        },
        ColumnValues = new Dictionary<string, object>()
        {
            { "cliente_id", "159753" },
            { "cpf_cnpj", "42281279839" },
            { "external_id", "4e137a1e-5bd2-4aa6-b24c-e9e6237e3cdf" }
        },
        Changes =
        [
            new EventEntryChange { ColumnName = "ativo", OriginalValue = "False", NewValue = "True" },
        ]
    };
}
