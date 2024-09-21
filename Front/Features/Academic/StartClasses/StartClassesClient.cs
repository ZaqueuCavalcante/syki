namespace Syki.Front.Features.Academic.StartClasses;

public class StartClassesClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Start(List<Guid> classes)
    {
        var data = new StartClassesIn { Classes = classes };

        var response = await http.PutAsJsonAsync("/academic/classes/start", data);

        return await response.Resolve<SuccessOut>();
    }
}
