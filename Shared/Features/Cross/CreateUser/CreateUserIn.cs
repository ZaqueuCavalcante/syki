namespace Syki.Shared;

public class CreateUserIn
{
    public Guid InstitutionId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }

    public static CreateUserIn NewAcademic(Guid institutionId, string email, string password)
    {
        return new CreateUserIn
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
        return new CreateUserIn
        {
            Name = name,
            Email = email,
            Role = UserRole.Teacher,
            InstitutionId = institutionId,
            Password = $"Teacher@{Guid.NewGuid().ToString().OnlyNumbers()}",
        };
    }

    public static CreateUserIn NewStudent(Guid institutionId, string name, string email)
    {
        return new CreateUserIn
        {
            Name = name,
            Email = email,
            Role = UserRole.Student,
            InstitutionId = institutionId,
            Password = $"Student@{Guid.NewGuid().ToString().OnlyNumbers()}",
        };
    }
}
