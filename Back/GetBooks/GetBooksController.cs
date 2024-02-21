using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Syki.Back.GetBooks;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class GetBooksController : ControllerBase
{
    private readonly GetBooksService _service;
    public GetBooksController(GetBooksService service) => _service = service;

    [HttpGet("books")]
    public async Task<IActionResult> Get()
    {
        var books = await _service.Get(User.InstitutionId());

        return Ok(books);
    }
}
