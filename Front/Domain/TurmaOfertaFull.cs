using Syki.Dtos;

namespace Syki.Front.Domain;

public class TurmaOfertaFull
{
    public Guid Id { get; set; }
    public string Campus { get; set; }
    public string Curso { get; set; }
    public string Grade { get; set; }
    public string Periodo { get; set; }
    public Turno Turno { get; set; }
    public bool IsSelected { get; set; }
}
