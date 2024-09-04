namespace Syki.Back.Features.Academic;

[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class TrackController() : ControllerBase
{
    [HttpPost("track/segmentations")]
    [Authorize(Policy = TrackApiPolicy.CreateSegmentation, AuthenticationSchemes = AuthenticationConfigs.BearerScheme)]
    public IActionResult CreateSegmentation()
    {
        return Ok(new { Result = "Criada" });
    }

    [HttpGet("track/campaigns")]
    [Authorize(Policy = TrackApiPolicy.GetCampaigns, AuthenticationSchemes = AuthenticationConfigs.BearerScheme)]
    public IActionResult GetCampaigns()
    {
        return Ok(new { Result = "Campanhas" });
    }

    [HttpGet("track/donors")]
    [Authorize(Policy = TrackApiPolicy.GetDonors, AuthenticationSchemes = AuthenticationConfigs.BearerScheme)]
    public IActionResult GetDonors()
    {
        return Ok(new { Result = "Doadores" });
    }
}
