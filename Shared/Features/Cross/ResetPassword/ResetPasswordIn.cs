namespace Syki.Shared;

public class ResetPasswordIn : IApiDto<ResetPasswordIn>
{
    public string? Token { get; set; }
    public string Password { get; set; }

    public static IEnumerable<(string, ResetPasswordIn)> GetExamples() =>
    [
        ("Exemplo",
        new ResetPasswordIn
        {
            Token = Guid.CreateVersion7().ToString(),
            Password = "M1@Str0ngP4ssword#"
        })
    ];
}
