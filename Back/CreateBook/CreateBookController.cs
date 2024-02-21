using Syki.Back.Extensions;
using Syki.Shared.CreateBook;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Syki.Back.CreateBook;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class CreateBookController : ControllerBase
{
    private readonly CreateBookService _service;
    public CreateBookController(CreateBookService service) => _service = service;

    [HttpPost("books")]
    public async Task<IActionResult> Create([FromBody] CreateBookIn data)
    {
        var book = await _service.Create(User.InstitutionId(), data);

        return Ok(book);
    }
}
