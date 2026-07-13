namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Teachers_GetTeacherCurrentClasses_Should_not_get_current_classes_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetTeacherCurrentClasses();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Teachers_GetTeacherCurrentClasses_Should_not_get_current_classes_when_user_is_not_a_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetTeacherCurrentClasses();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Teachers_GetTeacherCurrentClasses_Should_get_current_classes_ordered_by_discipline_name()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var teacher = (await director.CreateTeacher(DataGen.UserName, email)).Success;

        var geometria = (await director.CreateDiscipline("Geometria")).Success;
        var algebra = (await director.CreateDiscipline("Álgebra")).Success;
        await director.AssignDisciplinesToTeacher(teacher.Id, [geometria.Id, algebra.Id]);

        var period = (await director.CreateAcademicPeriod()).Success;
        var geometriaClass = (await director.CreateClass(geometria.Id, period.Id, teacherId: teacher.Id)).Success;
        var algebraClass = (await director.CreateClass(algebra.Id, period.Id, teacherId: teacher.Id)).Success;

        await StartClasses(geometriaClass.Id, algebraClass.Id);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetTeacherCurrentClasses();

        // Assert
        var classes = result.Success.Classes;
        classes.Should().HaveCount(2);
        classes[0].Id.Should().Be(algebraClass.Id);
        classes[0].Name.Should().Be("Álgebra");
        classes[1].Id.Should().Be(geometriaClass.Id);
        classes[1].Name.Should().Be("Geometria");
    }

    [Test]
    public async Task Teachers_GetTeacherCurrentClasses_Should_not_get_classes_that_are_not_started()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var teacher = (await director.CreateTeacher(DataGen.UserName, email)).Success;

        var discipline = (await director.CreateDiscipline()).Success;
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = (await director.CreateAcademicPeriod()).Success;
        await director.CreateClass(discipline.Id, period.Id, teacherId: teacher.Id);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetTeacherCurrentClasses();

        // Assert
        result.Success.Classes.Should().BeEmpty();
    }

    [Test]
    public async Task Teachers_GetTeacherCurrentClasses_Should_not_get_classes_of_another_teacher()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var teacher = (await director.CreateTeacher(DataGen.UserName, email)).Success;
        var otherTeacher = (await director.CreateTeacher(DataGen.UserName, DataGen.Email)).Success;

        var discipline = (await director.CreateDiscipline()).Success;
        await director.AssignDisciplinesToTeacher(otherTeacher.Id, [discipline.Id]);

        var period = (await director.CreateAcademicPeriod()).Success;
        var otherClass = (await director.CreateClass(discipline.Id, period.Id, teacherId: otherTeacher.Id)).Success;

        await StartClasses(otherClass.Id);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetTeacherCurrentClasses();

        // Assert
        result.Success.Classes.Should().BeEmpty();
    }

    private async Task StartClasses(params int[] ids)
    {
        await using var ctx = _back.GetDbContext();
        var classes = await ctx.Classes.Where(c => ids.Contains(c.Id)).ToListAsync();
        classes.ForEach(c => c.Status = ClassStatus.Started);
        await ctx.SaveChangesAsync();
    }

    #endregion
}
