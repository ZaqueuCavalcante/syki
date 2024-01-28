using Syki.Shared;

namespace Syki.Back.Domain;

public class Horario
{
    public Guid Id { get; set; }
    public Guid TurmaId { get; set; }
    public Dia Dia { get; set; }
    public List<Hora> Horas { get; set; }

    public Horario(
        Dia dia,
        List<Hora> horas
    ) {
        Id = Guid.NewGuid();
        Dia = dia;
        Horas = horas;
    }
}
