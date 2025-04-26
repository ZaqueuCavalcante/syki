namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_student_frequency_when_has_no_lesson_attendances()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Presences.Should().Be(0);
        response.Absences.Should().Be(0);
        response.Attendances.Should().Be(0);
        response.TotalLessons.Should().Be(128);
        response.Frequency.Should().Be(0.00M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_V()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.IntroToWebDev.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, [data.Student.Id]);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Presences.Should().Be(1);
        response.Absences.Should().Be(0);
        response.Attendances.Should().Be(1);
        response.TotalLessons.Should().Be(128);
        response.Frequency.Should().Be(100.00M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_X()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, []);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Presences.Should().Be(0);
        response.Absences.Should().Be(1);
        response.Attendances.Should().Be(1);
        response.TotalLessons.Should().Be(128);
        response.Frequency.Should().Be(0.00M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_VV()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, [data.Student.Id]);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Presences.Should().Be(2);
        response.Absences.Should().Be(0);
        response.Attendances.Should().Be(2);
        response.TotalLessons.Should().Be(128);
        response.Frequency.Should().Be(100.00M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_VX()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        var lessonsA = data.AdsClasses.IntroToWebDev.Lessons;
        var lessonsB = data.AdsClasses.DiscreteMath.Lessons;
        await teacherClient.CreateLessonAttendance(lessonsA[0].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessonsB[0].Id, []);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Presences.Should().Be(1);
        response.Absences.Should().Be(1);
        response.Attendances.Should().Be(2);
        response.TotalLessons.Should().Be(128);
        response.Frequency.Should().Be(50.00M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_XV()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, [data.Student.Id]);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Presences.Should().Be(1);
        response.Absences.Should().Be(1);
        response.Attendances.Should().Be(2);
        response.TotalLessons.Should().Be(128);
        response.Frequency.Should().Be(50.00M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_XX()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, []);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Presences.Should().Be(0);
        response.Absences.Should().Be(2);
        response.Attendances.Should().Be(2);
        response.TotalLessons.Should().Be(128);
        response.Frequency.Should().Be(0.00M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_VVV()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[2].Id, [data.Student.Id]);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Presences.Should().Be(3);
        response.Absences.Should().Be(0);
        response.Attendances.Should().Be(3);
        response.TotalLessons.Should().Be(128);
        response.Frequency.Should().Be(100.00M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_VVX()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[2].Id, []);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Presences.Should().Be(2);
        response.Absences.Should().Be(1);
        response.Attendances.Should().Be(3);
        response.TotalLessons.Should().Be(128);
        response.Frequency.Should().Be(66.67M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_VXV()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[2].Id, [data.Student.Id]);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Presences.Should().Be(2);
        response.Absences.Should().Be(1);
        response.Attendances.Should().Be(3);
        response.TotalLessons.Should().Be(128);
        response.Frequency.Should().Be(66.67M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_VXX()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[2].Id, []);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Presences.Should().Be(1);
        response.Absences.Should().Be(2);
        response.Attendances.Should().Be(3);
        response.TotalLessons.Should().Be(128);
        response.Frequency.Should().Be(33.33M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_XVV()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[2].Id, [data.Student.Id]);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Presences.Should().Be(2);
        response.Absences.Should().Be(1);
        response.Attendances.Should().Be(3);
        response.TotalLessons.Should().Be(128);
        response.Frequency.Should().Be(66.67M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_XVX()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[2].Id, []);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Presences.Should().Be(1);
        response.Absences.Should().Be(2);
        response.Attendances.Should().Be(3);
        response.TotalLessons.Should().Be(128);
        response.Frequency.Should().Be(33.33M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_XXV()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[2].Id, [data.Student.Id]);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Presences.Should().Be(1);
        response.Absences.Should().Be(2);
        response.Attendances.Should().Be(3);
        response.TotalLessons.Should().Be(128);
        response.Frequency.Should().Be(33.33M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_XXX()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[2].Id, []);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Presences.Should().Be(0);
        response.Absences.Should().Be(3);
        response.Attendances.Should().Be(3);
        response.TotalLessons.Should().Be(128);
        response.Frequency.Should().Be(0.00M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_many_classes()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        var lessonsA = data.AdsClasses.DiscreteMath.Lessons;
        await teacherClient.CreateLessonAttendance(lessonsA[0].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessonsA[1].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessonsA[2].Id, []);

        var lessonsB = data.AdsClasses.IntroToWebDev.Lessons;
        await teacherClient.CreateLessonAttendance(lessonsB[0].Id, []);
        await teacherClient.CreateLessonAttendance(lessonsB[1].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessonsB[2].Id, []);
        await teacherClient.CreateLessonAttendance(lessonsB[3].Id, []);

        var lessonsC = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessonsC[0].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessonsC[1].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessonsC[2].Id, []);
        await teacherClient.CreateLessonAttendance(lessonsC[3].Id, []);
        await teacherClient.CreateLessonAttendance(lessonsC[4].Id, []);
        await teacherClient.CreateLessonAttendance(lessonsC[5].Id, []);

        var lessonsD = data.AdsClasses.IntroToComputerNetworks.Lessons;
        await teacherClient.CreateLessonAttendance(lessonsD[0].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessonsD[1].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessonsD[2].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessonsD[3].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessonsD[4].Id, [data.Student.Id]);

        var lessonsE = data.AdsClasses.ComputationalThinkingAndAlgorithms.Lessons;
        await teacherClient.CreateLessonAttendance(lessonsE[0].Id, []);
        await teacherClient.CreateLessonAttendance(lessonsE[1].Id, []);
        await teacherClient.CreateLessonAttendance(lessonsE[2].Id, []);

        var lessonsF = data.AdsClasses.IntegratorProjectOne.Lessons;
        await teacherClient.CreateLessonAttendance(lessonsF[0].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessonsF[1].Id, []);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Presences.Should().Be(11);
        response.Absences.Should().Be(12);
        response.Attendances.Should().Be(23);
        response.TotalLessons.Should().Be(128);
        response.Frequency.Should().Be(47.83M);
    }
}
