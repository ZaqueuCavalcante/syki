namespace Syki.Shared;

public class CreateUserIn
{
    public Guid InstitutionId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }

    public static CreateUserIn New(Guid institutionId, string role)
    {
        var hash = institutionId.ToString().OnlyNumbers();
        return new CreateUserIn
        {
            InstitutionId = institutionId,
            Name = $"{role} - {hash}",
            Email = $"{role.ToLower()}@{hash}.com",
            Password = $"{role}@123{hash}",
            Role = role,
        };
    }

    public static CreateUserIn NewAcademico(Guid institutionId, string email, string password)
    {
        return new CreateUserIn
        {
            Name = email,
            Email = email,
            Role = "Academico",
            Password = password,
            InstitutionId = institutionId,
        };
    }

    public static CreateUserIn NewProfessor(Guid institutionId, string email)
    {
        return new CreateUserIn
        {
            Name = email,
            Email = email,
            Role = "Professor",
            InstitutionId = institutionId,
            Password = $"Professor@{Guid.NewGuid().ToString().OnlyNumbers()}",
        };
    }

    public static CreateUserIn NewAluno(Guid institutionId, string email)
    {
        return new CreateUserIn
        {
            Name = email,
            Email = email,
            Role = "Aluno",
            InstitutionId = institutionId,
            Password = $"Aluno@{Guid.NewGuid().ToString().OnlyNumbers()}",
        };
    }
}
