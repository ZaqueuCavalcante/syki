namespace Syki.Back.Domain.Teachers;

/// <summary>
/// Vínculo entre professor e campus
/// </summary>
public class TeacherCampus
{
    public int TeacherId { get; set; }
    public int CampusId { get; set; }
}
