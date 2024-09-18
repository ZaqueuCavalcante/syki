namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_student_frequencies_with_zero_values()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);

        // Act
        var response = await studentClient.GetStudentFrequencies();

        // Assert
        var frequencies = response.GetSuccess();
        frequencies.Should().HaveCount(1);
        frequencies[0].Should().BeEquivalentTo(new GetStudentFrequenciesOut("Total do curso", "-", 0, 0));
    }

    [Test]
    public async Task Should_return_student_frequencies_for_the_first_lesson_attendance()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        TeacherOut teacher = await academicClient.CreateTeacher();
        ClassOut discreteMathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, teacher.Id, data.AcademicPeriod2.Id, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);
        ClassOut introToWebDevClass = await academicClient.CreateClass(data.AdsDisciplines.IntroToWebDev.Id, teacher.Id, data.AcademicPeriod2.Id, 45, [new(Day.Tuesday, Hour.H07_00, Hour.H10_00)]);
        await academicClient.CreateClassLessons(discreteMathClass.Id);
        await academicClient.CreateClassLessons(introToWebDevClass.Id);

        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([discreteMathClass.Id, introToWebDevClass.Id]);

        var teacherClient = await _back.LoggedAsTeacher(teacher.Email);
        var teacherMathClass = await teacherClient.GetTeacherClass(discreteMathClass.Id);
        var firstLesson = teacherMathClass.Lessons.First();

        await teacherClient.CreateLessonAttendance(firstLesson.Id, [student.Id]);

        // Act
        var response = await studentClient.GetStudentFrequencies();

        // Assert
        var frequencies = response.GetSuccess();
        frequencies.Should().HaveCount(2);
        frequencies[0].Should().BeEquivalentTo(new GetStudentFrequenciesOut("Total do curso", "-", 1, 1));
        frequencies[1].Should().BeEquivalentTo(new GetStudentFrequenciesOut(data.AdsDisciplines.DiscreteMath.Name, "1", 1, 1));
    }

    [Test]
    public async Task Should_return_student_frequencies_for_the_first_period_lesson_attendances()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        TeacherOut teacher = await academicClient.CreateTeacher();
        ClassOut discreteMath = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, teacher.Id, data.AcademicPeriod1.Id, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);
        ClassOut introToWebDev = await academicClient.CreateClass(data.AdsDisciplines.IntroToWebDev.Id, teacher.Id, data.AcademicPeriod1.Id, 45, [new(Day.Tuesday, Hour.H07_00, Hour.H10_00)]);
        ClassOut humanMachineInteractionDesign = await academicClient.CreateClass(data.AdsDisciplines.HumanMachineInteractionDesign.Id, teacher.Id, data.AcademicPeriod1.Id, 45, [new(Day.Tuesday, Hour.H07_00, Hour.H10_00)]);
        ClassOut introToComputerNetworks = await academicClient.CreateClass(data.AdsDisciplines.IntroToComputerNetworks.Id, teacher.Id, data.AcademicPeriod1.Id, 45, [new(Day.Tuesday, Hour.H07_00, Hour.H10_00)]);
        ClassOut computationalThinkingAndAlgorithms = await academicClient.CreateClass(data.AdsDisciplines.ComputationalThinkingAndAlgorithms.Id, teacher.Id, data.AcademicPeriod1.Id, 45, [new(Day.Tuesday, Hour.H07_00, Hour.H10_00)]);
        ClassOut integratorProjectOne = await academicClient.CreateClass(data.AdsDisciplines.IntegratorProjectOne.Id, teacher.Id, data.AcademicPeriod1.Id, 45, [new(Day.Tuesday, Hour.H07_00, Hour.H10_00)]);
        await academicClient.CreateClassLessons(discreteMath.Id);
        await academicClient.CreateClassLessons(introToWebDev.Id);
        await academicClient.CreateClassLessons(humanMachineInteractionDesign.Id);
        await academicClient.CreateClassLessons(introToComputerNetworks.Id);
        await academicClient.CreateClassLessons(computationalThinkingAndAlgorithms.Id);
        await academicClient.CreateClassLessons(integratorProjectOne.Id);

        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([
            discreteMath.Id,
            introToWebDev.Id,
            humanMachineInteractionDesign.Id,
            introToComputerNetworks.Id,
            computationalThinkingAndAlgorithms.Id,
            integratorProjectOne.Id
        ]);

        var teacherClient = await _back.LoggedAsTeacher(teacher.Email);
        var teacherDiscreteMath = await teacherClient.GetTeacherClass(discreteMath.Id);
        var teacherIntroToWebDev = await teacherClient.GetTeacherClass(introToWebDev.Id);
        var teacherHumanMachineInteractionDesign = await teacherClient.GetTeacherClass(humanMachineInteractionDesign.Id);
        var teacherIntroToComputerNetworks = await teacherClient.GetTeacherClass(introToComputerNetworks.Id);
        var teacherComputationalThinkingAndAlgorithms = await teacherClient.GetTeacherClass(computationalThinkingAndAlgorithms.Id);
        var teacherIntegratorProjectOne = await teacherClient.GetTeacherClass(integratorProjectOne.Id);

        var lessons = new List<LessonOut>();
        lessons.AddRange(teacherHumanMachineInteractionDesign.Lessons.PickRandom(1));
        lessons.AddRange(teacherIntroToComputerNetworks.Lessons.PickRandom(3));
        lessons.AddRange(teacherIntroToWebDev.Lessons.PickRandom(2));
        lessons.AddRange(teacherDiscreteMath.Lessons.PickRandom(3));
        lessons.AddRange(teacherComputationalThinkingAndAlgorithms.Lessons.PickRandom(2));
        lessons.AddRange(teacherIntegratorProjectOne.Lessons.PickRandom(1));
        for (var i = 0; i < lessons.Count; i++)
        {
            List<Guid> presences = new List<int>{ 0, 2, 3, 5, 6, 8, 10  }.Contains(i) ? [student.Id] : [];
            await teacherClient.CreateLessonAttendance(lessons[i].Id, presences);
        }

        // Act
        var response = await studentClient.GetStudentFrequencies();

        // Assert
        var frequencies = response.GetSuccess();
        frequencies.Should().HaveCount(7);
        frequencies[0].Should().BeEquivalentTo(new GetStudentFrequenciesOut("Total do curso", "-", 12, 7));
        frequencies[1].Should().BeEquivalentTo(new GetStudentFrequenciesOut(data.AdsDisciplines.HumanMachineInteractionDesign.Name, "1", 1, 1));
        frequencies[2].Should().BeEquivalentTo(new GetStudentFrequenciesOut(data.AdsDisciplines.IntroToComputerNetworks.Name, "1", 3, 2));
        frequencies[3].Should().BeEquivalentTo(new GetStudentFrequenciesOut(data.AdsDisciplines.IntroToWebDev.Name, "1", 2, 1));
        frequencies[4].Should().BeEquivalentTo(new GetStudentFrequenciesOut(data.AdsDisciplines.DiscreteMath.Name, "1", 3, 2));
        frequencies[5].Should().BeEquivalentTo(new GetStudentFrequenciesOut(data.AdsDisciplines.ComputationalThinkingAndAlgorithms.Name, "1", 2, 1));
        frequencies[6].Should().BeEquivalentTo(new GetStudentFrequenciesOut(data.AdsDisciplines.IntegratorProjectOne.Name, "1", 1, 0));
    }
}
