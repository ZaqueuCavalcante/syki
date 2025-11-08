namespace Syki.Shared;

public class StudentCreatedWebhookPayload : IApiDto<StudentCreatedWebhookPayload>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public StudentCreatedWebhookPayload() {}

    public StudentCreatedWebhookPayload(Guid id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public static IEnumerable<(string, StudentCreatedWebhookPayload)> GetExamples() =>
    [
        ("Exemplo", new(Guid.CreateVersion7(), "Gilberto Gil", "gilberto.gil@gmail.com")),
    ];
}
