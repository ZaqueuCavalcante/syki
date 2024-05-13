namespace Syki.Shared;

public class CreateTeacherIn
{
    public string Name { get; set; }
    public string Email { get; set; }

    public static CreateTeacherIn Demo(string name)
    {
        return new CreateTeacherIn { Name = name, Email = $"{Guid.NewGuid().ToString().OnlyNumbers()[..8]}@syki.demo.com" };
    }
}
