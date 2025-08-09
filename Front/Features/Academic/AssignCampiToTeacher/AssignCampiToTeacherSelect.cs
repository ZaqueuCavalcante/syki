namespace Syki.Front.Features.Academic.AssignCampiToTeacher;

public class AssignCampiToTeacherSelect
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsSelected { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((AssignCampiToTeacherSelect)obj).Id;
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
