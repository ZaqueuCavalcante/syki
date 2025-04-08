namespace Syki.Shared;

public class ClassActivityWorkOut
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; }
    public ClassActivityWorkStatus Status { get; set; }
    public decimal Note { get; set; }
    public string? Link { get; set; }

    public string GetLink()
    {
        return Link.HasValue() ? Link! : "-";
    }
}
