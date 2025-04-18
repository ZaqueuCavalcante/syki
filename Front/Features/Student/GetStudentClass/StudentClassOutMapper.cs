using MudBlazor;

namespace Syki.Front.Features.Student.GetStudentClass;

public static class StudentClassOutMapper
{
    public static string GetIcon(this StudentClassOut @class)
    {
        if (@class.Average < 3) return Icons.Material.Filled.ErrorOutline;
        if (@class.Average < 7) return Icons.Material.Rounded.WarningAmber;
        return Icons.Material.Filled.CheckCircleOutline;
    }

    public static Color GetColor(this StudentClassOut @class)
    {
        if (@class.Average < 3) return Color.Error;
        if (@class.Average < 7) return Color.Warning;
        return Color.Success;
    }
}
