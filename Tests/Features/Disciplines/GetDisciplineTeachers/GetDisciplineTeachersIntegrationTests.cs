namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Disciplines_GetDisciplineTeachers_Should_not_get_teachers_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetDisciplineTeachers(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Disciplines_GetDisciplineTeachers_Should_not_get_teachers_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetDisciplineTeachers(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Disciplines_GetDisciplineTeachers_Should_not_get_teachers_when_discipline_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetDisciplineTeachers(999999);

        // Assert
        result.ShouldBeError(DisciplineNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Disciplines_GetDisciplineTeachers_Should_get_only_the_teachers_assigned_to_the_discipline()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var geometria = await client.CreateDiscipline("Geometria").Success();
        var fisica = await client.CreateDiscipline("Fisica").Success();

        var chico = await client.CreateTeacher("Chico Ferreira", DataGen.Email).Success();
        var ana = await client.CreateTeacher("Ana Lima", DataGen.Email).Success();
        var bruno = await client.CreateTeacher("Bruno Alves", DataGen.Email).Success();

        await client.AssignDisciplinesToTeacher(chico.Id, [geometria.Id]);
        await client.AssignDisciplinesToTeacher(ana.Id, [geometria.Id]);
        await client.AssignDisciplinesToTeacher(bruno.Id, [fisica.Id]);

        // Act
        var result = await client.GetDisciplineTeachers(geometria.Id);

        // Assert
        var items = result.Success.Items;
        items.Select(x => x.Name).Should().Equal("Ana Lima", "Chico Ferreira");
        items.Select(x => x.Id).Should().Equal(ana.Id, chico.Id);
    }

    [Test]
    public async Task Disciplines_GetDisciplineTeachers_Should_get_an_empty_list_when_the_discipline_has_no_teachers()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline("Geometria").Success();

        // Act
        var result = await client.GetDisciplineTeachers(discipline.Id);

        // Assert
        result.Success.Items.Should().BeEmpty();
    }

    #endregion
}
