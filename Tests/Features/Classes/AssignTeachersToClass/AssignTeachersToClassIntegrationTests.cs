namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Classes_AssignTeachersToClass_Should_not_assign_teachers_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.AssignTeachersToClass(1, []);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Classes_AssignTeachersToClass_Should_not_assign_teachers_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.AssignTeachersToClass(1, []);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Classes_AssignTeachersToClass_Should_not_assign_teachers_when_class_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.AssignTeachersToClass(999999, []);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Classes_AssignTeachersToClass_Should_not_assign_teachers_when_list_has_more_than_two_teachers()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id)).Success;

        var first = (await client.CreateTeacher(DataGen.UserName, DataGen.Email)).Success;
        var second = (await client.CreateTeacher(DataGen.UserName, DataGen.Email)).Success;
        var third = (await client.CreateTeacher(DataGen.UserName, DataGen.Email)).Success;

        // Act
        var result = await client.AssignTeachersToClass(@class.Id, [first.Id, second.Id, third.Id]);

        // Assert
        result.ShouldBeError(InvalidTeachersList.I);
    }

    [Test]
    public async Task Classes_AssignTeachersToClass_Should_not_assign_teachers_when_list_has_duplicated_teachers()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id)).Success;

        var teacher = (await client.CreateTeacher(DataGen.UserName, DataGen.Email)).Success;
        await client.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        // Act
        var result = await client.AssignTeachersToClass(@class.Id, [teacher.Id, teacher.Id]);

        // Assert
        result.ShouldBeError(InvalidTeachersList.I);
    }

    [Test]
    public async Task Classes_AssignTeachersToClass_Should_not_assign_teachers_when_a_teacher_does_not_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id)).Success;

        // Act
        var result = await client.AssignTeachersToClass(@class.Id, [999999]);

        // Assert
        result.ShouldBeError(TeacherNotFound.I);
    }

    [Test]
    public async Task Classes_AssignTeachersToClass_Should_not_assign_teachers_when_a_teacher_is_not_assigned_to_the_class_discipline()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline("Geometria")).Success;
        var otherDiscipline = (await client.CreateDiscipline("Fisica")).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id)).Success;

        var teacher = (await client.CreateTeacher(DataGen.UserName, DataGen.Email)).Success;
        await client.AssignDisciplinesToTeacher(teacher.Id, [otherDiscipline.Id]);

        // Act
        var result = await client.AssignTeachersToClass(@class.Id, [teacher.Id]);

        // Assert
        result.ShouldBeError(TeacherNotAssignedToDiscipline.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Classes_AssignTeachersToClass_Should_assign_two_teachers_to_the_class()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id)).Success;

        var chico = (await client.CreateTeacher("Chico Ferreira", DataGen.Email)).Success;
        var ana = (await client.CreateTeacher("Ana Lima", DataGen.Email)).Success;
        await client.AssignDisciplinesToTeacher(chico.Id, [discipline.Id]);
        await client.AssignDisciplinesToTeacher(ana.Id, [discipline.Id]);

        // Act
        var result = await client.AssignTeachersToClass(@class.Id, [chico.Id, ana.Id]);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var updated = (await client.GetClass(@class.Id)).Success;
        updated.Teachers.Select(t => t.Name).Should().Equal("Ana Lima", "Chico Ferreira");
    }

    [Test]
    public async Task Classes_AssignTeachersToClass_Should_replace_the_current_teachers_of_the_class()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id)).Success;

        var chico = (await client.CreateTeacher("Chico Ferreira", DataGen.Email)).Success;
        var ana = (await client.CreateTeacher("Ana Lima", DataGen.Email)).Success;
        await client.AssignDisciplinesToTeacher(chico.Id, [discipline.Id]);
        await client.AssignDisciplinesToTeacher(ana.Id, [discipline.Id]);
        await client.AssignTeachersToClass(@class.Id, [chico.Id]);

        // Act
        var result = await client.AssignTeachersToClass(@class.Id, [ana.Id]);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var updated = (await client.GetClass(@class.Id)).Success;
        updated.Teachers.Select(t => t.Name).Should().Equal("Ana Lima");
    }

    [Test]
    public async Task Classes_AssignTeachersToClass_Should_remove_all_teachers_of_the_class_when_list_is_empty()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id)).Success;

        var teacher = (await client.CreateTeacher("Chico Ferreira", DataGen.Email)).Success;
        await client.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);
        await client.AssignTeachersToClass(@class.Id, [teacher.Id]);

        // Act
        var result = await client.AssignTeachersToClass(@class.Id, []);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var updated = (await client.GetClass(@class.Id)).Success;
        updated.Teachers.Should().BeEmpty();
    }

    #endregion
}
