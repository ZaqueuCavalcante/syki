using Syki.Shared;

namespace Syki.Back.Domain;

public class Livro
{
    public Guid Id { get; set; }

    public Guid FaculdadeId { get; set; }

    public string Titulo { get; set; }

    public Livro(Guid faculdadeId, string titulo)
    {
        Id = Guid.NewGuid();
        FaculdadeId = faculdadeId;
        Titulo = titulo;
    }

    public LivroOut ToOut()
    {
        return new LivroOut
        {
            Id = Id,
            Titulo = Titulo,
        };
    }
}
