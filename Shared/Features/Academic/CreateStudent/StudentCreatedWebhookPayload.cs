namespace Syki.Shared;

public class StudentCreatedWebhookPayload : IApiDto<StudentCreatedWebhookPayload>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }


    public StudentCreatedWebhookPayload() {}

    public StudentCreatedWebhookPayload(Guid id, string name, string email, string phoneNumber, DateTime birthDate)
    {
        Id = id;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        BirthDate = birthDate;
    }

    public static IEnumerable<(string, StudentCreatedWebhookPayload)> GetExamples() =>
    [
        ("Exemplo", new(Guid.CreateVersion7(), "Gilberto Gil", "gilberto.gil@gmail.com", "(88) 99999-1234", new DateTime(1990, 1, 1))),
    ];
}
