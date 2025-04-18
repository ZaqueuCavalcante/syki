using MudBlazor;

namespace Syki.Front.Features.Student.GetStudentClass;

public static class StudentClassActivityOutMapper
{
    public static Color GetColor(this StudentClassActivityOut activity)
    {
        if (activity.WorkStatus == ClassActivityWorkStatus.Pending) return Color.Error;
        if (activity.WorkStatus == ClassActivityWorkStatus.Delivered) return Color.Info;
        return Color.Success;
    }
}
