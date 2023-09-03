namespace Syki.Domain;

public class Periodo
{
    // 2023.1 | 2023.2 | 2024.1 | 2024.2 | ...
    public string Id { get; set; }
    
    public DateTime Start { get; set; }

    public DateTime End { get; set; }
}
