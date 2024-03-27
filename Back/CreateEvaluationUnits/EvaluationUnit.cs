namespace Syki.Back.CreateEvaluationUnits;

public class EvaluationUnit
{
    public Guid Id { get; set; }
    public Guid TurmaId { get; set; }
    public int Order { get; set; }
    public string Name { get; set; }
    public List<Evaluation> Evaluations { get; set; }

    private EvaluationUnit() { }

    public EvaluationUnit(
        Guid id,
        Guid turmaId,
        int order,
        string name,
        List<Evaluation> evaluations
    ) {
        Id = id;
        TurmaId = turmaId;
        Order = order;
        Name = name;
        Evaluations = evaluations;
    }
}
