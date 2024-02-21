using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.GetBooks;

[ApiController]
[EnableRateLimiting("Medium")]
public class GetBooksController : ControllerBase
{
    private readonly GetBooksService _service;
    public GetBooksController(GetBooksService service) => _service = service;

    [HttpGet("books")]
    [Authorize(Roles = Academico)]
    public async Task<IActionResult> Get()
    {
        var books = await _service.Get(User.InstitutionId());

        return Ok(books);
    }
}
