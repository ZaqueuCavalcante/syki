namespace Syki.Front.Features.Academic.StartClass;

public class StartClassClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Start(Guid id)
    {
        var response = await http.PutAsJsonAsync($"/academic/classes/{id}/start", new { });

        return await response.Resolve<SuccessOut>();
    }
}
