namespace Estud.Back.Features.Parents.GetParentDetails;

public class GetParentDetailsOut : IApiDto<GetParentDetailsOut>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Data de criação do usuário do responsável
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Alunos vinculados ao responsável
    /// </summary>
    public List<GetParentDetailsStudentOut> Students { get; set; } = [];

    public static IEnumerable<(string, GetParentDetailsOut)> GetExamples() =>
    [
        ("Exemplo", new GetParentDetailsOut
        {
            Id = 1,
            Name = "Ana Souza",
            Email = "ana.souza@gmail.com",
            PhoneNumber = "82988887777",
            CreatedAt = new DateTime(2026, 2, 10, 0, 0, 0, DateTimeKind.Utc),
            Students =
            [
                new GetParentDetailsStudentOut
                {
                    Id = 1,
                    Name = "Maria Souza",
                    Email = "maria@ufal.edu.br",
                    EnrollmentCode = "20251A2B3C4D",
                    Status = StudentStatus.Enrolled,
                    Relationship = ParentRelationship.Mother,
                    LinkStatus = ParentStudentStatus.Active,
                    RevokedByStudent = false,
                    LinkedAt = new DateTime(2026, 2, 10, 0, 0, 0, DateTimeKind.Utc),
                    Course = "Análise e Desenvolvimento de Sistemas",
                    Campus = "Campus Maceió",
                    Period = "2026.1",
                },
            ],
        }),
    ];
}

public class GetParentDetailsStudentOut
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string EnrollmentCode { get; set; }
    public StudentStatus Status { get; set; }

    /// <summary>
    /// Parentesco do responsável com o aluno
    /// </summary>
    public ParentRelationship Relationship { get; set; }

    /// <summary>
    /// Status do vínculo entre o responsável e o aluno
    /// </summary>
    public ParentStudentStatus LinkStatus { get; set; }

    /// <summary>
    /// Indica se o aluno revogou o acesso do responsável aos seus dados
    /// </summary>
    public bool RevokedByStudent { get; set; }

    public DateTime LinkedAt { get; set; }

    /// <summary>
    /// Curso da oferta atual do aluno. Nulo quando o aluno ainda não foi matriculado em nenhuma.
    /// </summary>
    public string? Course { get; set; }

    public string? Campus { get; set; }
    public string? Period { get; set; }
}
