namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_student_enrollment()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2;
        await academicClient.CreateEnrollmentPeriod(period.Id);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        TeacherOut ana = await academicClient.CreateTeacher("Ana");

        ClassOut discreteMathClass = await academicClient.CreateClass(data.Disciplines.DiscreteMath.Id, chico.Id, period.Id, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);
        ClassOut introToWebDevClass = await academicClient.CreateClass(data.Disciplines.IntroToWebDev.Id, chico.Id, period.Id, 40, [new(Day.Tuesday, Hour.H07_00, Hour.H10_00)]);
        ClassOut introToComputerNetworksClass = await academicClient.CreateClass(data.Disciplines.IntroToComputerNetworks.Id, ana.Id, period.Id, 40, [new(Day.Wednesday, Hour.H07_00, Hour.H10_00)]);

        StudentOut student = await academicClient.CreateStudent(data.CourseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);

        // Act
        await studentClient.CreateStudentEnrollment([discreteMathClass.Id, introToComputerNetworksClass.Id]);

        // Assert
        var classes = await studentClient.GetStudentEnrollmentClasses();

        classes.Should().HaveCount(3);
        classes.First(x => x.Id == discreteMathClass.Id).IsSelected.Should().BeTrue();
        classes.First(x => x.Id == introToComputerNetworksClass.Id).IsSelected.Should().BeTrue();
        classes.First(x => x.Id == introToWebDevClass.Id).IsSelected.Should().BeFalse();
    }
}
