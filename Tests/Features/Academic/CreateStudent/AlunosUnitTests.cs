using Syki.Back.Features.Academic.CreateStudent;

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
        var aluno = new Student(userId, institutionId, "Zaqueu", ofertaId);

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
        var aluno = new Student(userId, institutionId, name, ofertaId);

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
        var aluno = new Student(userId, institutionId, name, ofertaId);

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
        var aluno = new Student(userId, institutionId, name, ofertaId);

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
        var aluno = new Student(userId, institutionId, name, ofertaId);

        // Assert
        aluno.CourseOfferingId.Should().Be(ofertaId);
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
        var aluno = new Student(userId, institutionId, name, ofertaId);

        // Assert
        aluno.EnrollmentCode.Should().HaveLength(12);
        aluno.EnrollmentCode.Should().StartWith(DateTime.Now.Year.ToString());
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
        var aluna = new Student(userId, institutionId, "Maria", ofertaId);
        var aluno = new Student(userId, institutionId, "Zaqueu", ofertaId);

        // Assert
        aluna.EnrollmentCode.Should().NotBeSameAs(aluno.EnrollmentCode);
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
        Action act = () => new Student(userId, institutionId, name, ofertaId);

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
        Action act = () => new Student(userId, institutionId, name, ofertaId);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE000);
    }

    [Test]
    public void Deve_converter_o_aluno_corretamente_pro_out_sem_oferta()
    {
        // Arrange
        var aluno = new Student(Guid.NewGuid(), Guid.NewGuid(), "Zaqueu", Guid.NewGuid());

        // Act
        var alunoOut = aluno.ToOut();

        // Assert
        alunoOut.Id.Should().Be(aluno.Id);
        alunoOut.CourseOfferingId.Should().Be(aluno.CourseOfferingId);
        alunoOut.Name.Should().Be(aluno.Name);
        alunoOut.CourseOffering.Should().Be("-");
        alunoOut.EnrollmentCode.Should().Be(aluno.EnrollmentCode);
    }

    [Test]
    public void Deve_converter_o_aluno_corretamente_pro_out_com_oferta()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        var campusId = Guid.NewGuid();
        var courseId = Guid.NewGuid();
        var courseCurriculumId = Guid.NewGuid();
        const string period = "2024.1";
        var shift = Shift.Matutino;

        var aluno = new Student(userId, institutionId, "Zaqueu", Guid.NewGuid())
        {
            CourseOffering = new(institutionId, campusId, courseId, courseCurriculumId, period, shift)
            {
                Course = new(institutionId, "Direito", CourseType.Doutorado)
            }
        };

        // Act
        var alunoOut = aluno.ToOut();

        // Assert
        alunoOut.Id.Should().Be(aluno.Id);
        alunoOut.CourseOfferingId.Should().Be(aluno.CourseOfferingId);
        alunoOut.Name.Should().Be(aluno.Name);
        alunoOut.CourseOffering.Should().Be("Direito");
        alunoOut.EnrollmentCode.Should().Be(aluno.EnrollmentCode);
    }
}
