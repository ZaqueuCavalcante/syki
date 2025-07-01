using System.Security.Claims;

namespace Syki.Tests.Base;

public static class StringExtensions
{
    public static List<Claim> ToClaims(this string jwt)
    {
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(jwt);
        return jwtToken.Claims.ToList();
    }
}
