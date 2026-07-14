namespace Estud.Back.Features.Students.CreateClassActivityWork;

public class CreateClassActivityWorkIn : IApiDto<CreateClassActivityWorkIn>
{
    /// <summary>
    /// Link da entrega (PDF, Doc, PPT).
    /// </summary>
    public string? Link { get; set; }

    public static IEnumerable<(string, CreateClassActivityWorkIn)> GetExamples() =>
    [
        ("Exemplo", new CreateClassActivityWorkIn { Link = "https://github.com/ZaqueuCavalcante/estud" }),
    ];
}
