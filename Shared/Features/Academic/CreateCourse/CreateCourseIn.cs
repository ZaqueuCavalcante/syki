namespace Syki.Shared;

public class CreateCourseIn
{
    /// <summary>
    /// Nome do curso
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Tipo do curso
    /// </summary>
    public CourseType? Type { get; set; }

    /// <summary>
    /// Nome das disciplinas que serão criadas junto com o curso
    /// </summary>
    public List<string> Disciplines { get; set; } = [];

    public static IEnumerable<(string, CreateCourseIn)> GetExamples() =>
    [
        ("Direito",
        new CreateCourseIn
        {
            Name = "Direito",
            Type = CourseType.Bacharelado,
            Disciplines = ["Direito Civil", "Direito Penal"],
        }),
        ("ADS",
        new CreateCourseIn
        {
            Name = "Análise e Desenvolvimento de Sistemas",
            Type = CourseType.Tecnologo,
            Disciplines = ["Programação Orientada a Objetos", "Banco de Dados"],
        }),
    ];
}
