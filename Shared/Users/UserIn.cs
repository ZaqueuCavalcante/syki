namespace Syki.Shared;

public class UserIn
{
    public Guid InstitutionId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }

    public static UserIn New(Guid institutionId, string role)
    {
        var hash = institutionId.ToString().OnlyNumbers();
        return new UserIn
        {
            InstitutionId = institutionId,
            Name = $"{role} - {hash}",
            Email = $"{role.ToLower()}@{hash}.com",
            Password = $"{role}@123{hash}",
            Role = role,
        };
    }

    public static UserIn NewDemoAcademico(Guid institutionId, string email, string password)
    {
        return new UserIn
        {
            Name = $"DEMO - {email}",
            Email = email,
            Password = password,
            InstitutionId = institutionId,
            Role = "Academico",
        };
    }
}
