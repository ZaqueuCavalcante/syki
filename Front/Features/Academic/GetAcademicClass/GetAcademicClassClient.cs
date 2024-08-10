namespace Syki.Front.Features.Academic.GetAcademicClass;

public class GetAcademicClassClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<GetAcademicClassOut, ErrorOut>> Get(Guid id)
    {
        var response = await http.GetAsync($"/academic/classes/{id}");

        return await response.Resolve<GetAcademicClassOut>();
    }
}
