namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_student_agenda()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2;
        await academicClient.CreateEnrollmentPeriod(period.Id);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        TeacherOut ana = await academicClient.CreateTeacher("Ana");

        ClassOut discreteMathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period.Id, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);
        ClassOut humanMachineInteractionDesignClass = await academicClient.CreateClass(data.AdsDisciplines.HumanMachineInteractionDesign.Id, ana.Id, period.Id, 45, [new(Day.Tuesday, Hour.H07_00, Hour.H10_00)]);
        ClassOut introToComputerNetworksClass = await academicClient.CreateClass(data.AdsDisciplines.IntroToComputerNetworks.Id, ana.Id, period.Id, 40, [new(Day.Wednesday, Hour.H07_00, Hour.H10_00)]);

        await academicClient.ReleaseClassesForEnrollment(period.Id, [discreteMathClass.Id, humanMachineInteractionDesignClass.Id, introToComputerNetworksClass.Id]);

        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);

        await studentClient.CreateStudentEnrollment([discreteMathClass.Id, introToComputerNetworksClass.Id]);

        // Act
        var agenda = await studentClient.GetStudentAgenda();

        // Assert
        agenda.Should().HaveCount(2);
    }
}
