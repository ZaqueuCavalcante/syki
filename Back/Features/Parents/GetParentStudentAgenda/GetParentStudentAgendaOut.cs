namespace Estud.Back.Features.Parents.GetParentStudentAgenda;

public class GetParentStudentAgendaOut : IApiDto<GetParentStudentAgendaOut>
{
    public List<GetParentStudentAgendaItemOut> Days { get; set; } = [];

    public static IEnumerable<(string Name, GetParentStudentAgendaOut Value)> GetExamples() =>
    [
        new() { Name = "Exemplo", Value = new() }
    ];
}

public class GetParentStudentAgendaItemOut
{
    public Day Day { get; set; }
    public List<GetParentStudentAgendaItemDisciplineOut> Disciplines { get; set; } = [];
}

public class GetParentStudentAgendaItemDisciplineOut
{
    public int? ClassId { get; set; }
    public string Name { get; set; }
    public string? ClassroomName { get; set; }
    public Hour Start { get; set; }
    public Hour End { get; set; }
}
