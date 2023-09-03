using Syki.Exceptions;
using Syki.Extensions;

namespace Syki.Domain;

public class Aluno
{
    public long Id { get; set; }
    
    public long FaculdadeId { get; set; }

    public long CursoId { get; set; }

    public string Nome { get; set; }

    public string Matricula { get; set; }

    public Aluno(string nome)
    {
        SetNome(nome);
        Matricula = $"{DateTime.Now.Year}{Guid.NewGuid().ToString()[..8]}";
    }

    public void SetNome(string nome)
    {
        if (nome.IsEmpty() || nome.Length < 3)
        {
            throw new DomainException(ExceptionMessages.DE0000);
        }

        Nome = nome;
    }
}
