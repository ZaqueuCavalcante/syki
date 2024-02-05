using Syki.Shared;

namespace Syki.Front.Domain;

public class HorariosSelector
{
    public IDictionary<Dia, HorasOptions> Options { get; set; }
    public List<HorarioIn> Values { get; set; }

    public HorariosSelector()
    {
        Options = new Dictionary<Dia, HorasOptions>()
        {
            { Dia.Segunda, new HorasOptions() },
            { Dia.Terca, new HorasOptions() },
            { Dia.Quarta, new HorasOptions() },
            { Dia.Quinta, new HorasOptions() },
            { Dia.Sexta, new HorasOptions() },
            { Dia.Sabado, new HorasOptions() },
        };

        Values = [];
    }

    public List<Dia> GetDias()
    {
        return Options.Where(o => o.Value.Starts.Count > 0)
            .ToList().ConvertAll(x => x.Key);
    }

    public List<Hora> GetStarts(Dia? dia)
    {
        return dia != null ? Options[dia.Value].Starts : [];
    }

    public List<Hora> GetEnds(Dia? dia)
    {
        return dia != null ? Options[dia.Value].Ends : [];
    }

    public bool Select(HorarioIn horario)
    {
        if (!Values.Any(v => v.Id == horario.Id))
        {
            foreach (var value in Values)
            {
                if (value.Conflict(horario))
                    return false;
            }        
            Values.Add(horario);
        }

        Options[horario.Dia].Starts.RemoveAll(s => s >= horario.Start && s < horario.End);
        Options[horario.Dia].Ends.RemoveAll(s => s > horario.Start && s <= horario.End);

        return true;
    }
}

public class HorasOptions
{
    public List<Hora> Starts { get; set; } = [];
    public List<Hora> Ends { get; set; } = [];

    public HorasOptions()
    {
        foreach (Hora hora in Enum.GetValues<Hora>())
        {
            Starts.Add(hora);
            Ends.Add(hora);
        }

        Starts.RemoveAt(Starts.Count - 1);
        Ends.RemoveAt(0);
    }
}
