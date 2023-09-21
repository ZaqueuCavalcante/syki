using Syki.Dtos;

namespace Syki.Domain;

public class Professor
{
    public long Id { get; set; }
    
    public long FaculdadeId { get; set; }

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
