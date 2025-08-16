namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_assign_class_to_classroom_for_most_simple_case()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        var campus = await client.CreateCampus();
        var period = await client.CreateCurrentAcademicPeriod();
        var data = await client.CreateAdsCourseOffering(campusId: campus.Id, periodId: period.Id);
        TeacherOut teacher = await client.CreateTeacher();
        await client.AssignCampiToTeacher(teacher.Id, [campus.Id]);
        await client.AssignDisciplinesToTeacher(teacher.Id, [data.AdsDisciplines.Databases.Id]);

        ClassOut @class = await client.CreateClass(data.AdsDisciplines.Databases.Id, campus.Id, teacher.Id, period.Id, 5, []);
        CreateClassroomOut classroom = await client.CreateClassroom(campus.Id, "Sala 05", 25);

        // Act
        var response = await client.AssignClassToClassroom(classroom.Id, @class.Id, []);

        // Assert
        response.ShouldBeSuccess();
        var classrooms = await _api.GetDbContext().ClassroomsClasses.Where(x => x.ClassroomId == classroom.Id).ToListAsync();
        classrooms.Single().ClassId.Should().Be(@class.Id);
    }

    [Test]
    public async Task Should_not_assign_class_to_classroom_when_class_vacancies_are_greater_than_classroom_capacity()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        var campus = await client.CreateCampus();
        var period = await client.CreateCurrentAcademicPeriod();
        var data = await client.CreateAdsCourseOffering(campusId: campus.Id, periodId: period.Id);
        TeacherOut teacher = await client.CreateTeacher();
        await client.AssignCampiToTeacher(teacher.Id, [campus.Id]);
        await client.AssignDisciplinesToTeacher(teacher.Id, [data.AdsDisciplines.Databases.Id]);

        ClassOut @class = await client.CreateClass(data.AdsDisciplines.Databases.Id, campus.Id, teacher.Id, period.Id, 26, []);
        CreateClassroomOut classroom = await client.CreateClassroom(campus.Id, "Sala 05", 25);

        // Act
        var response = await client.AssignClassToClassroom(classroom.Id, @class.Id, []);

        // Assert
        response.ShouldBeError(new ClassVacanciesGreaterThanClassroomCapacity());
    }

    [Test]
    public async Task Should_not_assign_class_to_classroom_with_invalid_schedules()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        var campus = await client.CreateCampus();
        var period = await client.CreateCurrentAcademicPeriod();
        var data = await client.CreateAdsCourseOffering(campusId: campus.Id, periodId: period.Id);
        TeacherOut teacher = await client.CreateTeacher();
        await client.AssignCampiToTeacher(teacher.Id, [campus.Id]);
        await client.AssignDisciplinesToTeacher(teacher.Id, [data.AdsDisciplines.Databases.Id]);

        ClassOut @class = await client.CreateClass(data.AdsDisciplines.Databases.Id, campus.Id, teacher.Id, period.Id, 20, []);
        CreateClassroomOut classroom = await client.CreateClassroom(campus.Id, "Sala 05", 25);

        var schedules = new List<ScheduleIn>() { new(Day.Monday, Hour.H07_00, Hour.H07_00) };

        // Act
        var response = await client.AssignClassToClassroom(classroom.Id, @class.Id, schedules);

        // Assert
        response.ShouldBeError(new InvalidSchedule());
    }
}
