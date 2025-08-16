using MudBlazor;

namespace Syki.Front.Features.Academic.GetWebhookSubscription;

public static class WebhookCallStatusMapper
{
    public static string GetIcon(this WebhookCallStatus status)
    {
        return status switch
        {
            WebhookCallStatus.Success => Icons.Material.Filled.CheckCircleOutline,
            WebhookCallStatus.Error => Icons.Material.Filled.ErrorOutline,
            _ => ""
        };
    }

    public static Color GetColor(this WebhookCallStatus status)
    {
        return status switch
        {
            WebhookCallStatus.Success => Color.Success,
            WebhookCallStatus.Error => Color.Error,
            _ => Color.Default
        };
    }
}
