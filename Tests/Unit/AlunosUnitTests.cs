using Syki.Tests.Base;
using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;
using Syki.Back.Exceptions;

namespace Syki.Tests.Unit;

public class AlunosUnitTests
{
    [Test]
    public void Deve_criar_um_aluno_com_id()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();

        // Act
        var aluno = new Aluno(faculdadeId, "Zaqueu");

        // Assert
        aluno.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_um_aluno_com_faculdade_id_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string nome = "Zaqueu";

        // Act
        var aluno = new Aluno(faculdadeId, nome);

        // Assert
        aluno.FaculdadeId.Should().Be(faculdadeId);
    }

    [Test]
    public void Deve_criar_um_aluno_com_nome_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string nome = "Zaqueu";

        // Act
        var aluno = new Aluno(faculdadeId, nome);

        // Assert
        aluno.Nome.Should().Be(nome);
    }

    [Test]
    public void Deve_criar_um_aluno_com_matricula()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string nome = "Zaqueu";

        // Act
        var aluno = new Aluno(faculdadeId, nome);

        // Assert
        aluno.Matricula.Should().HaveLength(12);
        aluno.Matricula.Should().StartWith(DateTime.Now.Year.ToString());
    }

    [Test]
    public void Deve_criar_alunos_com_matriculas_diferentes()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();

        // Act
        var aluna = new Aluno(faculdadeId, "Maria");
        var aluno = new Aluno(faculdadeId, "Zaqueu");

        // Assert
        aluna.Matricula.Should().NotBeSameAs(aluno.Matricula);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.InvalidNames))]
    public void Nao_deve_criar_um_aluno_com_nome_invalido(string nome)
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();

        // Act
        Action act = () => new Aluno(faculdadeId, nome);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(ExceptionMessages.DE0000);
    }
}
