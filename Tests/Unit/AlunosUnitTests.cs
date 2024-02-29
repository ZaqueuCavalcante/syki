using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;
using Syki.Back.Exceptions;
using Syki.Back.CreateAluno;

namespace Syki.Tests.Unit;

public class AlunosUnitTests
{
    [Test]
    public void Deve_criar_um_aluno_com_id()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var faculdadeId = Guid.NewGuid();
        var ofertaId = Guid.NewGuid();

        // Act
        var aluno = new Aluno(userId, faculdadeId, "Zaqueu", ofertaId);

        // Assert
        aluno.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_um_aluno_com_faculdade_id_correto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var faculdadeId = Guid.NewGuid();
        const string nome = "Zaqueu";
        var ofertaId = Guid.NewGuid();

        // Act
        var aluno = new Aluno(userId, faculdadeId, nome, ofertaId);

        // Assert
        aluno.InstitutionId.Should().Be(faculdadeId);
    }

    [Test]
    public void Deve_criar_um_aluno_com_user_id_correto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var faculdadeId = Guid.NewGuid();
        const string nome = "Zaqueu";
        var ofertaId = Guid.NewGuid();

        // Act
        var aluno = new Aluno(userId, faculdadeId, nome, ofertaId);

        // Assert
        aluno.Id.Should().Be(userId);
    }

    [Test]
    public void Deve_criar_um_aluno_com_nome_correto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var faculdadeId = Guid.NewGuid();
        const string nome = "Zaqueu";
        var ofertaId = Guid.NewGuid();

        // Act
        var aluno = new Aluno(userId, faculdadeId, nome, ofertaId);

        // Assert
        aluno.Nome.Should().Be(nome);
    }

    [Test]
    public void Deve_criar_um_aluno_com_oferta_id_correto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var faculdadeId = Guid.NewGuid();
        const string nome = "Zaqueu";
        var ofertaId = Guid.NewGuid();

        // Act
        var aluno = new Aluno(userId, faculdadeId, nome, ofertaId);

        // Assert
        aluno.OfertaId.Should().Be(ofertaId);
    }

    [Test]
    public void Deve_criar_um_aluno_com_matricula()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var faculdadeId = Guid.NewGuid();
        const string nome = "Zaqueu";
        var ofertaId = Guid.NewGuid();

        // Act
        var aluno = new Aluno(userId, faculdadeId, nome, ofertaId);

        // Assert
        aluno.Matricula.Should().HaveLength(12);
        aluno.Matricula.Should().StartWith(DateTime.Now.Year.ToString());
    }

    [Test]
    [Repeat(100)]
    public void Deve_criar_alunos_com_matriculas_diferentes()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var faculdadeId = Guid.NewGuid();
        var ofertaId = Guid.NewGuid();

        // Act
        var aluna = new Aluno(userId, faculdadeId, "Maria", ofertaId);
        var aluno = new Aluno(userId, faculdadeId, "Zaqueu", ofertaId);

        // Assert
        aluna.Matricula.Should().NotBeSameAs(aluno.Matricula);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ValidNames))]
    public void Deve_criar_um_aluno_com_nome_valido(string nome)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var faculdadeId = Guid.NewGuid();
        var ofertaId = Guid.NewGuid();

        // Act
        Action act = () => new Aluno(userId, faculdadeId, nome, ofertaId);

        // Assert
        act.Should().NotThrow<DomainException>();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidNames))]
    public void Nao_deve_criar_um_aluno_com_nome_invalido(string nome)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var faculdadeId = Guid.NewGuid();
        var ofertaId = Guid.NewGuid();

        // Act
        Action act = () => new Aluno(userId, faculdadeId, nome, ofertaId);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE000);
    }

    [Test]
    public void Deve_converter_o_aluno_corretamente_pro_out_sem_oferta()
    {
        // Arrange
        var aluno = new Aluno(Guid.NewGuid(), Guid.NewGuid(), "Zaqueu", Guid.NewGuid());

        // Act
        var alunoOut = aluno.ToOut();

        // Assert
        alunoOut.Id.Should().Be(aluno.Id);
        alunoOut.OfertaId.Should().Be(aluno.OfertaId);
        alunoOut.Nome.Should().Be(aluno.Nome);
        alunoOut.Oferta.Should().Be("-");
        alunoOut.Matricula.Should().Be(aluno.Matricula);
    }

    [Test]
    public void Deve_converter_o_aluno_corretamente_pro_out_com_oferta()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var faculdadeId = Guid.NewGuid();
        var campusId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var gradeId = Guid.NewGuid();
        const string periodo = "2024.1";
        var turno = Turno.Matutino;

        var aluno = new Aluno(userId, faculdadeId, "Zaqueu", Guid.NewGuid())
        {
            Oferta = new(faculdadeId, campusId, cursoId, gradeId, periodo, turno)
            {
                Curso = new(faculdadeId, "Direito", TipoDeCurso.Doutorado)
            }
        };

        // Act
        var alunoOut = aluno.ToOut();

        // Assert
        alunoOut.Id.Should().Be(aluno.Id);
        alunoOut.OfertaId.Should().Be(aluno.OfertaId);
        alunoOut.Nome.Should().Be(aluno.Nome);
        alunoOut.Oferta.Should().Be("Direito");
        alunoOut.Matricula.Should().Be(aluno.Matricula);
    }
}
