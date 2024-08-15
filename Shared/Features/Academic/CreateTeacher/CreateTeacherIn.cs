namespace Syki.Shared;

public class CreateTeacherIn
{
    public string Name { get; set; }
    public string Email { get; set; }

    public static CreateTeacherIn Seed(string name)
    {
        return new CreateTeacherIn { Name = name, Email = $"professor.{Guid.NewGuid().ToString().OnlyNumbers()[..8]}@syki.seed.com" };
    }
}
