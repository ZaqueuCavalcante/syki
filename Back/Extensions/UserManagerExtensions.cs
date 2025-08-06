using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Extensions;

public static class UserManagerExtensions
{
    extension(UserManager<SykiUser> userManager)
    {
        public async Task<bool> IsOnlyInRoleAsync(SykiUser user, UserRole role)
        {
            var adm = await userManager.IsInRoleAsync(user!, UserRole.Adm.ToString());
            var student = await userManager.IsInRoleAsync(user!, UserRole.Student.ToString());
            var teacher = await userManager.IsInRoleAsync(user!, UserRole.Teacher.ToString());
            var academic = await userManager.IsInRoleAsync(user!, UserRole.Academic.ToString());

            return role switch
            {
                UserRole.Academic => academic && !(adm || student || teacher),
                UserRole.Student => student && !(adm || academic || teacher),
                UserRole.Teacher => teacher && !(adm || student || academic),
                UserRole.Adm => adm && !(student || teacher || academic),
                _ => false
            };
        }
    }
}
