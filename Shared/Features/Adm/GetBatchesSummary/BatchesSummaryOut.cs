namespace Syki.Shared;

public class BatchesSummaryOut
{
    public int Total { get; set; }
    public int Pending { get; set; }
    public int Processing { get; set; }
    public int Success { get; set; }
    public int Error { get; set; }
}
