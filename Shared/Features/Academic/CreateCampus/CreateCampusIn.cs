using System.ComponentModel.DataAnnotations;

namespace Syki.Shared;

public class CreateCampusIn
{
    /// <summary>
    /// Nome
    /// </summary>
    public string Name { get; set; }

    public BrazilState State { get; set; }

    /// <summary>
    /// Cidade
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// Capacidade total de alunos
    /// </summary>
    public int Capacity { get; set; }
}
