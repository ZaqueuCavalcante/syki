namespace Syki.Shared;

public class GetBatchesSummaryOut
{
    public BatchesSummaryOut Summary { get; set; } = new();

    public List<BatchTypeCountOut> Types { get; set; } = [];

    public List<TinyInstitutionOut> Institutions { get; set; } = [];
}
