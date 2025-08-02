namespace Syki.Back.Features.Cross.CreatePreSignedUrlForUpload;

[ApiController, AuthBearer]
[EnableRateLimiting("Small")]
public class CreatePreSignedUrlForUploadController(CreatePreSignedUrlForUploadService service) : ControllerBase
{
    /// <summary>
    /// Url para upload
    /// </summary>
    /// <remarks>
    /// Cria uma url para upload.
    /// </remarks>
    [HttpPut("files/pre-signed-url")]
    [DbContextTransactionFilter]
    public async Task<IActionResult> Create([FromBody] CreatePreSignedUrlForUploadIn data)
    {
        var result = await service.Create(User.InstitutionId(), User.Id(),  data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
