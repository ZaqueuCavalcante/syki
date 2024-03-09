namespace Syki.Back.Services;

public interface IAuthService
{
    Task<CreateUserOut> RegisterUser(CreateUserIn body);
    Task<CreateUserOut> Register(CreateUserIn body);
    Task<List<CreateUserOut>> GetAllUsers();
}
