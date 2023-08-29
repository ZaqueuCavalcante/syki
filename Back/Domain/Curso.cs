namespace Syki.Domain;

public class Curso
{
    public long Id { get; set; }

    public long FaculdadeId { get; set; }
    
    public string Nome { get; set; }

    public Turno Turno { get; set; }

    public List<Disciplina> Disciplinas { get; set; }

    public Curso(string nome, long faculdadeId)
    {
        Nome = nome;
        FaculdadeId = faculdadeId;
    }
}
