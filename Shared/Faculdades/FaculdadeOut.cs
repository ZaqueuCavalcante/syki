namespace Syki.Shared;

public class FaculdadeOut
{
    public Guid Id { get; set; }
    public string Nome { get; set; }

    public override string ToString()
    {
        return Nome;
    }
}
