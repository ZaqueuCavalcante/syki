namespace Syki.Dtos;

public class OfertaOut
{
    public Guid Id { get; set; }
    public string Campus { get; set; }
    public string Curso { get; set; }
    public Guid GradeId { get; set; }
    public string Grade { get; set; }
    public string Periodo { get; set; }
    public Turno Turno { get; set; }

    public string GetLine()
    {
        return $"{Grade} | {Campus} | {Periodo} | {Turno.GetDescription()}";
    }
}
