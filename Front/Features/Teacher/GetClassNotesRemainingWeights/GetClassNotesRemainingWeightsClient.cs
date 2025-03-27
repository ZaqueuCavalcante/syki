namespace Syki.Front.Features.Teacher.GetClassNotesRemainingWeights;

public class GetClassNotesRemainingWeightsClient(HttpClient http) : ITeacherClient
{
    public async Task<List<ClassNoteRemainingWeightsOut>> Get(Guid id)
    {
        return await http.GetFromJsonAsync<List<ClassNoteRemainingWeightsOut>>($"/teacher/classes/{id}/remaining-weights", HttpConfigs.JsonOptions) ?? new();
    }
}
