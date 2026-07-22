namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Teachers_GetTeacherAgenda_Should_not_get_agenda_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetTeacherAgenda();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Teachers_GetTeacherAgenda_Should_not_get_agenda_when_user_is_a_manager()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetTeacherAgenda();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Teachers_GetTeacherAgenda_Should_not_get_agenda_when_user_is_a_student()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var email = DataGen.Email;
        await director.CreateStudent(DataGen.UserName, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetTeacherAgenda();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Teachers_GetTeacherAgenda_Should_get_empty_agenda_when_teacher_has_no_started_classes()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetTeacherAgenda();

        // Assert
        result.ShouldBeSuccess();
        result.Success.Days.Should().BeEmpty();
    }

    [Test]
    public async Task Teachers_GetTeacherAgenda_Should_get_agenda_of_a_class_with_a_single_teacher()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var discipline = await director.CreateDiscipline().Success();
        var period = await director.CreateAcademicPeriod().Success();

        var teacherEmail = DataGen.Email;
        var teacher = await director.CreateTeacher("Chico Ferreira", teacherEmail).Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var @class = await director.CreateClass(discipline.Id, period.Id).Success();
        await director.UpdateClassTeachers(@class.Id, [teacher.Id]);
        await director.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);
        await director.ReleaseClassForEnrollment(@class.Id);
        await director.StartClass(@class.Id);

        var client = await _back.LoginAs(teacherEmail);

        // Act
        var result = await client.GetTeacherAgenda();

        // Assert
        result.ShouldBeSuccess();
        var days = result.Success.Days;
        days.Should().HaveCount(1);
        days[0].Day.Should().Be(Day.Monday);
        days[0].Disciplines.Should().HaveCount(1);
        days[0].Disciplines[0].ClassId.Should().Be(@class.Id);
        days[0].Disciplines[0].Name.Should().Be("Geometria");
        days[0].Disciplines[0].Start.Should().Be(Hour.H07_00);
        days[0].Disciplines[0].End.Should().Be(Hour.H10_00);
    }

    [Test]
    public async Task Teachers_GetTeacherAgenda_Should_build_a_separate_agenda_for_each_teacher_of_a_two_teacher_class()
    {
        // Arrange — turma única com 2 professores: Chico na segunda e na terça, Ana na quarta.
        // Cada schedule aponta pro professor que o cobre, então cada agenda mostra só os horários daquele professor.
        var director = await _back.LoggedAsDirector();
        var discipline = await director.CreateDiscipline().Success();
        var period = await director.CreateAcademicPeriod().Success();

        var chicoEmail = DataGen.Email;
        var anaEmail = DataGen.Email;
        var chico = await director.CreateTeacher("Chico Ferreira", chicoEmail).Success();
        var ana = await director.CreateTeacher("Ana Lima", anaEmail).Success();
        await director.AssignDisciplinesToTeacher(chico.Id, [discipline.Id]);
        await director.AssignDisciplinesToTeacher(ana.Id, [discipline.Id]);

        var @class = await director.CreateClass(discipline.Id, period.Id).Success();
        await director.UpdateClassTeachers(@class.Id, [chico.Id, ana.Id]);
        await director.UpdateClassSchedules(@class.Id,
        [
            (Day.Monday, Hour.H07_00, Hour.H09_00, chico.Id),
            (Day.Tuesday, Hour.H07_00, Hour.H09_00, chico.Id),
            (Day.Wednesday, Hour.H07_00, Hour.H09_00, ana.Id),
        ]);
        await director.ReleaseClassForEnrollment(@class.Id);
        await director.StartClass(@class.Id);

        // Act — agenda do Chico
        var chicoClient = await _back.LoginAs(chicoEmail);
        var chicoResult = await chicoClient.GetTeacherAgenda();

        // Assert — só os dias do Chico (segunda e terça), nunca a quarta da Ana
        chicoResult.ShouldBeSuccess();
        var chicoDays = chicoResult.Success.Days;
        chicoDays.Should().HaveCount(2);
        chicoDays.Select(d => d.Day).Should().Equal(Day.Monday, Day.Tuesday);
        chicoDays.Should().OnlyContain(d => d.Disciplines.Count == 1);
        chicoDays.Should().OnlyContain(d => d.Disciplines[0].ClassId == @class.Id);
        chicoDays.Should().OnlyContain(d => d.Disciplines[0].Name == "Geometria");

        // Act — agenda da Ana
        var anaClient = await _back.LoginAs(anaEmail);
        var anaResult = await anaClient.GetTeacherAgenda();

        // Assert — só o dia da Ana (quarta), nunca os dias do Chico
        anaResult.ShouldBeSuccess();
        var anaDays = anaResult.Success.Days;
        anaDays.Should().HaveCount(1);
        anaDays[0].Day.Should().Be(Day.Wednesday);
        anaDays[0].Disciplines.Should().HaveCount(1);
        anaDays[0].Disciplines[0].ClassId.Should().Be(@class.Id);
        anaDays[0].Disciplines[0].Name.Should().Be("Geometria");
    }

    [Test]
    public async Task Teachers_GetTeacherAgenda_Should_order_the_disciplines_of_a_day_by_start_hour()
    {
        // Arrange — dois horários do mesmo professor no mesmo dia, cadastrados fora de ordem.
        var director = await _back.LoggedAsDirector();
        var geometria = await director.CreateDiscipline("Geometria").Success();
        var algebra = await director.CreateDiscipline("Álgebra").Success();
        var period = await director.CreateAcademicPeriod().Success();

        var teacherEmail = DataGen.Email;
        var teacher = await director.CreateTeacher("Chico Ferreira", teacherEmail).Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [geometria.Id, algebra.Id]);

        var morningClass = await director.CreateClass(geometria.Id, period.Id).Success();
        await director.UpdateClassTeachers(morningClass.Id, [teacher.Id]);
        await director.UpdateClassSchedules(morningClass.Id, [(Day.Monday, Hour.H07_00, Hour.H09_00, null)]);
        await director.ReleaseClassForEnrollment(morningClass.Id);
        await director.StartClass(morningClass.Id);

        var nightClass = await director.CreateClass(algebra.Id, period.Id).Success();
        await director.UpdateClassTeachers(nightClass.Id, [teacher.Id]);
        await director.UpdateClassSchedules(nightClass.Id, [(Day.Monday, Hour.H19_00, Hour.H22_00, null)]);
        await director.ReleaseClassForEnrollment(nightClass.Id);
        await director.StartClass(nightClass.Id);

        var client = await _back.LoginAs(teacherEmail);

        // Act
        var result = await client.GetTeacherAgenda();

        // Assert
        result.ShouldBeSuccess();
        var days = result.Success.Days;
        days.Should().HaveCount(1);
        days[0].Day.Should().Be(Day.Monday);
        days[0].Disciplines.Should().HaveCount(2);
        days[0].Disciplines.Select(d => d.Start).Should().Equal(Hour.H07_00, Hour.H19_00);
        days[0].Disciplines[0].Name.Should().Be("Geometria");
        days[0].Disciplines[1].Name.Should().Be("Álgebra");
    }

    [Test]
    public async Task Teachers_GetTeacherAgenda_Should_show_the_classroom_name_when_the_schedule_has_a_classroom_and_null_when_online()
    {
        // Arrange — turma com dois horários: segunda numa sala física, quarta sem sala (online).
        var director = await _back.LoggedAsDirector();
        var campus = await director.CreateCampus().Success();
        var classroom = await director.CreateClassroom(campus.Id, "Sala 07").Success();
        var discipline = await director.CreateDiscipline().Success();
        var period = await director.CreateAcademicPeriod().Success();

        var teacherEmail = DataGen.Email;
        var teacher = await director.CreateTeacher("Chico Ferreira", teacherEmail).Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var @class = await director.CreateClass(discipline.Id, period.Id, campusId: campus.Id).Success();
        await director.UpdateClassTeachers(@class.Id, [teacher.Id]);
        await director.UpdateClassSchedules(@class.Id,
        [
            (Day.Monday, Hour.H07_00, Hour.H10_00, null),
            (Day.Wednesday, Hour.H07_00, Hour.H10_00, null),
        ]);
        await director.UpdateClassClassrooms(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, classroom.Id)]);
        await director.ReleaseClassForEnrollment(@class.Id);
        await director.StartClass(@class.Id);

        var client = await _back.LoginAs(teacherEmail);

        // Act
        var result = await client.GetTeacherAgenda();

        // Assert
        result.ShouldBeSuccess();
        var days = result.Success.Days;
        days.Should().HaveCount(2);
        days[0].Day.Should().Be(Day.Monday);
        days[0].Disciplines[0].ClassroomName.Should().Be("Sala 07");
        days[1].Day.Should().Be(Day.Wednesday);
        days[1].Disciplines[0].ClassroomName.Should().BeNull();
    }

    #endregion
}
