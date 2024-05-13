using Audit.Core;
using Syki.Back.Audit;
using Audit.EntityFramework;

namespace Syki.Tests.Audit;

public class AuditUnitTests
{
    [Test]
    public void Deve_criar_uma_audit_change_com_valores_corretos()
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
    public void Deve_criar_um_audit_data_com_valores_corretos()
    {
        // Arrange / Act
        var audit = AuditData.NewAsJson(_eventEntry);

        // Assert
        audit.RootElement.GetProperty("Name").ToString().Should().Be(_eventEntry.Name);
        audit.RootElement.GetProperty("Table").ToString().Should().Be(_eventEntry.Table);
        audit.RootElement.GetProperty("Schema").ToString().Should().Be(_eventEntry.Schema);
    }

    [Test]
    public void Deve_criar_uma_audit_log_com_id_correto()
    {
        // Arrange
        var evt = new AuditEvent { CustomFields = [] };
        evt.CustomFields["UserId"] = Guid.NewGuid();
        evt.CustomFields["InstitutionId"] = Guid.NewGuid();

        // Act
        var audit = new AuditLog();
        audit.Fill(evt, _eventEntry);

        // Assert
        audit.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_uma_audit_log_com_entity_id_correto()
    {
        // Arrange
        var evt = new AuditEvent { CustomFields = [] };
        evt.CustomFields["UserId"] = Guid.NewGuid();
        evt.CustomFields["InstitutionId"] = Guid.NewGuid();

        // Act
        var audit = new AuditLog();
        audit.Fill(evt, _eventEntry);

        // Assert
        audit.EntityId.Should().Be(Guid.Parse("0346158a-f03f-4d95-b627-a154876c3f5b"));
    }

    [Test]
    public void Deve_criar_uma_audit_log_com_entity_type_correto()
    {
        // Arrange
        var evt = new AuditEvent { CustomFields = [] };
        evt.CustomFields["UserId"] = Guid.NewGuid();
        evt.CustomFields["InstitutionId"] = Guid.NewGuid();

        // Act
        var audit = new AuditLog();
        audit.Fill(evt, _eventEntry);

        // Assert
        audit.EntityType.Should().Be("Class");
    }

    [Test]
    public void Deve_criar_uma_audit_log_com_action_correta()
    {
        // Arrange
        var evt = new AuditEvent { CustomFields = [] };
        evt.CustomFields["UserId"] = Guid.NewGuid();
        evt.CustomFields["InstitutionId"] = Guid.NewGuid();

        // Act
        var audit = new AuditLog();
        audit.Fill(evt, _eventEntry);

        // Assert
        audit.Action.Should().Be("Insert");
    }

    [Test]
    public void Deve_criar_uma_audit_log_com_created_at_correto()
    {
        // Arrange
        var evt = new AuditEvent { CustomFields = [] };
        evt.CustomFields["UserId"] = Guid.NewGuid();
        evt.CustomFields["InstitutionId"] = Guid.NewGuid();

        // Act
        var audit = new AuditLog();
        audit.Fill(evt, _eventEntry);

        // Assert
        audit.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Test]
    public void Deve_criar_uma_audit_log_com_user_id_correto()
    {
        // Arrange
        var evt = new AuditEvent { CustomFields = [] };
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        evt.CustomFields["UserId"] = userId;
        evt.CustomFields["InstitutionId"] = institutionId;

        // Act
        var audit = new AuditLog();
        audit.Fill(evt, _eventEntry);

        // Assert
        audit.UserId.Should().Be(userId);
    }

    [Test]
    public void Deve_criar_uma_audit_log_com_institution_id_correto()
    {
        // Arrange
        var evt = new AuditEvent { CustomFields = [] };
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        evt.CustomFields["UserId"] = userId;
        evt.CustomFields["InstitutionId"] = institutionId;

        // Act
        var audit = new AuditLog();
        audit.Fill(evt, _eventEntry);

        // Assert
        audit.InstitutionId.Should().Be(institutionId);
    }

    [Test]
    public void Deve_retornar_false_quando_eh_um_request_de_login()
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
    public void Deve_retornar_true_quando_nao_eh_um_request_de_login()
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
        Name = "Turma",
        Table = "turmas",
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
            { "professor_id", "4ce214d0-cf13-453f-8543-7696d71827c5" },
            { "discipline_id", "2dd62a1e-e8ed-4d39-ae76-59a3c9983235" },
        },
        Changes =
        [
            new EventEntryChange { ColumnName = "name", OriginalValue = "Caruaru", NewValue = "Recife", },
            new EventEntryChange { ColumnName = "id", OriginalValue = "2023.1", NewValue = "2023.2", },
        ]
    };
}
