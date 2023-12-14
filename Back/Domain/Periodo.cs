using Syki.Shared;

namespace Syki.Back.Domain;

public class Periodo
{
    // 2023.1 | 2023.2 | 2024.1 | 2024.2 | ...
    public string Id { get; set; }
    public Guid FaculdadeId { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }

    public PeriodoOut ToOut()
    {
        return new PeriodoOut
        {
            Id = Id,
            Start = Start,
            End = End,
        };
    }
}
