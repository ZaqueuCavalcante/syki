namespace Syki.Shared;

public class UserOut : IApiDto<UserOut>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
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
    PhoneNumber = "(88) 98888-0000",
    BirthDate = new DateTime(2000, 3, 15),
    Password = "M1@Str0ngP4ssword#",
    InstitutionId = Guid.CreateVersion7(),
    Institution = "Universidade Federal Caruaruense",
    Role = UserRole.Student.ToString(),
    Online = false,
    Connections = 0
})
    ];
}
