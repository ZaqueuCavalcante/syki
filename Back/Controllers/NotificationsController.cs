using Syki.Shared;
using Syki.Back.Services;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[ApiController, Route("[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationsService _service;
    public NotificationsController(INotificationsService service) => _service = service;

    [HttpPost("")]
    [Authorize(Roles = Academico)]
    public async Task<IActionResult> Create([FromBody] NotificationIn body)
    {
        var notification = await _service.Create(User.Facul(), body);

        return Created("", notification);
    }

    [Authorize]
    [HttpPut("user")]
    public async Task<IActionResult> ViewByUserId()
    {
        await _service.ViewByUserId(User.Facul(), User.Id());

        return Ok();
    }

    [HttpGet("")]
    [Authorize(Roles = Academico)]
    public async Task<IActionResult> GetAll()
    {
        var notifications = await _service.GetAll(User.Facul());
        
        return Ok(notifications);
    }

    [Authorize]
    [HttpGet("user")]
    public async Task<IActionResult> GetByUserId()
    {
        var notifications = await _service.GetByUserId(User.Facul(), User.Id());
        
        return Ok(notifications);
    }
}
