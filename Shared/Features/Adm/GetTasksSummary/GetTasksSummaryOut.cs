namespace Syki.Shared;

public class GetTasksSummaryOut
{
    public TasksSummaryOut Summary { get; set; } = new();

    public List<TaskTypeCountOut> TaskTypes { get; set; } = [];

    public List<TinyInstitutionOut> Institutions { get; set; } = [];
}
