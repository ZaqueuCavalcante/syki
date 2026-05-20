using Syki.Back.Domain.Courses;

namespace Syki.Back.Domain.Disciplines;

public class Discipline
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public List<CourseDiscipline> Links { get; set; }

    private Discipline() { }

    public Discipline(
        int institutionId,
        string name
    ) {
        InstitutionId = institutionId;
        Name = name;
        Code = $"{Guid.NewGuid().ToString()[..8].ToUpper()}";
        Links = [];
    }
}
