namespace Syki.Back.Features.Academico.CreateDisciplina;

public class Disciplina
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public List<CursoDisciplina> Vinculos { get; set; }

    public Disciplina(
        Guid institutionId,
        string name
    ) {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
        Name = name;
        Code = $"{Guid.NewGuid().ToString()[..8].ToUpper()}";
        Vinculos = [];
    }

    public DisciplinaOut ToOut()
    {
        return new DisciplinaOut
        {
            Id = Id,
            Name = Name,
            Code = Code,
            Cursos = Vinculos.ConvertAll(v => v.CursoId),
        };
    }
}
