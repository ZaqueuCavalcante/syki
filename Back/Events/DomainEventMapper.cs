namespace Syki.Back.Events;

public static class DomainEventMapper
{
    public static string ToDomainEventDescription(this string value)
    {
        if (value.IsEmpty()) return value;

        var type = typeof(IDomainEvent).Assembly.GetType(value)!;

        var customAttributes = (DomainEventDescriptionAttribute[])type.GetCustomAttributes(typeof(DomainEventDescriptionAttribute), true);
        var description = customAttributes[0].Description;
    
        return description;

        // if (value.Contains(nameof(PendingUserRegisterCreatedDomainEvent))) return "Registro de usuário criado";
        // if (value.Contains(nameof(ExamGradeNoteAddedDomainEvent))) return "Nota adicionada";

        // return value;
    }
}
