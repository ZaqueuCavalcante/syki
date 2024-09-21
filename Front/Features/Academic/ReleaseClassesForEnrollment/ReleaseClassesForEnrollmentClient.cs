namespace Syki.Front.Features.Academic.ReleaseClassesForEnrollment;

public class ReleaseClassesForEnrollmentClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Release(List<Guid> classes)
    {
        var data = new ReleaseClassesForEnrollmentIn { Classes = classes };

        var response = await http.PutAsJsonAsync("/academic/classes/release-for-enrollment", data);

        return await response.Resolve<SuccessOut>();
    }
}
