namespace Estud.Back.Features.Teachers.GetTeacherAgenda;

public class GetTeacherAgendaOut : IApiDto<GetTeacherAgendaOut>
{
    public List<GetTeacherAgendaItemOut> Days { get; set; } = [];

    public static IEnumerable<(string Name, GetTeacherAgendaOut Value)> GetExamples() =>
    [
        new() { Name = "Exemplo", Value = new() }
    ];
}

public class GetTeacherAgendaItemOut
{
    public Day Day { get; set; }
    public List<GetTeacherAgendaItemDisciplineOut> Disciplines { get; set; } = [];
}

public class GetTeacherAgendaItemDisciplineOut
{
    public int? ClassId { get; set; }
    public string Name { get; set; }
    public string? ClassroomName { get; set; }
    public Hour Start { get; set; }
    public Hour End { get; set; }
}
