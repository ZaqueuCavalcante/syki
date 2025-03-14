using MudBlazor;

namespace Syki.Front.Features.Adm.GetBatchesSummary;

public static class CommandBatchStatusMapper
{
    public static string GetIcon(this CommandBatchStatus status)
    {
        return status switch
        {
            CommandBatchStatus.Pending => Icons.Material.Rounded.WarningAmber,
            CommandBatchStatus.Processing => Icons.Material.Filled.Autorenew,
            CommandBatchStatus.Success => Icons.Material.Filled.CheckCircleOutline,
            CommandBatchStatus.Error => Icons.Material.Filled.ErrorOutline,
            _ => ""
        };
    }

    public static Color GetColor(this CommandBatchStatus status)
    {
        return status switch
        {
            CommandBatchStatus.Pending => Color.Warning,
            CommandBatchStatus.Processing => Color.Info,
            CommandBatchStatus.Success => Color.Success,
            CommandBatchStatus.Error => Color.Error,
            _ => Color.Default
        };
    }
}
