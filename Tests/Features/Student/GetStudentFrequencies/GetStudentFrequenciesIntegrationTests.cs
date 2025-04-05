namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_student_frequencies_with_zero_values()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // Act
        var response = await studentClient.GetStudentFrequencies();

        // Assert
        var frequencies = response.GetSuccess();
        frequencies.Should().HaveCount(0);
    }

    [Test]
    public async Task Should_return_student_frequencies_for_the_first_lesson_attendance()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut teacher = await academicClient.CreateTeacher();
        ClassOut discreteMathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, teacher.Id, period, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);
        ClassOut introToWebDevClass = await academicClient.CreateClass(data.AdsDisciplines.IntroToWebDev.Id, teacher.Id, period, 45, [new(Day.Tuesday, Hour.H07_00, Hour.H10_00)]);

        await academicClient.ReleaseClassesForEnrollment([discreteMathClass.Id]);

        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _api.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([discreteMathClass.Id, introToWebDevClass.Id]);

        await academicClient.UpdateEnrollmentPeriod(period, -2, -1);
        await academicClient.StartClasses([discreteMathClass.Id]);

        var teacherClient = await _api.LoggedAsTeacher(teacher.Email);
        var mathClassLessons = (await teacherClient.GetTeacherClassLessons(discreteMathClass.Id)).GetSuccess();
        var firstLesson = mathClassLessons.First();

        await teacherClient.CreateLessonAttendance(firstLesson.Id, [student.Id]);

        // Act
        var response = await studentClient.GetStudentFrequencies();

        // Assert
        var frequencies = response.GetSuccess();
        frequencies[0].Should().BeEquivalentTo(new GetStudentFrequenciesOut(data.AdsDisciplines.DiscreteMath.Name, "1", 1, 1, StudentDisciplineStatus.Matriculado));
    }

    [Test]
    public async Task Should_return_student_frequencies_for_the_first_period_lesson_attendances()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod1.Id;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut teacher = await academicClient.CreateTeacher();
        ClassOut humanMachineInteractionDesign = await academicClient.CreateClass(data.AdsDisciplines.HumanMachineInteractionDesign.Id, teacher.Id, period, 45, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);
        ClassOut introToComputerNetworks = await academicClient.CreateClass(data.AdsDisciplines.IntroToComputerNetworks.Id, teacher.Id, period, 45, [new(Day.Tuesday, Hour.H07_00, Hour.H10_00)]);
        ClassOut introToWebDev = await academicClient.CreateClass(data.AdsDisciplines.IntroToWebDev.Id, teacher.Id, period, 45, [new(Day.Wednesday, Hour.H07_00, Hour.H10_00)]);
        ClassOut discreteMath = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, teacher.Id, period, 40, [new(Day.Thursday, Hour.H07_00, Hour.H10_00)]);
        ClassOut computationalThinkingAndAlgorithms = await academicClient.CreateClass(data.AdsDisciplines.ComputationalThinkingAndAlgorithms.Id, teacher.Id, period, 45, [new(Day.Friday, Hour.H07_00, Hour.H10_00)]);
        ClassOut integratorProjectOne = await academicClient.CreateClass(data.AdsDisciplines.IntegratorProjectOne.Id, teacher.Id, period, 45, [new(Day.Saturday, Hour.H07_00, Hour.H10_00)]);

        await academicClient.ReleaseClassesForEnrollment([
            discreteMath.Id,
            introToWebDev.Id,
            humanMachineInteractionDesign.Id,
            introToComputerNetworks.Id,
            computationalThinkingAndAlgorithms.Id,
            integratorProjectOne.Id
        ]);

        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _api.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([
            discreteMath.Id,
            introToWebDev.Id,
            humanMachineInteractionDesign.Id,
            introToComputerNetworks.Id,
            computationalThinkingAndAlgorithms.Id,
            integratorProjectOne.Id
        ]);

        await academicClient.UpdateEnrollmentPeriod(period, -2, -1);
        await academicClient.StartClasses([discreteMath.Id]);
        await academicClient.StartClasses([introToWebDev.Id]);
        await academicClient.StartClasses([humanMachineInteractionDesign.Id]);
        await academicClient.StartClasses([introToComputerNetworks.Id]);
        await academicClient.StartClasses([computationalThinkingAndAlgorithms.Id]);
        await academicClient.StartClasses([integratorProjectOne.Id]);

        var teacherClient = await _api.LoggedAsTeacher(teacher.Email);
        var teacherDiscreteMathLessons = (await teacherClient.GetTeacherClassLessons(discreteMath.Id)).GetSuccess();
        var teacherIntroToWebDevLessons = (await teacherClient.GetTeacherClassLessons(introToWebDev.Id)).GetSuccess();
        var teacherHumanMachineInteractionDesignLessons = (await teacherClient.GetTeacherClassLessons(humanMachineInteractionDesign.Id)).GetSuccess();
        var teacherIntroToComputerNetworksLessons = (await teacherClient.GetTeacherClassLessons(introToComputerNetworks.Id)).GetSuccess();
        var teacherComputationalThinkingAndAlgorithmsLessons = (await teacherClient.GetTeacherClassLessons(computationalThinkingAndAlgorithms.Id)).GetSuccess();
        var teacherIntegratorProjectOneLessons = (await teacherClient.GetTeacherClassLessons(integratorProjectOne.Id)).GetSuccess();

        var lessons = new List<ClassLessonOut>();
        lessons.AddRange(teacherHumanMachineInteractionDesignLessons.PickRandom(1));
        lessons.AddRange(teacherIntroToComputerNetworksLessons.PickRandom(3));
        lessons.AddRange(teacherIntroToWebDevLessons.PickRandom(2));
        lessons.AddRange(teacherDiscreteMathLessons.PickRandom(3));
        lessons.AddRange(teacherComputationalThinkingAndAlgorithmsLessons.PickRandom(2));
        lessons.AddRange(teacherIntegratorProjectOneLessons.PickRandom(1));
        for (var i = 0; i < lessons.Count; i++)
        {
            List<Guid> presences = new List<int>{ 0, 2, 3, 5, 6, 8, 10 }.Contains(i) ? [student.Id] : [];
            var x = await teacherClient.CreateLessonAttendance(lessons[i].Id, presences);
            x.ShouldBeSuccess();
        }

        // Act
        var response = await studentClient.GetStudentFrequencies();

        // Assert
        var frequencies = response.GetSuccess();
        frequencies.Should().HaveCount(6);
        frequencies[0].Should().BeEquivalentTo(new GetStudentFrequenciesOut(data.AdsDisciplines.HumanMachineInteractionDesign.Name, "1", 1, 1, StudentDisciplineStatus.Matriculado));
        frequencies[1].Should().BeEquivalentTo(new GetStudentFrequenciesOut(data.AdsDisciplines.IntroToComputerNetworks.Name, "1", 3, 2, StudentDisciplineStatus.Matriculado));
        frequencies[2].Should().BeEquivalentTo(new GetStudentFrequenciesOut(data.AdsDisciplines.IntroToWebDev.Name, "1", 2, 1, StudentDisciplineStatus.Matriculado));
        frequencies[3].Should().BeEquivalentTo(new GetStudentFrequenciesOut(data.AdsDisciplines.DiscreteMath.Name, "1", 3, 2, StudentDisciplineStatus.Matriculado));
        frequencies[4].Should().BeEquivalentTo(new GetStudentFrequenciesOut(data.AdsDisciplines.ComputationalThinkingAndAlgorithms.Name, "1", 2, 1, StudentDisciplineStatus.Matriculado));
        frequencies[5].Should().BeEquivalentTo(new GetStudentFrequenciesOut(data.AdsDisciplines.IntegratorProjectOne.Name, "1", 1, 0, StudentDisciplineStatus.Matriculado));
    }
}
