namespace Syki.Shared;

public class EvaluationUnitIn
{
    public Guid Id { get; set; }
    public int Order { get; set; }
    public string Name { get; set; }
    public List<EvaluationIn> Evaluations { get; set; } = [];
}
