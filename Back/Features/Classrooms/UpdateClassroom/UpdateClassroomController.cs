namespace Estud.Back.Features.Classrooms.UpdateClassroom;

[ApiController, Authorize(Policies.UpdateClassroom)]
public class UpdateClassroomController(UpdateClassroomService service) : ControllerBase
{
    /// <summary>
    /// Editar sala de aula
    /// </summary>
    /// <remarks>
    /// Edita os dados da sala de aula informada.
    /// </remarks>
    [HttpPut("classrooms")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Update([FromBody] UpdateClassroomIn data)
    {
        var result = await service.Update(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<UpdateClassroomIn>;
internal class ResponseExamples : ExamplesProvider<UpdateClassroomOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidClassroomName,
    InvalidClassroomCapacity,
    ClassroomNotFound,
    CampusNotFound
>;
