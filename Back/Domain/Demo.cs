using Syki.Shared;

namespace Syki.Back.Domain;

public class Demo
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public DateOnly? Start { get; set; }
    public DateOnly? End { get; set; }

    public Demo(string email)
    {
        Id = Guid.NewGuid();
        Email = email.ToLower();
    }

    public void Setup()
    {
        Start = DateOnly.FromDateTime(DateTime.Now);
        End = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
    }

    public DemoOut ToOut()
    {
        return new DemoOut
        {
            Email = Email,
        };
    }
}
