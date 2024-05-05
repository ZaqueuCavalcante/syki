using Syki.Back.CreateAluno;

namespace Syki.Tests.Unit;

public class AlunosUnitTests
{
    [Test]
    public void Deve_criar_um_aluno_com_id()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        var ofertaId = Guid.NewGuid();

        // Act
        var aluno = new Aluno(userId, institutionId, "Zaqueu", ofertaId);

        // Assert
        aluno.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_um_aluno_com_institution_id_correto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        const string name = "Zaqueu";
        var ofertaId = Guid.NewGuid();

        // Act
        var aluno = new Aluno(userId, institutionId, name, ofertaId);

        // Assert
        aluno.InstitutionId.Should().Be(institutionId);
    }

    [Test]
    public void Deve_criar_um_aluno_com_user_id_correto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        const string name = "Zaqueu";
        var ofertaId = Guid.NewGuid();

        // Act
        var aluno = new Aluno(userId, institutionId, name, ofertaId);

        // Assert
        aluno.Id.Should().Be(userId);
    }

    [Test]
    public void Deve_criar_um_aluno_com_nome_correto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        const string name = "Zaqueu";
        var ofertaId = Guid.NewGuid();

        // Act
        var aluno = new Aluno(userId, institutionId, name, ofertaId);

        // Assert
        aluno.Name.Should().Be(name);
    }

    [Test]
    public void Deve_criar_um_aluno_com_oferta_id_correto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        const string name = "Zaqueu";
        var ofertaId = Guid.NewGuid();

        // Act
        var aluno = new Aluno(userId, institutionId, name, ofertaId);

        // Assert
        aluno.OfertaId.Should().Be(ofertaId);
    }

    [Test]
    public void Deve_criar_um_aluno_com_matricula()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        const string name = "Zaqueu";
        var ofertaId = Guid.NewGuid();

        // Act
        var aluno = new Aluno(userId, institutionId, name, ofertaId);

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
        var institutionId = Guid.NewGuid();
        var ofertaId = Guid.NewGuid();

        // Act
        var aluna = new Aluno(userId, institutionId, "Maria", ofertaId);
        var aluno = new Aluno(userId, institutionId, "Zaqueu", ofertaId);

        // Assert
        aluna.Matricula.Should().NotBeSameAs(aluno.Matricula);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ValidNames))]
    public void Deve_criar_um_aluno_com_nome_valido(string name)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        var ofertaId = Guid.NewGuid();

        // Act
        Action act = () => new Aluno(userId, institutionId, name, ofertaId);

        // Assert
        act.Should().NotThrow<DomainException>();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidNames))]
    public void Nao_deve_criar_um_aluno_com_nome_invalido(string name)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        var ofertaId = Guid.NewGuid();

        // Act
        Action act = () => new Aluno(userId, institutionId, name, ofertaId);

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
        alunoOut.Name.Should().Be(aluno.Name);
        alunoOut.Oferta.Should().Be("-");
        alunoOut.Matricula.Should().Be(aluno.Matricula);
    }

    [Test]
    public void Deve_converter_o_aluno_corretamente_pro_out_com_oferta()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        var campusId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var gradeId = Guid.NewGuid();
        const string periodo = "2024.1";
        var turno = Turno.Matutino;

        var aluno = new Aluno(userId, institutionId, "Zaqueu", Guid.NewGuid())
        {
            Oferta = new(institutionId, campusId, cursoId, gradeId, periodo, turno)
            {
                Curso = new(institutionId, "Direito", CourseType.Doutorado)
            }
        };

        // Act
        var alunoOut = aluno.ToOut();

        // Assert
        alunoOut.Id.Should().Be(aluno.Id);
        alunoOut.OfertaId.Should().Be(aluno.OfertaId);
        alunoOut.Name.Should().Be(aluno.Name);
        alunoOut.Oferta.Should().Be("Direito");
        alunoOut.Matricula.Should().Be(aluno.Matricula);
    }
}
