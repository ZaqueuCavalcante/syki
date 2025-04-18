using MudBlazor;

namespace Syki.Front.Features.Student.GetStudentClass;

public static class StudentClassOutMapper
{
    public static string GetAverageIcon(this StudentClassOut @class)
    {
        if (@class.Average < 3) return Icons.Material.Filled.ErrorOutline;
        if (@class.Average < 7) return Icons.Material.Rounded.WarningAmber;
        return Icons.Material.Filled.CheckCircleOutline;
    }

    public static Color GetAverageColor(this StudentClassOut @class)
    {
        if (@class.Average < 3) return Color.Error;
        if (@class.Average < 7) return Color.Warning;
        return Color.Success;
    }

    public static string GetFrequencyIcon(this StudentClassOut @class)
    {
        if (@class.Frequency < 30) return Icons.Material.Filled.ErrorOutline;
        if (@class.Frequency < 70) return Icons.Material.Rounded.WarningAmber;
        return Icons.Material.Filled.CheckCircleOutline;
    }

    public static Color GetFrequencyColor(this StudentClassOut @class)
    {
        if (@class.Frequency < 30) return Color.Error;
        if (@class.Frequency < 70) return Color.Warning;
        return Color.Success;
    }
}
