namespace Syki.Shared;

public class EventsSummaryOut
{
    public int Total { get; set; }
    public int Processed { get; set; }
    public int Pending { get; set; }
    public int Error { get; set; }
}
