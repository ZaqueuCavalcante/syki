namespace Syki.Back.Extensions;

public class AuthSellerAttribute : AuthorizeAttribute
{
	public AuthSellerAttribute()
	{
		Roles = UserRole.Seller.ToString();
		AuthenticationSchemes = AuthenticationConfigs.BearerScheme;
	}
}
