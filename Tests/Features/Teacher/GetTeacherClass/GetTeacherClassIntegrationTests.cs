namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_teacher_class()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;
        var math = data.AdsDisciplines.DiscreteMath;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        await academicClient.AssignCampiToTeacher(chico.Id, [data.Campus.Id]);
        await academicClient.AssignDisciplinesToTeacher(chico.Id, [math.Id]);
        ClassOut mathClass = await academicClient.CreateClass(math.Id, data.Campus.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");

        await academicClient.ReleaseClassesForEnrollment([mathClass.Id]);

        var studentClient = await _api.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([ mathClass.Id ]);

        await academicClient.UpdateEnrollmentPeriod(period, -2, -1);
        await academicClient.StartClasses([mathClass.Id]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        // Act
        TeacherClassOut @class = await teacherClient.GetTeacherClass(mathClass.Id);

        // Assert
        @class.Code.Should().Be(math.Code);
        @class.Period.Should().Be(period);
        @class.Discipline.Should().Be(math.Name);
        @class.Status.Should().Be(ClassStatus.Started);
    }

    [Test]
    public async Task Should_not_return_teacher_class_when_not_found()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;
        var math = data.AdsDisciplines.DiscreteMath;

        TeacherOut chico = await academicClient.CreateTeacher();
        await academicClient.CreateClass(math.Id, data.Campus.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        // Act
        var response = await teacherClient.GetTeacherClass(Guid.CreateVersion7());

        // Assert
        response.ShouldBeError(new ClassNotFound());
    }
}
