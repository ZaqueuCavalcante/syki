using Syki.Back.Features.Academic.CreateTeacher;
using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Academic.CreateDiscipline;
using Syki.Back.Features.Student.CreateStudentEnrollment;

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
        Schedules = schedules;
    }

    public static OneOf<Class, SykiError> New(
        Guid institutionId,
        Guid disciplineId,
        Guid teacherId,
        string period,
        int vacancies,
        List<Schedule> schedules
    ) {
        var result = Validate(schedules);

        return result.Match<OneOf<Class, SykiError>>(
            _ => new Class(institutionId, disciplineId, teacherId, period, vacancies, schedules),
            error => error
        );
    }

    private static OneOf<SykiSuccess, SykiError> Validate(List<Schedule> schedules)
    {
        for (int i = 0; i < schedules.Count-1; i++)
        {
            for (int j = i+1; j < schedules.Count; j++)
            {
                if (schedules[i].Conflict(schedules[j]))
                    return new ConflictingSchedules();
            }
        }

        return new SykiSuccess();
    }

    private string GetScheduleAsString()
    {
        return string.Join(" | ", Schedules.OrderBy(h => h.Day).ThenBy(h => h.StartAt).ToList().ConvertAll(h => h.ToString()));
    }

    public void Start()
    {
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
        var students = Students.ConvertAll(x => x.ToTeacherClassStudentOut());
        students.ForEach(s =>
        {
            var studentExamGrades = ExamGrades.Where(g => g.StudentId == s.Id).ToList();
            s.AverageNote = studentExamGrades.GetAverageNote();
            s.ExamGrades = studentExamGrades.OrderBy(x => x.ExamType).Select(g => g.ToOut()).ToList();
        });

        return new()
        {
            Id = Id,
            Discipline = Discipline.Name,
            Code = Discipline.Code,
            Period = Period,
            Status = Status,
            Students = students,
        };
    }

    public TeacherClassesOut ToTeacherClassesOut()
    {
        return new TeacherClassesOut
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
