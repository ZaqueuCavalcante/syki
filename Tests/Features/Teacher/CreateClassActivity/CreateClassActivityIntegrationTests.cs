namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_class_activity()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        TeacherOut chico = await academicClient.CreateTeacher();
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        // Act
        var response = await teacherClient.CreateClassActivity(
            mathClass.Id,
            ClassNoteType.N2,
            "Test activity",
            "Test description",
            ClassActivityType.Work,
            69,
            DateTime.UtcNow.AddDays(7).ToDateOnly(),
            Hour.H08_30);

        // Assert
        var activity = (await teacherClient.GetTeacherClassActivity(mathClass.Id, response.Success.Id)).Success;
        activity.Note.Should().Be(ClassNoteType.N2);
        activity.Title.Should().Be("Test activity");
        activity.Description.Should().Be("Test description");
        activity.Type.Should().Be(ClassActivityType.Work);
        activity.Weight.Should().Be(69);
        activity.DueDate.Should().Be(DateTime.UtcNow.AddDays(7).ToDateOnly());
        activity.DueHour.Should().Be(Hour.H08_30);
    }

    [Test]
    public async Task Should_create_many_class_activities()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        TeacherOut chico = await academicClient.CreateTeacher();
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        // Act
        await teacherClient.CreateClassActivity(mathClass.Id, ClassNoteType.N1, type: ClassActivityType.Work, weight: 25);
        await teacherClient.CreateClassActivity(mathClass.Id, ClassNoteType.N1, type: ClassActivityType.Exam, weight: 75);
        await teacherClient.CreateClassActivity(mathClass.Id, ClassNoteType.N2, type: ClassActivityType.Presentation, weight: 10);
        await teacherClient.CreateClassActivity(mathClass.Id, ClassNoteType.N2, type: ClassActivityType.Work, weight: 30);
        await teacherClient.CreateClassActivity(mathClass.Id, ClassNoteType.N2, type: ClassActivityType.Exam, weight: 60);
        await teacherClient.CreateClassActivity(mathClass.Id, ClassNoteType.N3, type: ClassActivityType.Project, weight: 100);

        // Assert
        var activities = (await teacherClient.GetTeacherClassActivities(mathClass.Id)).Success;
        activities.Should().HaveCount(6);
    }

    [Test]
    public async Task Should_not_create_class_activity_when_class_not_found()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();

        TeacherOut chico = await academicClient.CreateTeacher();
        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        // Act
        var response = await teacherClient.CreateClassActivity(Guid.CreateVersion7());

        // Assert
        response.ShouldBeError(new ClassNotFound());
    }

    [Test]
    [TestCase(-1)]
    [TestCase(101)]
    public async Task Should_not_create_class_activity_with_invalid_weight(int weight)
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        TeacherOut chico = await academicClient.CreateTeacher();
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        // Act
        var response = await teacherClient.CreateClassActivity(mathClass.Id, weight: weight);

        // Assert
        response.ShouldBeError(new InvalidClassActivityWeight());
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ClassActivityInvalidWeightsLists))]
    public async Task Should_not_add_class_activities_with_invalid_weights(int[] weights)
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        TeacherOut chico = await academicClient.CreateTeacher();
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        OneOf<CreateClassActivityOut, ErrorOut> response = new CreateClassActivityOut();

        // Act
        foreach (var weight in weights)
        {
            response = await teacherClient.CreateClassActivity(mathClass.Id, weight: weight);
        }

        // Assert
        response.ShouldBeError(new InvalidClassActivityWeight());
    }
}
