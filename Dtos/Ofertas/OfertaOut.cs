namespace Syki.Dtos;

public class OfertaOut
{
    public long Id { get; set; }
    public string Campus { get; set; }
    public string Curso { get; set; }
    public string Grade { get; set; }
    public string Periodo { get; set; }
    public Turno Turno { get; set; }
}
