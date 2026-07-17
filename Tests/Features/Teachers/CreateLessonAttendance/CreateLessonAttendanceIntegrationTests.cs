using Estud.Tests.Integration.Clients;

namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Teachers_CreateLessonAttendance_Should_not_create_attendance_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateLessonAttendance(1, []);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Teachers_CreateLessonAttendance_Should_not_create_attendance_when_user_is_not_a_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateLessonAttendance(1, []);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Teachers_CreateLessonAttendance_Should_not_create_attendance_when_lesson_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateLessonAttendance(999999, []);

        // Assert
        result.ShouldBeError(ClassLessonNotFound.I);
    }

    [Test]
    public async Task Teachers_CreateLessonAttendance_Should_not_create_attendance_on_lesson_of_another_institution()
    {
        // Arrange
        var otherDirector = await _back.LoggedAsDirector();
        var otherClass = await CreateTeacherClassWithLessons(otherDirector, DataGen.Email);
        var lessonId = await GetFirstLessonId(otherClass);

        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateLessonAttendance(lessonId, []);

        // Assert
        result.ShouldBeError(ClassLessonNotFound.I);
    }

    [Test]
    public async Task Teachers_CreateLessonAttendance_Should_not_create_attendance_on_lesson_of_another_teacher()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateTeacher(DataGen.UserName, email);

        var @class = await CreateTeacherClassWithLessons(director, DataGen.Email);
        var lessonId = await GetFirstLessonId(@class);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.CreateLessonAttendance(lessonId, []);

        // Assert
        result.ShouldBeError(TeacherNotAssignedToClass.I);
    }

    [Test]
    public async Task Teachers_CreateLessonAttendance_Should_not_create_attendance_when_student_is_not_enrolled_in_class()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var @class = await CreateTeacherClassWithLessons(director, email);
        var lessonId = await GetFirstLessonId(@class);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.CreateLessonAttendance(lessonId, [999999]);

        // Assert
        result.ShouldBeError(InvalidStudentsList.I);
    }

    [Test]
    public async Task Teachers_CreateLessonAttendance_Should_not_create_attendance_when_student_is_duplicated()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var @class = await CreateTeacherClassWithLessons(director, email);
        var students = await EnrollStudentsInClass(director, @class, 1);
        var lessonId = await GetFirstLessonId(@class);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.CreateLessonAttendance(lessonId, [students[0], students[0]]);

        // Assert
        result.ShouldBeError(InvalidStudentsList.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Teachers_CreateLessonAttendance_Should_create_attendance()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var @class = await CreateTeacherClassWithLessons(director, email);
        var students = await EnrollStudentsInClass(director, @class, 2);
        var lessonId = await GetFirstLessonId(@class);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.CreateLessonAttendance(lessonId, [students[0]]);

        // Assert
        result.ShouldBeSuccess();

        await using var ctx = _back.GetDbContext();
        var attendances = await ctx.ClassLessonAttendances.Where(a => a.LessonId == lessonId).ToListAsync();
        attendances.Should().HaveCount(2);
        attendances.First(a => a.StudentId == students[0]).Present.Should().BeTrue();
        attendances.First(a => a.StudentId == students[1]).Present.Should().BeFalse();
        attendances.Should().AllSatisfy(a => a.ClassId.Should().Be(@class));

        var lesson = await ctx.ClassLessons.FirstAsync(l => l.Id == lessonId);
        lesson.Status.Should().Be(ClassLessonStatus.Finalized);
    }

    [Test]
    public async Task Teachers_CreateLessonAttendance_Should_create_attendance_for_class_without_students()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var @class = await CreateTeacherClassWithLessons(director, email);
        var lessonId = await GetFirstLessonId(@class);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.CreateLessonAttendance(lessonId, []);

        // Assert
        result.ShouldBeSuccess();

        await using var ctx = _back.GetDbContext();
        var attendances = await ctx.ClassLessonAttendances.Where(a => a.LessonId == lessonId).ToListAsync();
        attendances.Should().BeEmpty();

        var lesson = await ctx.ClassLessons.FirstAsync(l => l.Id == lessonId);
        lesson.Status.Should().Be(ClassLessonStatus.Finalized);
    }

    [Test]
    public async Task Teachers_CreateLessonAttendance_Should_update_attendance_when_lesson_is_called_again()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var @class = await CreateTeacherClassWithLessons(director, email);
        var students = await EnrollStudentsInClass(director, @class, 2);
        var lessonId = await GetFirstLessonId(@class);

        var client = await _back.LoginAs(email);
        await client.CreateLessonAttendance(lessonId, [students[0]]);

        // Act
        var result = await client.CreateLessonAttendance(lessonId, [students[1]]);

        // Assert
        result.ShouldBeSuccess();

        await using var ctx = _back.GetDbContext();
        var attendances = await ctx.ClassLessonAttendances.Where(a => a.LessonId == lessonId).ToListAsync();
        attendances.Should().HaveCount(2);
        attendances.First(a => a.StudentId == students[0]).Present.Should().BeFalse();
        attendances.First(a => a.StudentId == students[1]).Present.Should().BeTrue();
    }

    #endregion

    private async Task<int> CreateTeacherClassWithLessons(TestsHttpClient director, string teacherEmail)
    {
        var teacher = (await director.CreateTeacher(DataGen.UserName, teacherEmail)).Success;

        var discipline = (await director.CreateDiscipline()).Success;
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = (await director.CreateAcademicPeriod()).Success;
        var @class = (await director.CreateClass(discipline.Id, period.Id)).Success;

        return @class.Id;
    }

    private async Task<List<int>> EnrollStudentsInClass(TestsHttpClient director, int classId, int count)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await director.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await director.ReleaseClassForEnrollment(classId);

        List<int> students = [];
        for (var i = 0; i < count; i++)
        {
            var student = (await director.CreateStudent(DataGen.UserName, DataGen.Email)).Success;
            await director.AssignStudentToClass(student.Id, classId);
            students.Add(student.Id);
        }

        return students;
    }

    private async Task<int> GetFirstLessonId(int classId)
    {
        await using var ctx = _back.GetDbContext();
        return await ctx.ClassLessons
            .Where(l => l.ClassId == classId)
            .OrderBy(l => l.Number)
            .Select(l => l.Id)
            .FirstAsync();
    }
}
