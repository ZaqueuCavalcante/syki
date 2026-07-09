namespace Estud.Back.Errors;

public class TeacherNotFound : EstudError
{
    public static readonly TeacherNotFound I = new();
    public override string Code { get; set; } = nameof(TeacherNotFound);
    public override string Message { get; set; } = "Professor não encontrado.";
}

public class InvalidTeacherName : EstudError
{
    public static readonly InvalidTeacherName I = new();
    public override string Code { get; set; } = nameof(InvalidTeacherName);
    public override string Message { get; set; } = "Nome de professor inválido.";
}

public class TeacherNotAssignedToCampus : EstudError
{
    public static readonly TeacherNotAssignedToCampus I = new();
    public override string Code { get; set; } = nameof(TeacherNotAssignedToCampus);
    public override string Message { get; set; } = "Professor não está vinculado ao campus.";
}

public class TeacherNotAssignedToDiscipline : EstudError
{
    public static readonly TeacherNotAssignedToDiscipline I = new();
    public override string Code { get; set; } = nameof(TeacherNotAssignedToDiscipline);
    public override string Message { get; set; } = "Professor não está vinculado à disciplina.";
}
