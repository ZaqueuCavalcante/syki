using Syki.Dtos;

namespace Syki.Domain;

public class Professor
{
    public Guid Id { get; set; }
    
    public Guid FaculdadeId { get; set; }

    public string Nome { get; set; }

    public ProfessorOut ToOut()
    {
        return new ProfessorOut
        {
            Id = Id,
            Nome = Nome,
        };
    }
}
