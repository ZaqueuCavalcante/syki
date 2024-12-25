using MudBlazor;

namespace Syki.Front.Features.Adm.GetTasksSummary;

public static class SykiTaskStatusMapper
{
    public static string GetIcon(this SykiTaskStatus status)
    {
        return status switch
        {
            SykiTaskStatus.Pending => Icons.Material.Rounded.WarningAmber,
            SykiTaskStatus.Processing => Icons.Material.Filled.Autorenew,
            SykiTaskStatus.Success => Icons.Material.Filled.CheckCircleOutline,
            SykiTaskStatus.Error => Icons.Material.Filled.ErrorOutline,
            _ => ""
        };
    }

    public static Color GetColor(this SykiTaskStatus status)
    {
        return status switch
        {
            SykiTaskStatus.Pending => Color.Warning,
            SykiTaskStatus.Processing => Color.Info,
            SykiTaskStatus.Success => Color.Success,
            SykiTaskStatus.Error => Color.Error,
            _ => Color.Default
        };
    }
}
