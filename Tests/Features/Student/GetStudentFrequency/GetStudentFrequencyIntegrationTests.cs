namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_student_frequency_when_has_no_lesson_attendances()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Frequency.Should().Be(0.00M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_V()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _back);

        var teacherClient = await _back.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _back.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, [data.Student.Id]);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Frequency.Should().Be(100.00M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_X()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _back);

        var teacherClient = await _back.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _back.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, []);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Frequency.Should().Be(0.00M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_VV()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _back);

        var teacherClient = await _back.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _back.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, [data.Student.Id]);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Frequency.Should().Be(100.00M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_VX()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _back);

        var teacherClient = await _back.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _back.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, []);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Frequency.Should().Be(50.00M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_XV()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _back);

        var teacherClient = await _back.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _back.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, [data.Student.Id]);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Frequency.Should().Be(50.00M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_XX()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _back);

        var teacherClient = await _back.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _back.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, []);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Frequency.Should().Be(0.00M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_VVV()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _back);

        var teacherClient = await _back.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _back.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[2].Id, [data.Student.Id]);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Frequency.Should().Be(100.00M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_VVX()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _back);

        var teacherClient = await _back.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _back.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[2].Id, []);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Frequency.Should().Be(66.67M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_VXV()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _back);

        var teacherClient = await _back.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _back.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[2].Id, [data.Student.Id]);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Frequency.Should().Be(66.67M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_VXX()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _back);

        var teacherClient = await _back.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _back.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[2].Id, []);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Frequency.Should().Be(33.33M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_XVV()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _back);

        var teacherClient = await _back.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _back.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[2].Id, [data.Student.Id]);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Frequency.Should().Be(66.67M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_XVX()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _back);

        var teacherClient = await _back.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _back.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[2].Id, []);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Frequency.Should().Be(33.33M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_XXV()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _back);

        var teacherClient = await _back.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _back.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[2].Id, [data.Student.Id]);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Frequency.Should().Be(33.33M);
    }

    [Test]
    public async Task Should_return_student_frequency_for_sequence_XXX()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _back);

        var teacherClient = await _back.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _back.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, []);
        await teacherClient.CreateLessonAttendance(lessons[2].Id, []);

        // Act
        var response = await studentClient.GetStudentFrequency();

        // Assert
        response.Frequency.Should().Be(0.00M);
    }
}
