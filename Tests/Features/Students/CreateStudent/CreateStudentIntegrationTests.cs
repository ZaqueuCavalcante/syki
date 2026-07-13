namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Students_CreateStudent_Should_not_create_student_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateStudent(DataGen.UserName, DataGen.Email);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Students_CreateStudent_Should_not_create_student_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateStudent(DataGen.UserName, DataGen.Email);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Students_CreateStudent_Should_not_create_student_when_email_already_used()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var email = DataGen.Email;
        await client.CreateStudent(DataGen.UserName, email);

        // Act
        var result = await client.CreateStudent(DataGen.UserName, email);

        // Assert
        result.ShouldBeError(EmailAlreadyUsed.I);
    }

    [TestCase("123456789")]
    [TestCase("123456789012")]
    [TestCase("(82) 98888-7777")]
    [TestCase("82 98888 7777")]
    [TestCase("abcdefghijk")]
    public async Task Students_CreateStudent_Should_not_create_student_when_phone_number_is_invalid(string phoneNumber)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateStudent(DataGen.UserName, DataGen.Email, phoneNumber: phoneNumber);

        // Assert
        result.ShouldBeError(InvalidPhoneNumber.I);
    }

    [Test]
    public async Task Students_CreateStudent_Should_not_create_student_when_birthdate_is_in_the_future()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var birthdate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1);

        // Act
        var result = await client.CreateStudent(DataGen.UserName, DataGen.Email, birthdate: birthdate);

        // Assert
        result.ShouldBeError(InvalidBirthdate.I);
    }

    [Test]
    public async Task Students_CreateStudent_Should_not_create_student_when_birthdate_is_too_old()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var birthdate = new DateOnly(1899, 12, 31);

        // Act
        var result = await client.CreateStudent(DataGen.UserName, DataGen.Email, birthdate: birthdate);

        // Assert
        result.ShouldBeError(InvalidBirthdate.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Students_CreateStudent_Should_create_student()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateStudent(DataGen.UserName, DataGen.Email);

        // Assert
        result.ShouldBeSuccess();
        var student = result.Success;
        student.Id.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task Students_CreateStudent_Should_create_student_without_phone_number_and_birthdate()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateStudent(DataGen.UserName, DataGen.Email);

        // Assert
        result.ShouldBeSuccess();
        var studentId = result.Success.Id;

        await using var ctx = _back.GetDbContext();
        var user = await ctx.Students.Where(s => s.Id == studentId).Select(s => s.User!).FirstAsync();
        user.PhoneNumber.Should().BeNull();
        user.Birthdate.Should().BeNull();
    }

    [Test]
    public async Task Students_CreateStudent_Should_create_student_with_phone_number_and_birthdate()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var phoneNumber = "82988887777";
        var birthdate = new DateOnly(2000, 5, 10);

        // Act
        var result = await client.CreateStudent(DataGen.UserName, DataGen.Email, phoneNumber, birthdate);

        // Assert
        result.ShouldBeSuccess();
        var studentId = result.Success.Id;

        await using var ctx = _back.GetDbContext();
        var user = await ctx.Students.Where(s => s.Id == studentId).Select(s => s.User!).FirstAsync();
        user.PhoneNumber.Should().Be(phoneNumber);
        user.Birthdate.Should().Be(birthdate);
    }

    [Test]
    public async Task Students_CreateStudent_Should_create_student_with_only_phone_number()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var phoneNumber = "8233334444";

        // Act
        var result = await client.CreateStudent(DataGen.UserName, DataGen.Email, phoneNumber: phoneNumber);

        // Assert
        result.ShouldBeSuccess();
        var studentId = result.Success.Id;

        await using var ctx = _back.GetDbContext();
        var user = await ctx.Students.Where(s => s.Id == studentId).Select(s => s.User!).FirstAsync();
        user.PhoneNumber.Should().Be(phoneNumber);
        user.Birthdate.Should().BeNull();
    }

    [Test]
    public async Task Students_CreateStudent_Should_create_student_with_only_birthdate()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var birthdate = new DateOnly(1995, 1, 20);

        // Act
        var result = await client.CreateStudent(DataGen.UserName, DataGen.Email, birthdate: birthdate);

        // Assert
        result.ShouldBeSuccess();
        var studentId = result.Success.Id;

        await using var ctx = _back.GetDbContext();
        var user = await ctx.Students.Where(s => s.Id == studentId).Select(s => s.User!).FirstAsync();
        user.PhoneNumber.Should().BeNull();
        user.Birthdate.Should().Be(birthdate);
    }

    #endregion
}
