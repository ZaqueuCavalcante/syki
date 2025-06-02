using MudBlazor;

namespace Syki.Front.Features.Academic.GetWebhookCall;

public static class WebhookCallAttemptStatusMapper
{
    public static string GetIcon(this WebhookCallAttemptStatus status)
    {
        return status switch
        {
            WebhookCallAttemptStatus.Success => Icons.Material.Filled.CheckCircleOutline,
            WebhookCallAttemptStatus.Error => Icons.Material.Filled.ErrorOutline,
            _ => ""
        };
    }

    public static Color GetColor(this WebhookCallAttemptStatus status)
    {
        return status switch
        {
            WebhookCallAttemptStatus.Success => Color.Success,
            WebhookCallAttemptStatus.Error => Color.Error,
            _ => Color.Default
        };
    }
}
