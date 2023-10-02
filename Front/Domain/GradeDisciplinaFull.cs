namespace Front.Domain;

public class GradeDisciplinaFull
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public ushort? CargaHoraria { get; set; }
    public byte? Periodo { get; set; }
    public byte? Creditos { get; set; }
    public bool IsSelected { get; set; }
}
