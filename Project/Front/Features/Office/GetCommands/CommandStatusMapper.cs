using MudBlazor;

namespace Exato.Front.Features.Office.GetCommands;

public static class CommandStatusMapper
{
    public static string GetIcon(this CommandStatus status)
    {
        return status switch
        {
            CommandStatus.Pending => Icons.Material.Rounded.WarningAmber,
            CommandStatus.Awaiting => Icons.Material.Filled.HourglassTop,
            CommandStatus.Processing => Icons.Material.Filled.Autorenew,
            CommandStatus.Success => Icons.Material.Filled.CheckCircleOutline,
            CommandStatus.Error => Icons.Material.Filled.ErrorOutline,
            _ => ""
        };
    }

    public static Color GetColor(this CommandStatus status)
    {
        return status switch
        {
            CommandStatus.Pending => Color.Warning,
            CommandStatus.Awaiting => Color.Default,
            CommandStatus.Processing => Color.Info,
            CommandStatus.Success => Color.Success,
            CommandStatus.Error => Color.Error,
            _ => Color.Default
        };
    }
}
