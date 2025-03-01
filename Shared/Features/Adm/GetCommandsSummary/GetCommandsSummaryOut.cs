namespace Syki.Shared;

public class GetCommandsSummaryOut
{
    public CommandsSummaryOut Summary { get; set; } = new();

    public List<CommandTypeCountOut> TaskTypes { get; set; } = [];

    public List<TinyInstitutionOut> Institutions { get; set; } = [];
}
