namespace Syki.Domain;

public class Disciplina
{
    public long Id { get; set; }

    public long FaculdadeId { get; set; }
    
    public string Nome { get; set; }

    public short Creditos { get; set; }

    public short CargaHoraria { get; set; }
}
