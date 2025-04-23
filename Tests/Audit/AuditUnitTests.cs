using Audit.Core;
using Syki.Back.Audit;
using Audit.EntityFramework;
using Syki.Back.Features.Academic.CreateClass;

namespace Syki.Tests.Audit;

public class AuditUnitTests
{
    [Test]
    public void Should_create_audit_change_with_correct_values()
    {
        // Arrange
        var change = new EventEntryChange
        {
            ColumnName = "name",
            OriginalValue = "Caruaru",
            NewValue = "Recife",
        };

        // Act
        var audit = new AuditChange(change);

        // Assert
        audit.Column.Should().Be("name");
        audit.Old.Should().Be("Caruaru");
        audit.New.Should().Be("Recife");
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
    public void Should_create_audit_log_with_correct_values()
    {
        // Arrange
        var evt = new AuditEvent { CustomFields = [] };
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        evt.CustomFields["UserId"] = userId;
        evt.CustomFields["InstitutionId"] = institutionId;;

        // Act
        var audit = new AuditLog();
        audit.Fill(evt, _eventEntry);

        // Assert
        audit.Id.Should().NotBeEmpty();
        audit.EntityId.Should().Be(Guid.Parse("0346158a-f03f-4d95-b627-a154876c3f5b"));
        audit.EntityType.Should().Be("Class");
        audit.Action.Should().Be("Insert");
        audit.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        audit.UserId.Should().Be(userId);
        audit.InstitutionId.Should().Be(institutionId);
    }

    [Test]
    public void Should_return_false_on_login_request()
    {
        // Arrange
        var evt = new AuditEvent { CustomFields = [] };
        evt.CustomFields["Skip"] = true;

        // Act
        var audit = new AuditLog();
        var result = audit.Fill(evt, _eventEntry);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Should_return_true_on_non_login_request()
    {
        // Arrange
        var evt = new AuditEvent { CustomFields = [] };
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        evt.CustomFields["UserId"] = userId;
        evt.CustomFields["InstitutionId"] = institutionId;

        // Act
        var audit = new AuditLog();
        var result = audit.Fill(evt, _eventEntry);

        // Assert
        result.Should().BeTrue();
    }

    private static EventEntry _eventEntry = new()
    {
        Name = "Class",
        Table = "classes",
        Action = "Insert",
        Schema = "syki",
        EntityType = typeof(Class),
        PrimaryKey = new Dictionary<string, object>()
        {
            { "Id", "0346158a-f03f-4d95-b627-a154876c3f5b" },
        },
        ColumnValues = new Dictionary<string, object>()
        {
            { "id", "0346158a-f03f-4d95-b627-a154876c3f5b" },
            { "period", "2023.2" },
            { "institution_id", "8d08e437-8b18-4a15-a231-4a2260e60432" },
            { "teacher_id", "4ce214d0-cf13-453f-8543-7696d71827c5" },
            { "discipline_id", "2dd62a1e-e8ed-4d39-ae76-59a3c9983235" },
        },
        Changes =
        [
            new EventEntryChange { ColumnName = "period", OriginalValue = "2023.1", NewValue = "2023.2", },
        ]
    };
}
