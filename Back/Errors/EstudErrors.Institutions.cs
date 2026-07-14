namespace Estud.Back.Errors;

public class InvalidNoteLimit : EstudError
{
    public static readonly InvalidNoteLimit I = new();
    public override string Code { get; set; } = nameof(InvalidNoteLimit);
    public override string Message { get; set; } = "Nota limite inválida (deve estar entre 0 e 10).";
}

public class InvalidFrequencyLimit : EstudError
{
    public static readonly InvalidFrequencyLimit I = new();
    public override string Code { get; set; } = nameof(InvalidFrequencyLimit);
    public override string Message { get; set; } = "Frequência limite inválida (deve estar entre 0 e 100).";
}
