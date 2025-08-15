namespace Syki.Front.Features.Academic.GetCampusCourseOfferings;

public class GetCampusCourseOfferingsClient(HttpClient http) : IAcademicClient
{
    public async Task<List<GetCampusCourseOfferingsOut>> Get(Guid campusId)
    {
        return await http.GetFromJsonAsync<List<GetCampusCourseOfferingsOut>>($"/academic/campi/{campusId}/course-offerings", HttpConfigs.JsonOptions) ?? [];
    }
}
