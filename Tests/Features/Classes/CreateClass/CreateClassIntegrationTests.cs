namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateClass(disciplineId: 1, periodId: 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateClass(disciplineId: 1, periodId: 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_when_vacancies_is_negative()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        // Act
        var result = await client.CreateClass(discipline.Id, period.Id, vacancies: -1);

        // Assert
        result.ShouldBeError(InvalidClassVacancies.I);
    }

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_when_vacancies_is_greater_than_100()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        // Act
        var result = await client.CreateClass(discipline.Id, period.Id, vacancies: 101);

        // Assert
        result.ShouldBeError(InvalidClassVacancies.I);
    }

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_when_discipline_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var period = await client.CreateAcademicPeriod().Success();

        // Act
        var result = await client.CreateClass(disciplineId: 999999, period.Id);

        // Assert
        result.ShouldBeError(DisciplineNotFound.I);
    }

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_when_academic_period_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();

        // Act
        var result = await client.CreateClass(discipline.Id, periodId: 999999);

        // Assert
        result.ShouldBeError(AcademicPeriodNotFound.I);
    }

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_when_campus_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        // Act
        var result = await client.CreateClass(discipline.Id, period.Id, campusId: 999999);

        // Assert
        result.ShouldBeError(CampusNotFound.I);
    }

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_with_discipline_of_another_institution()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var period = await client.CreateAcademicPeriod().Success();

        var otherClient = await _back.LoggedAsDirector();
        var otherDiscipline = await otherClient.CreateDiscipline().Success();

        // Act
        var result = await client.CreateClass(otherDiscipline.Id, period.Id);

        // Assert
        result.ShouldBeError(DisciplineNotFound.I);
    }

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_with_academic_period_of_another_institution()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();

        var otherClient = await _back.LoggedAsDirector();
        var otherPeriod = await otherClient.CreateAcademicPeriod().Success();

        // Act
        var result = await client.CreateClass(discipline.Id, otherPeriod.Id);

        // Assert
        result.ShouldBeError(AcademicPeriodNotFound.I);
    }

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_with_campus_of_another_institution()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        var otherClient = await _back.LoggedAsDirector();
        var otherCampus = await otherClient.CreateCampus().Success();

        // Act
        var result = await client.CreateClass(discipline.Id, period.Id, campusId: otherCampus.Id);

        // Assert
        result.ShouldBeError(CampusNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Classes_CreateClass_Should_create_class()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        // Act
        var result = await client.CreateClass(discipline.Id, period.Id);

        // Assert
        var @class = result.Success;
        @class.Id.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task Classes_CreateClass_Should_create_class_with_campus()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var campus = await client.CreateCampus().Success();

        // Act
        var result = await client.CreateClass(discipline.Id, period.Id, campusId: campus.Id);

        // Assert
        var @class = result.Success;
        @class.Id.Should().BeGreaterThan(0);
    }

    #endregion
}
