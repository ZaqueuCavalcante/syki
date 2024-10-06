namespace Syki.Front.Features.Academic.FinalizeClasses;

public class FinalizeClassesClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Finalize(List<Guid> classes)
    {
        var data = new FinalizeClassesIn { Classes = classes };

        var response = await http.PutAsJsonAsync("/academic/classes/finalize", data);

        return await response.Resolve<SuccessOut>();
    }
}
