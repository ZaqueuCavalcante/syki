namespace Syki.Back.Features.Academic.CreateDiscipline;

public class Discipline
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public List<CourseDiscipline> Links { get; set; }

    private Discipline() { }

    public Discipline(
        Guid institutionId,
        string name
    ) {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
        Name = name;
        Code = $"{Guid.NewGuid().ToString()[..8].ToUpper()}";
        Links = [];
    }

    public DisciplineOut ToOut()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            Code = Code,
            Courses = Links?.ConvertAll(v => v.CourseId) ?? [],
        };
    }
}
