namespace Syki.Shared;

public class UserOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Guid InstitutionId { get; set; }
    public string Institution { get; set; }
    public string Role { get; set; }
    public bool Online { get; set; }
    public int Connections { get; set; }

    public static IEnumerable<(string, UserOut)> GetExamples() =>
    [
        ("Exemplo",
        new UserOut
        {
            Id = Guid.CreateVersion7(),
            Name = "Zaqueu Cavalcante",
            Email = "zaqueu.cavalcante@gmail.com",
            Password = "M1@Str0ngP4ssword#",
            InstitutionId = Guid.CreateVersion7(),
            Institution = "Universidade Federal Caruaruense",
            Role = UserRole.Student.ToString()
        }),
    ];
}
