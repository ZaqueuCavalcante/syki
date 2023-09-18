namespace Syki.Domain;

public class Periodo
{
    // 2023.1 | 2023.2 | 2024.1 | 2024.2 | ...
    public string Id { get; set; }

    public long FaculdadeId { get; set; }

    public DateOnly Start { get; set; }

    public DateOnly End { get; set; }
}
