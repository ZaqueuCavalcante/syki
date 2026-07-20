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

public class ClassAndClassroomDifferentCampus : EstudError
{
    public static readonly ClassAndClassroomDifferentCampus I = new();
    public override string Code { get; set; } = nameof(ClassAndClassroomDifferentCampus);
    public override string Message { get; set; } = "A sala e a turma devem estar no mesmo campus.";
}

public class ClassroomCapacityExceeded : EstudError
{
    public static readonly ClassroomCapacityExceeded I = new();
    public override string Code { get; set; } = nameof(ClassroomCapacityExceeded);
    public override string Message { get; set; } = "A capacidade da sala é menor que o número de vagas da turma.";
}

public class ClassroomScheduleConflict : EstudError
{
    public static readonly ClassroomScheduleConflict I = new();
    public override string Code { get; set; } = nameof(ClassroomScheduleConflict);
    public override string Message { get; set; } = "Conflito de horário com outra turma na sala.";
}
