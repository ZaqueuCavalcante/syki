namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_lesson_attendance_for_empty_class()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();

        var discipline = await academicClient.CreateDiscipline();
        var start = new DateOnly(2024, 08, 12);
        var end = new DateOnly(2024, 08, 30);
        AcademicPeriodOut period = await academicClient.CreateAcademicPeriod("2024.1", start, end);
        var schedules = new List<ScheduleIn>() { new(Day.Tuesday, Hour.H19_00, Hour.H22_00) };

        TeacherOut teacher = await academicClient.CreateTeacher();
        ClassOut @class = await academicClient.CreateClass(discipline.Id, teacher.Id, period.Id, 40, schedules);
        await academicClient.CreateClassLessons(@class.Id);

        var teacherClient = await _back.LoggedAsTeacher(teacher.Email);
        GetAcademicClassOut classDb = await academicClient.GetAcademicClass(@class.Id);
        var lessonId = classDb.Lessons[0].Id;

        // Act
        var response = await teacherClient.CreateLessonAttendance(lessonId, []);

        // Assert
        response.ShouldBeSuccess();
    }

    [Test]
    public async Task Should_not_create_lesson_attendance_when_lesson_not_exists()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        var teacherClient = await _back.LoggedAsTeacher(chico.Email);

        // Act
        var response = await teacherClient.CreateLessonAttendance(Guid.NewGuid(), []);

        // Assert
        response.ShouldBeError(new LessonNotFound());
    }

    [Test]
    public async Task Should_not_create_lesson_attendance_when_lesson_is_not_linked_with_the_class()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();

        var discipline = await academicClient.CreateDiscipline();
        var start = new DateOnly(2024, 08, 12);
        var end = new DateOnly(2024, 08, 30);
        AcademicPeriodOut period = await academicClient.CreateAcademicPeriod("2024.1", start, end);
        var schedules = new List<ScheduleIn>() { new(Day.Tuesday, Hour.H19_00, Hour.H22_00) };

        TeacherOut teacher1 = await academicClient.CreateTeacher();
        ClassOut class1 = await academicClient.CreateClass(discipline.Id, teacher1.Id, period.Id, 40, schedules);
        await academicClient.CreateClassLessons(class1.Id);

        TeacherOut teacher2 = await academicClient.CreateTeacher();
        ClassOut class2 = await academicClient.CreateClass(discipline.Id, teacher2.Id, period.Id, 40, schedules);
        await academicClient.CreateClassLessons(class2.Id);

        var teacherClient = await _back.LoggedAsTeacher(teacher1.Email);
        GetAcademicClassOut classDb = await academicClient.GetAcademicClass(class2.Id);
        var lessonId = classDb.Lessons[0].Id;

        // Act
        var response = await teacherClient.CreateLessonAttendance(lessonId, []);

        // Assert
        response.ShouldBeError(new LessonNotFound());
    }

    [Test]
    public async Task Should_not_create_lesson_attendance_when_student_list_is_invalid()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();

        var discipline = await academicClient.CreateDiscipline();
        var start = new DateOnly(2024, 08, 12);
        var end = new DateOnly(2024, 08, 30);
        AcademicPeriodOut period = await academicClient.CreateAcademicPeriod("2024.1", start, end);
        var schedules = new List<ScheduleIn>() { new(Day.Tuesday, Hour.H19_00, Hour.H22_00) };

        TeacherOut teacher = await academicClient.CreateTeacher();
        ClassOut @class = await academicClient.CreateClass(discipline.Id, teacher.Id, period.Id, 40, schedules);
        await academicClient.CreateClassLessons(@class.Id);

        var teacherClient = await _back.LoggedAsTeacher(teacher.Email);
        GetAcademicClassOut classDb = await academicClient.GetAcademicClass(@class.Id);
        var lessonId = classDb.Lessons[0].Id;

        // Act
        var response = await teacherClient.CreateLessonAttendance(lessonId, [Guid.NewGuid()]);

        // Assert
        response.ShouldBeError(new InvalidStudentsList());
    }

    [Test]
    public async Task Should_not_create_lesson_attendance_when_lesson_is_in_future()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();

        var discipline = await academicClient.CreateDiscipline();
        var start = new DateOnly(2029, 08, 12);
        var end = new DateOnly(2029, 08, 30);
        AcademicPeriodOut period = await academicClient.CreateAcademicPeriod("2029.1", start, end);
        var schedules = new List<ScheduleIn>() { new(Day.Tuesday, Hour.H19_00, Hour.H22_00) };

        TeacherOut teacher = await academicClient.CreateTeacher();
        ClassOut @class = await academicClient.CreateClass(discipline.Id, teacher.Id, period.Id, 40, schedules);
        await academicClient.CreateClassLessons(@class.Id);

        var teacherClient = await _back.LoggedAsTeacher(teacher.Email);
        GetAcademicClassOut classDb = await academicClient.GetAcademicClass(@class.Id);
        var lessonId = classDb.Lessons[0].Id;

        // Act
        var response = await teacherClient.CreateLessonAttendance(lessonId, []);

        // Assert
        response.ShouldBeError(new InvalidLesson());
    }
}
