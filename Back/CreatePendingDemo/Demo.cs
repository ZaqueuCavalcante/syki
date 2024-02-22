using Syki.Shared;
using Syki.Back.Exceptions;

namespace Syki.Back.CreatePendingDemo;

public class Demo
{
    public Guid Id { get; }
    public string Email { get; }
    public DateOnly? Start { get; private set; }
    public DateOnly? End { get; private set; }

    public Demo(string email)
    {
        Id = Guid.NewGuid();
        if (!email.IsValidEmail())
            Throw.DE016.Now();
        Email = email.ToLower();
    }

    public void Setup()
    {
        if (Start != null)
            Throw.DE025.Now();

        Start = DateOnly.FromDateTime(DateTime.Now);
        End = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
    }
}
