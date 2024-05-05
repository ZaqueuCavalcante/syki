namespace Syki.Shared;

public class ProfessorIn
{
    public string Name { get; set; }
    public string Email { get; set; }

    public static ProfessorIn Demo(string name)
    {
        return new ProfessorIn { Name = name, Email = $"{Guid.NewGuid().ToString().OnlyNumbers()[..8]}@syki.demo.com" };
    }
}
