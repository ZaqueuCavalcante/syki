namespace Syki.Back.Extensions;

public class SykiController : ControllerBase
{
    protected BadRequestObjectResult BadRequest(SykiError error)
    {
        return BadRequest(new ErrorOut { Message = error.Message });
    }
}
