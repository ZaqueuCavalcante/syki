namespace Syki.Shared;

public class ProfessorIn
{
    public string Nome { get; set; }
    public string Email { get; set; }

    public static ProfessorIn Demo(string nome)
    {
        return new ProfessorIn { Nome = nome, Email = $"{Guid.NewGuid().ToString().OnlyNumbers()[..8]}@syki.demo.com" };
    }
}
