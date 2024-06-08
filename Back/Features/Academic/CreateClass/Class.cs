using Syki.Back.Features.Academic.StartClass;
using Syki.Back.Features.Academic.CreateTeacher;
using Syki.Back.Features.Academic.CreateStudent;
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
    public int Vacancies { get; set; }
    public ClassStatus Status { get; set; }
    public List<SykiStudent> Students { get; set; }
    public List<Schedule> Schedules { get; set; }
    public List<ExamGrade> ExamGrades { get; set; }

    public string FillRatio { get; set; }

    private Class() {}

    public Class(
        Guid institutionId,
        Guid disciplineId,
        Guid teacherId,
        string period,
        int vacancies,
        List<Schedule> schedules
    ) {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
        DisciplineId = disciplineId;
        TeacherId = teacherId;
        Period = period;
        Vacancies = vacancies;
        Status = ClassStatus.OnEnrollmentPeriod;
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
        return string.Join(" | ", Schedules.OrderBy(h => h.Day).ThenBy(h => h.StartAt).ToList().ConvertAll(h => h.ToString()));
    }

    public void Start()
    {
        ExamGrades = [];
        foreach (var student in Students)
        {
            ExamGrades.Add(new(Id, student.Id, ExamType.N1));
            ExamGrades.Add(new(Id, student.Id, ExamType.N2));
            ExamGrades.Add(new(Id, student.Id, ExamType.Final));
        }
        Status = ClassStatus.Started;
    }

    public ClassOut ToOut()
    {
        return new ClassOut
        {
            Id = Id,
            Discipline = Discipline.Name,
            Teacher = Teacher.Name,
            Period = Period,
            Vacancies = Vacancies,
            Status = Status,
            Schedules = Schedules.ConvertAll(h => h.ToOut()),
            SchedulesInline = GetScheduleAsString(),
            FillRatio = FillRatio,
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
