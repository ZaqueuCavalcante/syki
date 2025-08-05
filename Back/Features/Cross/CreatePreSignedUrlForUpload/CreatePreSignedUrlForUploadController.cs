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
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Create([FromBody] CreatePreSignedUrlForUploadIn data)
    {
        var result = await service.Create(User.InstitutionId(), User.Id(), data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreatePreSignedUrlForUploadIn>;
internal class ResponseExamples : ExamplesProvider<CreatePreSignedUrlForUploadOut>;
