namespace Syki.Back.CreateEvaluationUnits;

public class Evaluation
{
    public Guid Id { get; set; }
    public Guid UnitId { get; set; }
    public int Order { get; set; }
    public string Name { get; set; }
    public int Weight { get; set; }

    private Evaluation() { }

    public Evaluation(
        Guid id,
        Guid unitId,
        int order,
        string name,
        int weight
    ) {
        Id = id;
        UnitId = unitId;
        Order = order;
        Name = name;
        Weight = weight;
    }
}
