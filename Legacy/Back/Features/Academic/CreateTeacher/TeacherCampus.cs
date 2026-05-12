namespace Syki.Back.Features.Academic.CreateTeacher;

/// <summary>
/// Vínculo entre professor e campus
/// </summary>
public class TeacherCampus
{
    public Guid SykiTeacherId { get; set; }
    public Guid CampusId { get; set; }
}
