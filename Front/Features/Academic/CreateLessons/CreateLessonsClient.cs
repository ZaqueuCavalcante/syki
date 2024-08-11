namespace Syki.Front.Features.Academic.CreateLessons;

public class CreateLessonsClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Create(Guid id)
    {
        var response = await http.PostAsJsonAsync($"/academic/classes/{id}/lessons", new {});

        return await response.Resolve<SuccessOut>();
    }
}
