namespace Syki.Front.Features.Teacher.GetClassNotesRemainingWeights;

public class GetClassNotesRemainingWeightsClient(HttpClient http) : ITeacherClient
{
    public async Task<OneOf<List<ClassNoteRemainingWeightsOut>, ErrorOut>> Get(Guid classId)
    {
        var response = await http.GetAsync($"/teacher/classes/{classId}/remaining-weights");

        return await response.Resolve<List<ClassNoteRemainingWeightsOut>>();
    }
}
