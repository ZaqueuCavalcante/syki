using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Syki.Back.GetBooks;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class GetBooksController(GetBooksService service) : ControllerBase
{
    [HttpGet("books")]
    public async Task<IActionResult> Get()
    {
        var books = await service.Get(User.InstitutionId());

        return Ok(books);
    }
}
