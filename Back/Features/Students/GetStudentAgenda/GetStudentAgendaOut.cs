namespace Estud.Back.Features.Students.GetStudentAgenda;

public class GetStudentAgendaOut : IApiDto<GetStudentAgendaOut>
{
    public List<GetStudentAgendaItemOut> Days { get; set; } = [];

    public static IEnumerable<(string Name, GetStudentAgendaOut Value)> GetExamples() =>
    [
        new() { Name = "Exemplo", Value = new() }
    ];
}

public class GetStudentAgendaItemOut
{
    public Day Day { get; set; }
    public List<GetStudentAgendaItemDisciplineOut> Disciplines { get; set; } = [];
}

public class GetStudentAgendaItemDisciplineOut
{
    public int? ClassId { get; set; }
    public string Name { get; set; }
    public string? ClassroomName { get; set; }
    public Hour Start { get; set; }
    public Hour End { get; set; }
}
