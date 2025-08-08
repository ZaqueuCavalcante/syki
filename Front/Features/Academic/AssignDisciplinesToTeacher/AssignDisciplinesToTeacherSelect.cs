namespace Syki.Front.Features.Academic.AssignDisciplinesToTeacher;

public class AssignDisciplinesToTeacherSelect
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((AssignDisciplinesToTeacherSelect)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }

    public override string ToString()
    {
        return Name;
    }
}
