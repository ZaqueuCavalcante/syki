namespace Estud.Back.Errors;

public class DisciplineNotFound : EstudError
{
    public static readonly DisciplineNotFound I = new();
    public override string Code { get; set; } = nameof(DisciplineNotFound);
    public override string Message { get; set; } = "Disciplina não encontrada.";
}

public class InvalidDisciplineName : EstudError
{
    public static readonly InvalidDisciplineName I = new();
    public override string Code { get; set; } = nameof(InvalidDisciplineName);
    public override string Message { get; set; } = "Nome de disciplina inválido.";
}

public class InvalidDisciplinesList : EstudError
{
    public static readonly InvalidDisciplinesList I = new();
    public override string Code { get; set; } = nameof(InvalidDisciplinesList);
    public override string Message { get; set; } = "Lista de disciplinas inválida.";
}

public class CourseDisciplineNotFound : EstudError
{
    public static readonly CourseDisciplineNotFound I = new();
    public override string Code { get; set; } = nameof(CourseDisciplineNotFound);
    public override string Message { get; set; } = "Vínculo entre disciplina e curso não encontrado.";
}
