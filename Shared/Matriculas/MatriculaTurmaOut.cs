namespace Syki.Shared;

public class MatriculaTurmaOut
{
    public Guid Id { get; set; }
    public string Disciplina { get; set; }
    public byte Periodo { get; set; }
    public byte Creditos { get; set; }
    public ushort CargaHoraria { get; set; }
    public string Professor { get; set; }
    public string Horario { get; set; }
    public string Alunos { get; set; }
}
