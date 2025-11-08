using MudBlazor;

namespace Exato.Front.Features.Office.GetDomainEvents;

public static class DomainEventStatusMapper
{
    public static string GetIcon(this DomainEventStatus status)
    {
        return status switch
        {
            DomainEventStatus.Pending => Icons.Material.Rounded.WarningAmber,
            DomainEventStatus.Processing => Icons.Material.Filled.Autorenew,
            DomainEventStatus.Success => Icons.Material.Filled.CheckCircleOutline,
            DomainEventStatus.Error => Icons.Material.Filled.ErrorOutline,
            _ => ""
        };
    }

    public static Color GetColor(this DomainEventStatus status)
    {
        return status switch
        {
            DomainEventStatus.Pending => Color.Warning,
            DomainEventStatus.Processing => Color.Info,
            DomainEventStatus.Success => Color.Success,
            DomainEventStatus.Error => Color.Error,
            _ => Color.Default
        };
    }
}
