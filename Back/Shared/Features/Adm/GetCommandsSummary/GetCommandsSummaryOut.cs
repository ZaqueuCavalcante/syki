namespace Estud.Back.Shared;

public class GetCommandsSummaryOut
{
    public CommandsSummaryOut Summary { get; set; } = new();

    public List<CommandTypeCountOut> Types { get; set; } = [];

    public List<TinyInstitutionOut> Institutions { get; set; } = [];
}
