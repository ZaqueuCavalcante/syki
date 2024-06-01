namespace Syki.Back.Features.Seller.GetSellerCourseOfferings;

[ApiController, AuthSeller]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetSellerCourseOfferingsController(GetSellerCourseOfferingsService service) : ControllerBase
{
    [HttpGet("seller/course-offerings")]
    public async Task<IActionResult> Get()
    {
        var offerings = await service.Get(User.InstitutionId());

        return Ok(offerings);
    }
}
