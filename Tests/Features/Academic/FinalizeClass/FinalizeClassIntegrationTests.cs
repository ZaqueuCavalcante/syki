namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_finalize_class()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;

        foreach (var lesson in lessons)
        {
            await teacherClient.CreateLessonAttendance(lesson.Id, [data.Student.Id]);
        }

        // Act
        var response = await academicClient.FinalizeClasses([data.AdsClasses.HumanMachineInteractionDesign.Id]);

        // Assert
        response.ShouldBeSuccess();
    }

    [Test]
    public async Task Should_not_finalize_invalid_classes_list()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();

        // Act
        var response = await academicClient.FinalizeClasses([Guid.CreateVersion7()]);

        // Assert
        response.ShouldBeError(new InvalidClassesList());
    }

    [Test]
    public async Task Should_not_finalize_on_pre_enrollment_class()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        await academicClient.AssignCampiToTeacher(chico.Id, [data.Campus.Id]);
        await academicClient.AssignDisciplinesToTeacher(chico.Id, [data.AdsDisciplines.DiscreteMath.Id]);

        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, data.Campus.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        // Act
        var response = await academicClient.FinalizeClasses([mathClass.Id]);

        // Assert
        response.ShouldBeError(new ClassMustHaveStartedStatus());
    }

    [Test]
    public async Task Should_not_finalize_on_enrollment_class()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        await academicClient.AssignCampiToTeacher(chico.Id, [data.Campus.Id]);
        await academicClient.AssignDisciplinesToTeacher(chico.Id, [data.AdsDisciplines.DiscreteMath.Id]);

        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, data.Campus.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        await academicClient.ReleaseClassesForEnrollment([mathClass.Id]);

        // Act
        var response = await academicClient.FinalizeClasses([mathClass.Id]);

        // Assert
        response.ShouldBeError(new ClassMustHaveStartedStatus());
    }

    [Test]
    public async Task Should_not_finalize_classes_with_pending_lessons()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        // Act
        var response = await academicClient.FinalizeClasses([data.AdsClasses.HumanMachineInteractionDesign.Id]);

        // Assert
        response.ShouldBeError(new AllClassLessonsMustHaveFinalizedStatus());
    }
}
