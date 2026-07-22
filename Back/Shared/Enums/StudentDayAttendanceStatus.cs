namespace Estud.Back.Shared;

/// <summary>
/// Status de frequência do Aluno em um dia do calendário
/// </summary>
public enum StudentDayAttendanceStatus
{
    /// <summary>
    /// Dia sem aula para o aluno: fim de semana, feriado, férias, recesso
    /// ou dia letivo em que o aluno não tem nenhuma aula agendada.
    /// </summary>
    [Description("Sem aula")]
    NoClass = 0,

    /// <summary>
    /// Dia letivo com aula do aluno, mas ainda sem frequência definida:
    /// aula futura ou aula passada cuja frequência ainda não foi lançada.
    /// </summary>
    [Description("Indefinido")]
    Undefined = 1,

    /// <summary>
    /// Dia letivo com presença do aluno.
    /// </summary>
    [Description("Presença")]
    Present = 2,

    /// <summary>
    /// Dia letivo com falta do aluno.
    /// </summary>
    [Description("Falta")]
    Absent = 3,
}
