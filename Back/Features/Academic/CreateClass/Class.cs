using Syki.Back.Features.Academic.CreateTeacher;
using Syki.Back.Features.Academic.CreateDiscipline;

namespace Syki.Back.Features.Academic.CreateClass;

public class Class
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public Guid DisciplineId { get; set; }
    public Discipline Discipline { get; set; }
    public Guid TeacherId { get; set; }
    public SykiTeacher Teacher { get; set; }
    public string Period { get; set; }
    public List<CreateStudent.Student> Students { get; set; }
    public List<Schedule> Schedules { get; set; }

    private Class() {}

    public Class(
        Guid institutionId,
        Guid disciplineId,
        Guid teacherId,
        string period,
        List<Schedule> schedules
    ) {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
        DisciplineId = disciplineId;
        TeacherId = teacherId;
        Period = period;
        SetSchedules(schedules);
    }

    private void SetSchedules(List<Schedule> schedules)
    {
        for (int i = 0; i < schedules.Count-1; i++)
        {
            for (int j = i+1; j < schedules.Count; j++)
            {
                if (schedules[i].Conflict(schedules[j]))
                    Throw.DE022.Now();
            }
        }

        Schedules = schedules;
    }

    public string GetScheduleAsString()
    {
        return string.Join(" | ", Schedules.OrderBy(h => h.Day).ThenBy(h => h.Start).ToList().ConvertAll(h => h.ToString()));
    }

    public ClassOut ToOut()
    {
        return new ClassOut
        {
            Id = Id,
            Discipline = Discipline.Name,
            Teacher = Teacher.Name,
            Period = Period,
            Schedules = Schedules.ConvertAll(h => h.ToOut()),
            SchedulesInline = GetScheduleAsString(),
        };
    }

    public TeacherClassOut ToTeacherClassOut()
    {
        return new TeacherClassOut
        {
            Id = Id,
            Discipline = Discipline.Name,
            Code = Discipline.Code,
            Period = Period,
            Schedules = Schedules.ConvertAll(h => h.ToOut()),
            SchedulesInline = GetScheduleAsString(),
        };
    }
}
