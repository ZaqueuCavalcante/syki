namespace Syki.Shared;

public class TurmaOut
{
    public Guid Id { get; set; }
    public string Disciplina { get; set; }
    public string Professor { get; set; }
    public string Periodo { get; set; }
    public Dia Dia { get; set; }
    public Hora Start { get; set; }
    public Hora End { get; set; }
}
