using Estud.Back.Domain.Periods;
using Estud.Back.Domain.Teachers;
using Estud.Back.Domain.Students;
using Estud.Back.Domain.Disciplines;

namespace Estud.Back.Domain.Classes;

/// <summary>
/// Turma
/// </summary>
public class Class
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }

    public int DisciplineId { get; set; }
    public Discipline Discipline { get; set; }

    public int PeriodId { get; set; }
    public AcademicPeriod Period { get; set; }

    public int Vacancies { get; set; }
    public ClassStatus Status { get; set; }
    public int Workload { get; set; }

    public int? CampusId { get; set; }

    public int? TeacherId { get; set; }
    public EstudTeacher Teacher { get; set; }

    public List<Schedule> Schedules { get; set; }
    public List<ClassLesson> Lessons { get; set; }
    public List<EstudStudent> Students { get; set; }

    private Class() {}

    public Class(
        int institutionId,
        int disciplineId,
        int? campusId,
        int? teacherId,
        AcademicPeriod period,
        int vacancies,
        List<Schedule> schedules
    ) {
        InstitutionId = institutionId;
        DisciplineId = disciplineId;
        CampusId = campusId;
        TeacherId = teacherId;
        Period = period;
        Vacancies = vacancies;
        Status = ClassStatus.OnPreEnrollment;
        Schedules = schedules;
        Lessons = [];
        Students = [];
    }

    public void CreateLessons()
    {
        var schedules = Schedules.OrderBy(x => x.Day).ThenBy(x => x.Start).ToList();

        var number = 1;
        var current = Period.StartAt;
        while (current < Period.EndAt)
        {
            foreach (var schedule in schedules)
            {
                if (current.DayOfWeek.Is(schedule.Day))
                {
                    Lessons.Add(new(this, number, current, schedule.Start, schedule.End));
                    Workload += schedule.GetDiff();
                    number++;
                }
            }
            current = current.AddDays(1);
        }
    }
}
