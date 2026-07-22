using Estud.Tests.Integration.Clients;

namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Students_GetStudentAttendanceCalendar_Should_not_get_calendar_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetStudentAttendanceCalendar(2026);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Students_GetStudentAttendanceCalendar_Should_not_get_calendar_when_user_is_not_a_student()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetStudentAttendanceCalendar(2026);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Students_GetStudentAttendanceCalendar_Should_not_get_calendar_when_user_is_a_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetStudentAttendanceCalendar(2026);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Students_GetStudentAttendanceCalendar_Should_get_all_days_of_the_year()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateStudent(DataGen.UserName, email);

        var client = await _back.LoginAs(email);

        // Act
        var calendar = (await client.GetStudentAttendanceCalendar(2026)).Success;

        // Assert
        calendar.Year.Should().Be(2026);
        calendar.Total.Should().Be(365);
        calendar.Items.Should().HaveCount(365);

        // Aluno sem turmas: todo dia é sem aula
        calendar.Items.Should().AllSatisfy(i => i.Status.Should().Be(StudentDayAttendanceStatus.NoClass));

        calendar.Items.First().Date.Should().Be(new DateTime(2026, 1, 1));
        calendar.Items.Last().Date.Should().Be(new DateTime(2026, 12, 31));
    }

    [Test]
    public async Task Students_GetStudentAttendanceCalendar_Should_use_current_year_when_year_is_not_informed()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateStudent(DataGen.UserName, email);

        var client = await _back.LoginAs(email);

        // Act
        var calendar = (await client.GetStudentAttendanceCalendar()).Success;

        // Assert
        calendar.Year.Should().Be(DateTime.UtcNow.Year);
    }

    [Test]
    public async Task Students_GetStudentAttendanceCalendar_Should_reflect_presence_absence_and_undefined_days()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var teacherEmail = DataGen.Email;
        var teacher = await director.CreateTeacher(DataGen.UserName, teacherEmail).Success();

        var discipline = await director.CreateDiscipline().Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = await director.CreateAcademicPeriod("2026.1", new DateOnly(2026, 01, 06), new DateOnly(2026, 12, 20)).Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();
        await director.UpdateClassTeachers(@class.Id, [teacher.Id]);
        await director.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await director.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await director.ReleaseClassForEnrollment(@class.Id);

        var studentEmail = DataGen.Email;
        var student = await director.CreateStudent(DataGen.UserName, studentEmail).Success();
        await director.AssignStudentToClass(student.Id, @class.Id);

        await director.StartClass(@class.Id);

        // Aulas do aluno (segundas-feiras de 2026), em ordem cronológica
        List<(int Id, DateOnly Date)> lessons;
        await using (var ctx = _back.GetDbContext())
        {
            var rows = await ctx.ClassLessons.AsNoTracking()
                .Where(l => l.ClassId == @class.Id)
                .OrderBy(l => l.Number)
                .Select(l => new { l.Id, l.Date })
                .ToListAsync();
            lessons = rows.Select(r => (r.Id, r.Date)).ToList();
        }

        var presentLesson = lessons[0];                          // segunda passada (12/01/2026)
        var absentLesson = lessons[1];                           // segunda passada (19/01/2026)
        var futureLesson = lessons.First(l => l.Date > today);   // primeira segunda futura

        // Professor lança a frequência das duas primeiras aulas
        var teacherClient = await _back.LoginAs(teacherEmail);
        await teacherClient.CreateLessonAttendance(presentLesson.Id, [student.Id]);   // presente
        await teacherClient.CreateLessonAttendance(absentLesson.Id, []);              // ausente

        var client = await _back.LoginAs(studentEmail);

        // Act
        var calendar = (await client.GetStudentAttendanceCalendar(2026)).Success;

        // Assert
        StudentDayAttendanceStatus StatusOf(DateOnly d) =>
            calendar.Items.First(i => i.Date == d.ToDateTime(TimeOnly.MinValue)).Status;

        calendar.Total.Should().Be(365);

        StatusOf(presentLesson.Date).Should().Be(StudentDayAttendanceStatus.Present);
        StatusOf(absentLesson.Date).Should().Be(StudentDayAttendanceStatus.Absent);
        StatusOf(futureLesson.Date).Should().Be(StudentDayAttendanceStatus.Undefined);

        // Feriado (Confraternização Universal) → sem aula
        StatusOf(new DateOnly(2026, 01, 01)).Should().Be(StudentDayAttendanceStatus.NoClass);

        // Domingo anterior a uma aula → sem aula
        StatusOf(presentLesson.Date.AddDays(-1)).Should().Be(StudentDayAttendanceStatus.NoClass);

        // Dia letivo (terça) em que o aluno não tem aula → sem aula
        StatusOf(presentLesson.Date.AddDays(1)).Should().Be(StudentDayAttendanceStatus.NoClass);
    }

    #endregion
}
