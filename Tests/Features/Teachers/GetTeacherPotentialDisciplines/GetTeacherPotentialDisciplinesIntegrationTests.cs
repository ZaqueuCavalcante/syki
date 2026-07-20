namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Teachers_GetTeacherPotentialDisciplines_Should_not_get_potential_disciplines_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetTeacherPotentialDisciplines(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Teachers_GetTeacherPotentialDisciplines_Should_not_get_potential_disciplines_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetTeacherPotentialDisciplines(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Teachers_GetTeacherPotentialDisciplines_Should_not_get_potential_disciplines_when_teacher_does_not_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetTeacherPotentialDisciplines(999999);

        // Assert
        result.ShouldBeError(TeacherNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Teachers_GetTeacherPotentialDisciplines_Should_get_the_disciplines_not_assigned_to_the_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var calculo = await client.CreateDiscipline("Calculo").Success();
        var fisica = await client.CreateDiscipline("Fisica").Success();
        var teacher = await client.CreateTeacher("Ana Lima", DataGen.Email).Success();

        // Act
        var result = await client.GetTeacherPotentialDisciplines(teacher.Id);

        // Assert
        var items = result.Success.Items;
        items.Should().HaveCount(2);
        items.Should().Contain(x => x.Id == calculo.Id);
        items.Should().Contain(x => x.Id == fisica.Id);
    }

    [Test]
    public async Task Teachers_GetTeacherPotentialDisciplines_Should_not_get_disciplines_already_assigned_to_the_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var calculo = await client.CreateDiscipline("Calculo").Success();
        var fisica = await client.CreateDiscipline("Fisica").Success();
        var teacher = await client.CreateTeacher("Ana Lima", DataGen.Email).Success();
        await client.AssignDisciplinesToTeacher(teacher.Id, [calculo.Id]);

        // Act
        var result = await client.GetTeacherPotentialDisciplines(teacher.Id);

        // Assert
        var items = result.Success.Items;
        items.Should().ContainSingle(x => x.Id == fisica.Id);
        items.Should().NotContain(x => x.Id == calculo.Id);
    }

    [Test]
    public async Task Teachers_GetTeacherPotentialDisciplines_Should_filter_potential_disciplines_by_name()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var calculo = await client.CreateDiscipline("Calculo").Success();
        await client.CreateDiscipline("Fisica");
        var teacher = await client.CreateTeacher("Ana Lima", DataGen.Email).Success();

        // Act
        var result = await client.GetTeacherPotentialDisciplines(teacher.Id, name: "Calc");

        // Assert
        var items = result.Success.Items;
        items.Should().ContainSingle(x => x.Id == calculo.Id);
    }

    #endregion
}
