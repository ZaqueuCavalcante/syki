namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_only_classes_of_student_course_curriculum()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var period = await client.CreateAcademicPeriod($"{DateTime.Now.Year}.1");
        await client.CreateEnrollmentPeriod(period.Id);

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        CourseOut ads = await client.CreateCourse("ADS");
        CourseOut direito = await client.CreateCourse("Direito");

        var matematica = await client.CreateDiscipline("Matemática Discreta", [ads.Id]);
        var bancoDeDados = await client.CreateDiscipline("Banco de Dados", [ads.Id]);
        var estruturaDeDados = await client.CreateDiscipline("Estrutura de Dados", [ads.Id]);
        var infoSociedade = await client.CreateDiscipline("Informática e Sociedade", [ads.Id, direito.Id]);
        var direitoEconomia = await client.CreateDiscipline("Direito e Economia", [direito.Id]);
        var introDireito = await client.CreateDiscipline("Introdução ao Direito", [direito.Id]);
        var direitoFinanceiro = await client.CreateDiscipline("Direito Financeiro", [direito.Id]);

        CourseCurriculumOut courseCurriculumAds = await client.CreateCourseCurriculum("Grade ADS 1.0", ads.Id,
        [
            new(matematica.Id, 1, 7, 73),
            new(bancoDeDados.Id, 1, 7, 73),
            new(estruturaDeDados.Id, 2, 7, 73),
            new(infoSociedade.Id, 2, 7, 73),
        ]);

        CourseCurriculumOut courseCurriculumDireito = await client.CreateCourseCurriculum("Grade Direito 1.0", direito.Id,
        [
            new(direitoEconomia.Id, 1, 7, 73),
            new(infoSociedade.Id, 2, 7, 73),
            new(introDireito.Id, 1, 7, 73),
            new(direitoFinanceiro.Id, 1, 7, 73),
        ]);

        CourseOfferingOut courseOfferingAds = await client.CreateCourseOffering(campus.Id, ads.Id, courseCurriculumAds.Id, period.Id, Shift.Noturno);
        CourseOfferingOut courseOfferingDireito = await client.CreateCourseOffering(campus.Id, direito.Id, courseCurriculumDireito.Id, period.Id, Shift.Noturno);

        TeacherOut chico = await client.CreateTeacher("Chico");
        TeacherOut ana = await client.CreateTeacher("Ana");

        ClassOut classMatematica = await client.CreateClass(matematica.Id, chico.Id, period.Id, 40, [new(Day.Segunda, Hour.H07_00, Hour.H10_00)]);
        ClassOut classBancoDeDados = await client.CreateClass(bancoDeDados.Id, chico.Id, period.Id, 40, [new(Day.Terca, Hour.H07_00, Hour.H10_00)]);
        ClassOut classEstruturaDeDados = await client.CreateClass(estruturaDeDados.Id, chico.Id, period.Id, 40, [new(Day.Quarta, Hour.H07_00, Hour.H10_00)]);
        ClassOut classInfoSociedade = await client.CreateClass(infoSociedade.Id, ana.Id, period.Id, 40, [new(Day.Segunda, Hour.H07_00, Hour.H08_00)]);
        ClassOut classDireitoEconomia = await client.CreateClass(direitoEconomia.Id, ana.Id, period.Id, 40, [new(Day.Terca, Hour.H07_00, Hour.H08_00)]);
        ClassOut classIntroDireito = await client.CreateClass(introDireito.Id, ana.Id, period.Id, 40, [new(Day.Quarta, Hour.H07_00, Hour.H08_00)]);
        ClassOut classDireitoFinanceiro = await client.CreateClass(direitoFinanceiro.Id, ana.Id, period.Id, 40, [new(Day.Quinta, Hour.H07_00, Hour.H08_00)]);

        StudentOut zaqueu = await client.CreateStudent(courseOfferingAds.Id, "Zaqueu");
        StudentOut maju = await client.CreateStudent(courseOfferingDireito.Id, "Maju");

        var studentClient = await _back.LoggedAsStudent(zaqueu.Email);

        // Act
        var classes = await studentClient.GetStudentEnrollmentClasses();

        // Assert
        classes.Should().HaveCount(4);
        classes.Should().Contain(t => t.Id == classMatematica.Id);
        classes.Should().Contain(t => t.Id == classBancoDeDados.Id);
        classes.Should().Contain(t => t.Id == classEstruturaDeDados.Id);
        classes.Should().Contain(t => t.Id == classInfoSociedade.Id);
    }

    [Test]
    public async Task Should_not_return_any_class_without_enrollment_period()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        CourseOut course = await client.CreateCourse("ADS");
        CourseCurriculumOut cc = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        CourseOfferingOut courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, cc.Id, period.Id, Shift.Noturno);

        StudentOut student = await client.CreateStudent(courseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);

        // Act
        var classes = await studentClient.GetStudentEnrollmentClasses();

        // Assert
        classes.Should().BeEmpty();
    }

    [Test]
    public async Task Should_not_return_any_class_before_enrollment_period_start()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        CourseOut course = await client.CreateCourse("ADS");
        CourseCurriculumOut cc = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        CourseOfferingOut co = await client.CreateCourseOffering(campus.Id, course.Id, cc.Id, period.Id, Shift.Noturno);

        await client.CreateEnrollmentPeriod(period.Id, 2, 4);

        StudentOut student = await client.CreateStudent(co.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);

        // Act
        var classes = await studentClient.GetStudentEnrollmentClasses();

        // Assert
        classes.Should().BeEmpty();
    }

    [Test]
    public async Task Should_not_return_any_class_after_enrollment_period_end()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        CourseOut course = await client.CreateCourse("ADS");
        CourseCurriculumOut courseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        CourseOfferingOut courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);

        await client.CreateEnrollmentPeriod(period.Id, -4, -2);

        StudentOut student = await client.CreateStudent(courseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);

        // Act
        var classes = await studentClient.GetStudentEnrollmentClasses();

        // Assert
        classes.Should().BeEmpty();
    }
}
