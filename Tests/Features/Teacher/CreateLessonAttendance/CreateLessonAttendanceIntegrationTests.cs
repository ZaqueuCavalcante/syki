namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_lesson_attendance_for_empty_class()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();

        var campus = await academicClient.CreateCampus();
        var period = await academicClient.CreateCurrentAcademicPeriod();
        var discipline = await academicClient.CreateDiscipline();
        TeacherOut teacher = await academicClient.CreateTeacher();
        await academicClient.AssignCampiToTeacher(teacher.Id, [campus.Id]);
        await academicClient.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);
        var schedules = new List<ScheduleIn>() { new(Day.Tuesday, Hour.H19_00, Hour.H22_00) };

        ClassOut @class = await academicClient.CreateClass(discipline.Id, campus.Id, teacher.Id, period.Id, 40, schedules);

        var teacherClient = await _api.LoggedAsTeacher(teacher.Email);
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
        var academicClient = await _api.LoggedAsAcademic();

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        // Act
        var response = await teacherClient.CreateLessonAttendance(Guid.CreateVersion7(), []);

        // Assert
        response.ShouldBeError(new LessonNotFound());
    }

    [Test]
    public async Task Should_not_create_lesson_attendance_when_lesson_is_not_linked_with_the_class()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();

        var campus = await academicClient.CreateCampus();
        var period = await academicClient.CreateCurrentAcademicPeriod();
        var discipline = await academicClient.CreateDiscipline(); 
        var schedules = new List<ScheduleIn>() { new(Day.Tuesday, Hour.H19_00, Hour.H22_00) };

        TeacherOut teacher1 = await academicClient.CreateTeacher();
        await academicClient.AssignCampiToTeacher(teacher1.Id, [campus.Id]);
        await academicClient.AssignDisciplinesToTeacher(teacher1.Id, [discipline.Id]);
        ClassOut class1 = await academicClient.CreateClass(discipline.Id, campus.Id, teacher1.Id, period.Id, 40, schedules);

        TeacherOut teacher2 = await academicClient.CreateTeacher();
        await academicClient.AssignCampiToTeacher(teacher2.Id, [campus.Id]);
        await academicClient.AssignDisciplinesToTeacher(teacher2.Id, [discipline.Id]);
        ClassOut class2 = await academicClient.CreateClass(discipline.Id, campus.Id, teacher2.Id, period.Id, 40, schedules);

        var teacherClient = await _api.LoggedAsTeacher(teacher1.Email);
        GetAcademicClassOut classDb = await academicClient.GetAcademicClass(class2.Id);
        var lessonId = classDb.Lessons[0].Id;

        // Act
        var response = await teacherClient.CreateLessonAttendance(lessonId, []);

        // Assert
        response.ShouldBeError(new ClassNotFound());
    }

    [Test]
    public async Task Should_not_create_lesson_attendance_when_student_list_is_invalid()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();

        var campus = await academicClient.CreateCampus();
        var period = await academicClient.CreateCurrentAcademicPeriod();
        var discipline = await academicClient.CreateDiscipline(); 
        var schedules = new List<ScheduleIn>() { new(Day.Tuesday, Hour.H19_00, Hour.H22_00) };

        TeacherOut teacher = await academicClient.CreateTeacher();
        await academicClient.AssignCampiToTeacher(teacher.Id, [campus.Id]);
        await academicClient.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);
        ClassOut @class = await academicClient.CreateClass(discipline.Id, campus.Id, teacher.Id, period.Id, 40, schedules);

        var teacherClient = await _api.LoggedAsTeacher(teacher.Email);
        GetAcademicClassOut classDb = await academicClient.GetAcademicClass(@class.Id);
        var lessonId = classDb.Lessons[0].Id;

        // Act
        var response = await teacherClient.CreateLessonAttendance(lessonId, [Guid.CreateVersion7()]);

        // Assert
        response.ShouldBeError(new InvalidStudentsList());
    }
}
