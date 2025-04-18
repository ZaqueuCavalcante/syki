using MudBlazor;

namespace Syki.Front.Features.Student.GetStudentClassFrequency;

public static class GetStudentClassFrequencyOutMapper
{
    public static Color GetFrequencyColor(this GetStudentClassFrequencyOut frequency)
    {
        return frequency.Frequency >= 70 ? Color.Tertiary : Color.Error;
    }
}
