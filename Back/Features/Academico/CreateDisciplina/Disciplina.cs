namespace Syki.Back.Features.Academico.CreateDisciplina;

public class Disciplina
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Nome { get; set; }
    public string Code { get; set; }
    public List<CursoDisciplina> Vinculos { get; set; }

    public Disciplina(
        Guid institutionId,
        string nome
    ) {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
        Nome = nome;
        Code = $"{Guid.NewGuid().ToString()[..8].ToUpper()}";
        Vinculos = [];
    }

    public DisciplinaOut ToOut()
    {
        return new DisciplinaOut
        {
            Id = Id,
            Nome = Nome,
            Code = Code,
            Cursos = Vinculos.ConvertAll(v => v.CursoId),
        };
    }
}
