using Syki.Back.Features.Academic.CreateTeacher;
using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Academic.CreateLessons;
using Syki.Back.Features.Academic.CreateDiscipline;
using Syki.Back.Features.Academic.CreateAcademicPeriod;
using Syki.Back.Features.Student.CreateStudentEnrollment;

namespace Syki.Back.Features.Academic.CreateClass;

/// <summary>
/// Representa uma Turma.
/// </summary>
public class Class
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public Guid DisciplineId { get; set; }
    public Discipline Discipline { get; set; }
    public Guid TeacherId { get; set; }
    public SykiTeacher Teacher { get; set; }
    public string PeriodId { get; set; }
    public AcademicPeriod Period { get; set; }
    public int Vacancies { get; set; }
    public ClassStatus Status { get; set; }
    public int Workload { get; set; }
    public List<SykiStudent> Students { get; set; }
    public List<Schedule> Schedules { get; set; }
    public List<ExamGrade> ExamGrades { get; set; }
    public List<Lesson> Lessons { get; set; }

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
        PeriodId = period;
        Vacancies = vacancies;
        Status = ClassStatus.OnEnrollmentPeriod;
        Schedules = schedules;
        Students = [];
        Lessons = [];
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

        if (result.IsError()) return result.GetError();

        return new Class(institutionId, disciplineId, teacherId, period, vacancies, schedules);
    }

    public void CreateLessons()
    {
        var schedules = Schedules.OrderBy(x => x.Day).ThenBy(x => x.StartAt).ToList();

        var current = Period.StartAt;
        while (current < Period.EndAt)
        {
            foreach (var schedule in schedules)
            {
                if (current.DayOfWeek.Is(schedule.Day))
                {
                    Lessons.Add(new(Id, current, schedule.StartAt, schedule.EndAt));
                    Workload += schedule.GetDiff();
                }
            }
            current = current.AddDays(1);
        }
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

    private string GetWorkloadAsString()
    {
        return Workload.MinutesToString();
    }

    private string GetProgressAsString()
    {
        var total = Lessons.Count;
        var finalized = Lessons.Count(x => x.Status == LessonStatus.Finalized);
        return $"{finalized}/{total}";
    }

    public void Start()
    {
        Status = ClassStatus.Started;
    }

    public void SetFillRatio(int count)
    {
        FillRatio = $"{count}/{Vacancies}";
    }

    public ClassOut ToOut()
    {
        var presences = Lessons.Sum(l => l.Attendances.Count(a => a.Present));
        var attendances = Lessons.Sum(l => l.Attendances.Count);
        return new()
        {
            Id = Id,
            Discipline = Discipline.Name,
            Teacher = Teacher.Name,
            Period = PeriodId,
            Vacancies = Vacancies,
            Status = Status,
            Schedules = Schedules.ConvertAll(h => h.ToOut()),
            FillRatio = FillRatio,
            Frequency = attendances == 0 ? 0.00M : 100M * (1M * presences / (1M * attendances)),
        };
    }

    public GetAcademicClassOut ToGetAcademicClassOut()
    {
        var students = Students.ConvertAll(x => x.ToAcademicClassStudentOut());
        var lessons = Lessons.Count(x => x.Attendances.Count > 0);
        students.ForEach(s =>
        {
            var studentExamGrades = ExamGrades.Where(g => g.StudentId == s.Id).ToList();
            s.AverageNote = studentExamGrades.GetAverageNote();
            var presences = Lessons.Count(x => x.Attendances.Exists(a => a.StudentId == s.Id && a.Present));
            s.Frequency = lessons == 0 ? 0.00M : 100M * (1M * presences / (1M * lessons));
            s.ExamGrades = studentExamGrades.OrderBy(x => x.ExamType).Select(g => g.ToOut()).ToList();
        });

        return new()
        {
            Id = Id,
            Discipline = Discipline.Name,
            Code = Discipline.Code,
            Teacher = Teacher.Name,
            Period = PeriodId,
            Vacancies = Vacancies,
            Status = Status,
            Schedules = Schedules.ConvertAll(h => h.ToOut()),
            Lessons = Lessons.OrderBy(x => x.Date).ThenBy(x => x.StartAt).Select((l, i) => l.ToOut(i+1)).ToList(),
            SchedulesInline = GetScheduleAsString(),
            Workload = GetWorkloadAsString(),
            Progress = GetProgressAsString(),
            Students = students,
            FillRatio = FillRatio,
            Frequency = students.Count > 0 ? students.Average(s => s.Frequency) : 0.00M,
        };
    }

    public TeacherClassOut ToTeacherClassOut()
    {
        var students = Students.OrderBy(x => x.Name).Select(x => x.ToTeacherClassStudentOut()).ToList();
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
            Period = PeriodId,
            Status = Status,
            Students = students,
            Lessons = Lessons.OrderBy(x => x.Date).ThenBy(x => x.StartAt).Select((l, i) => l.ToOut(i+1)).ToList(),
        };
    }

    public TeacherClassesOut ToTeacherClassesOut()
    {
        return new()
        {
            Id = Id,
            Discipline = Discipline.Name,
            Code = Discipline.Code,
            Period = PeriodId,
            Schedules = Schedules.ConvertAll(h => h.ToOut()),
            SchedulesInline = GetScheduleAsString(),
        };
    }
}
