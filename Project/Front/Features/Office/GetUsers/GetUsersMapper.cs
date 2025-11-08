using MudBlazor;
using Exato.Shared.Features.Office.GetUsers;

namespace Exato.Front.Features.Office.GetUsers;

public static class GetUsersMapper
{
    extension(GetUsersItemOut item)
    {
        public (Color color, string icon, string status) GetTwoFactorEnabled()
        {
            if (item.TwoFactorEnabled) return (Color.Success, Icons.Material.Filled.SecurityUpdateGood, "2FA Habilitada");

            return (Color.Default, Icons.Material.Filled.SecurityUpdateWarning, "2FA Inabilitada");
        }
    }
}
