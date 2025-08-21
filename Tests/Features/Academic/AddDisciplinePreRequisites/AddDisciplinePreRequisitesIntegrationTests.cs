namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_add_discipline_pre_requisits()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        // Act
        var response = await client.AddDisciplinePreRequisites(
            data.AdsCourseCurriculum.Id,
            data.AdsDisciplines.Databases.Id,
            [data.AdsDisciplines.DiscreteMath.Id]
        );

        // Assert
        response.ShouldBeSuccess();
    }

    [Test]
    public async Task Should_not_add_discipline_pre_requisits_when_course_curriculum_is_missing()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.AddDisciplinePreRequisites(Guid.CreateVersion7(), Guid.CreateVersion7(), []);

        // Assert
        response.ShouldBeError(CourseCurriculumNotFound.I);
    }

    [Test]
    public async Task Should_not_add_discipline_pre_requisits_when_discipline_is_missing()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        // Act
        var response = await client.AddDisciplinePreRequisites(
            data.AdsCourseCurriculum.Id,
            data.DireitoDisciplines.GeneralTheoryOfLaw.Id,
            []
        );

        // Assert
        response.ShouldBeError(new DisciplineNotFound());
    }

    [Test]
    public async Task Should_not_add_discipline_pre_requisits_when_target_discipline_is_in_pre_requisites()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        // Act
        var response = await client.AddDisciplinePreRequisites(
            data.AdsCourseCurriculum.Id,
            data.AdsDisciplines.Databases.Id,
            [data.AdsDisciplines.Databases.Id]
        );

        // Assert
        response.ShouldBeError(new InvalidDisciplinesList());
    }

    [Test]
    public async Task Should_not_add_discipline_pre_requisits_when_pre_requisites_is_not_subset_of_course_curriculums_disciplines()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        // Act
        var response = await client.AddDisciplinePreRequisites(
            data.AdsCourseCurriculum.Id,
            data.AdsDisciplines.Databases.Id,
            [data.AdsDisciplines.DiscreteMath.Id, data.DireitoDisciplines.GeneralTheoryOfLaw.Id]
        );

        // Assert
        response.ShouldBeError(new InvalidDisciplinesList());
    }

    [Test]
    public async Task Should_not_add_discipline_pre_requisits_when_pre_requisites_have_greater_period_discipline()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        // Act
        var response = await client.AddDisciplinePreRequisites(
            data.AdsCourseCurriculum.Id,
            data.AdsDisciplines.DiscreteMath.Id,
            [data.AdsDisciplines.Databases.Id]
        );

        // Assert
        response.ShouldBeError(new InvalidDisciplinesList());
    }

    [Test]
    public async Task Should_not_add_discipline_pre_requisits_when_pre_requisites_have_equal_period_discipline()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        // Act
        var response = await client.AddDisciplinePreRequisites(
            data.AdsCourseCurriculum.Id,
            data.AdsDisciplines.DiscreteMath.Id,
            [data.AdsDisciplines.Arch.Id]
        );

        // Assert
        response.ShouldBeError(new InvalidDisciplinesList());
    }
}
