namespace Syki.Back.Features.Academic.GetCourseCurriculumDisciplines;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetCourseCurriculumDisciplinesController(GetCourseCurriculumDisciplinesService service) : ControllerBase
{
    [HttpGet("academic/course-curriculums/{id}/disciplines")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var disciplines = await service.Get(User.InstitutionId(), id);

        return Ok(disciplines);
    }
}
