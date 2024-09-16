// namespace Syki.Back.Features.Cross.SkipUserRegister;

// [ApiController]
// [EnableRateLimiting("Small")]
// [Consumes("application/json"), Produces("application/json")]
// public class SkipUserRegisterController(SkipUserRegisterService service) : ControllerBase
// {
//     [HttpPost("skip-user-register")]
//     public async Task<IActionResult> Skip([FromBody] SkipRegisterLoginIn data)
//     {
//         var result = await service.Skip(data);

//         return result.Match<IActionResult>(Ok, BadRequest);
//     }
// }
