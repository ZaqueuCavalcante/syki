namespace Estud.Back.Errors;

public class ClassroomNotFound : EstudError
{
    public static readonly ClassroomNotFound I = new();
    public override string Code { get; set; } = nameof(ClassroomNotFound);
    public override string Message { get; set; } = "Sala de aula não encontrada.";
}

public class InvalidClassroomName : EstudError
{
    public static readonly InvalidClassroomName I = new();
    public override string Code { get; set; } = nameof(InvalidClassroomName);
    public override string Message { get; set; } = "Nome de sala de aula inválido.";
}

public class InvalidClassroomCapacity : EstudError
{
    public static readonly InvalidClassroomCapacity I = new();
    public override string Code { get; set; } = nameof(InvalidClassroomCapacity);
    public override string Message { get; set; } = "Capacidade inválida (deve ser maior que zero).";
}
