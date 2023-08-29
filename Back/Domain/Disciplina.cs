namespace Syki.Domain;

public class Disciplina
{
    public long Id { get; set; }

    public long FaculdadeId { get; set; }
    
    public string Nome { get; set; }

    public short Creditos { get; set; }

    public short CargaHoraria { get; set; }

    public Disciplina(
        string nome,
        long faculdadeId,
        short creditos,
        short cargaHoraria
    ) {
        Nome = nome;
        FaculdadeId = faculdadeId;
        Creditos = creditos;
        CargaHoraria = cargaHoraria;
    }
}
