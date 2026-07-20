namespace Estud.Back.Features.Users.GetAuthStatus;

[ApiController, Authorize(Policies.GetAuthStatus)]
public class GetAuthStatusController : ControllerBase
{
    /// <summary>
    /// Status de autenticação
    /// </summary>
    /// <remarks>
    /// Retorna 204 se o usuário está autenticado e 401 caso contrário. Valida apenas o JWT do
    /// cookie (assinatura + expiração, em memória), sem acessar o banco — feito para ser chamado
    /// no carregamento da landing page para decidir rapidamente se deve redirecionar para /home.
    /// </remarks>
    [HttpGet("users/logged")]
    public IActionResult Get()
    {
        return NoContent();
    }
}
