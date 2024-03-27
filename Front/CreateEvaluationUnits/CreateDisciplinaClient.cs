namespace Front.CreateEvaluationUnits;

public class CreateEvaluationUnitsClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(Guid turmaId, List<EvaluationUnitIn> units)
    {
        var data = new CreateEvaluationUnitsIn { TurmaId = turmaId, Units = units };
        return await http.PostAsJsonAsync("/evaluation-units", data);
    }
}
