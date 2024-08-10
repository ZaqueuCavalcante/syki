namespace Syki.Front.Features.Seller.GetSellerCourseOfferings;

public class GetSellerCourseOfferingsClient(HttpClient http) : ISellerClient
{
    public async Task<List<CourseOfferingOut>> Get()
    {
        return await http.GetFromJsonAsync<List<CourseOfferingOut>>("/seller/course-offerings") ?? [];
    }
}
