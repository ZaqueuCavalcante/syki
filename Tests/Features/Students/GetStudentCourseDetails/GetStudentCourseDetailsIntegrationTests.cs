using Estud.Back.Features.CourseCurriculums.CreateCourseCurriculum;

namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Students_GetStudentCourseDetails_Should_not_get_course_details_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetStudentCourseDetails();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Students_GetStudentCourseDetails_Should_not_get_course_details_when_user_is_a_director()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetStudentCourseDetails();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Students_GetStudentCourseDetails_Should_not_get_course_details_when_user_is_a_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetStudentCourseDetails();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Students_GetStudentCourseDetails_Should_not_get_course_details_when_student_is_not_enrolled_in_any_course()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateStudent(DataGen.UserName, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentCourseDetails();

        // Assert
        result.ShouldBeError(StudentNotEnrolledInAnyCourse.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Students_GetStudentCourseDetails_Should_get_course_details_with_disciplines_status()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var campus = await director.CreateCampus().Success();
        var course = await director.CreateCourse().Success();

        var algorithms = await director.CreateDiscipline("Algoritmos").Success();
        var databases = await director.CreateDiscipline("Banco de Dados").Success();
        await director.AddDisciplineCourses(algorithms.Id, [course.Id]);
        await director.AddDisciplineCourses(databases.Id, [course.Id]);

        var curriculum = await director.CreateCourseCurriculum(course.Id, "Grade ADS 2024",
        [
            new CreateCourseCurriculumDisciplineIn(algorithms.Id, 1, 4, 60),
            new CreateCourseCurriculumDisciplineIn(databases.Id, 2, 4, 60),
        ]).Success();

        var period = await director.CreateAcademicPeriod().Success();
        var offering = await director.CreateCourseOffering(campus.Id, course.Id, curriculum.Id, period.Id).Success();

        var email = DataGen.Email;
        var student = await director.CreateStudent(DataGen.UserName, email).Success();
        await director.EnrollStudentInCourseOffering(student.Id, offering.Id);

        // Aluno está cursando "Algoritmos" (matriculado numa turma da disciplina)
        var @class = await director.CreateClass(algorithms.Id, period.Id).Success();
        await director.ReleaseClassForEnrollment(@class.Id);
        await director.AssignStudentToClass(student.Id, @class.Id);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentCourseDetails();

        // Assert
        var details = result.Success;
        details.CourseOfferingId.Should().Be(offering.Id);
        details.Course.Should().Be("Análise e Desenvolvimento de Sistemas");
        details.Curriculum.Should().Be("Grade ADS 2024");
        details.Campus.Should().NotBeNullOrEmpty();
        details.Period.Should().Be("2024.1");
        details.Disciplines.Should().HaveCount(2);

        var algorithmsItem = details.Disciplines.Single(d => d.Id == algorithms.Id);
        algorithmsItem.Name.Should().Be("Algoritmos");
        algorithmsItem.Period.Should().Be(1);
        algorithmsItem.Credits.Should().Be(4);
        algorithmsItem.Workload.Should().Be(60);
        algorithmsItem.Status.Should().Be(StudentDisciplineStatus.Cursando);

        var databasesItem = details.Disciplines.Single(d => d.Id == databases.Id);
        databasesItem.Name.Should().Be("Banco de Dados");
        databasesItem.Period.Should().Be(2);
        databasesItem.Status.Should().Be(StudentDisciplineStatus.NaoCursada);
    }

    [Test]
    public async Task Students_GetStudentCourseDetails_Should_order_disciplines_by_curriculum_period()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var campus = await director.CreateCampus().Success();
        var course = await director.CreateCourse().Success();

        var first = await director.CreateDiscipline("Algoritmos").Success();
        var second = await director.CreateDiscipline("Estrutura de Dados").Success();
        var third = await director.CreateDiscipline("Banco de Dados").Success();
        await director.AddDisciplineCourses(first.Id, [course.Id]);
        await director.AddDisciplineCourses(second.Id, [course.Id]);
        await director.AddDisciplineCourses(third.Id, [course.Id]);

        var curriculum = await director.CreateCourseCurriculum(course.Id, "Grade ADS 2024",
        [
            new CreateCourseCurriculumDisciplineIn(third.Id, 3, 4, 60),
            new CreateCourseCurriculumDisciplineIn(first.Id, 1, 4, 60),
            new CreateCourseCurriculumDisciplineIn(second.Id, 2, 4, 60),
        ]).Success();

        var period = await director.CreateAcademicPeriod().Success();
        var offering = await director.CreateCourseOffering(campus.Id, course.Id, curriculum.Id, period.Id).Success();

        var email = DataGen.Email;
        var student = await director.CreateStudent(DataGen.UserName, email).Success();
        await director.EnrollStudentInCourseOffering(student.Id, offering.Id);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentCourseDetails();

        // Assert
        var details = result.Success;
        details.Disciplines.Select(d => d.Period).Should().ContainInOrder((byte)1, (byte)2, (byte)3);
        details.Disciplines.Should().OnlyContain(d => d.Status == StudentDisciplineStatus.NaoCursada);
    }

    #endregion
}
