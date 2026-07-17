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

    /// <summary>
    /// Se a turma não for presencial, o campus será nulo
    /// </summary>
    public int? CampusId { get; set; }

    /// <summary>
    /// Professores que lecionam na turma
    /// </summary>
    public List<EstudTeacher> Teachers { get; set; }

    public List<Schedule> Schedules { get; set; }
    public List<ClassLesson> Lessons { get; set; }
    public List<EstudStudent> Students { get; set; }

    private Class() {}

    public Class(
        int institutionId,
        int disciplineId,
        int periodId,
        int vacancies,
        int? campusId
    ) {
        InstitutionId = institutionId;
        DisciplineId = disciplineId;
        PeriodId = periodId;
        Vacancies = vacancies;
        CampusId = campusId;
        Status = ClassStatus.OnPreEnrollment;
        Teachers = [];
        Schedules = [];
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
