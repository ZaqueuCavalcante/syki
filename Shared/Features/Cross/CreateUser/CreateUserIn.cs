namespace Syki.Shared;

public class CreateUserIn
{
    public Guid InstitutionId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime BirthDate { get; set; }
    public UserRole Role { get; set; }
    public string PhoneNumber { get; set; }

    public static CreateUserIn NewAcademic(Guid institutionId, string email, string password)
    {
        return new()
        {
            Name = email,
            Email = email,
            Password = password,
            Role = UserRole.Academic,
            InstitutionId = institutionId,
        };
    }

    public static CreateUserIn NewTeacher(Guid institutionId, string name, string email)
    {
        return new()
        {
            Name = name,
            Email = email,
            Role = UserRole.Teacher,
            InstitutionId = institutionId,
            Password = $"Teacher@{Guid.CreateVersion7()}",
        };
    }

    public static CreateUserIn NewStudent(Guid institutionId, string name, string email, string phoneNumber, DateTime birthDate)
    {
        return new()
        {
            Name = name,
            Email = email,
            Role = UserRole.Student,
            PhoneNumber = phoneNumber,
            BirthDate = birthDate,
            InstitutionId = institutionId,
            Password = $"Student@{Guid.CreateVersion7()}",
        };
    }
}
