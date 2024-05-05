using Syki.Back.CreateProfessor;

namespace Syki.Tests.Unit;

public class ProfessoresUnitTests
{
    [Test]
    public void Deve_criar_um_professor_com_id()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        const string name = "Chico Science";

        // Act
        var professor = new Professor(userId, institutionId, name);

        // Assert
        professor.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_um_professor_com_institution_id_correto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        const string name = "Chico Science";

        // Act
        var professor = new Professor(userId, institutionId, name);

        // Assert
        professor.InstitutionId.Should().Be(institutionId);
    }

    [Test]
    public void Deve_criar_um_professor_com_user_id_correto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        const string name = "Chico Science";

        // Act
        var professor = new Professor(userId, institutionId, name);

        // Assert
        professor.Id.Should().Be(userId);
    }

    [Test]
    public void Deve_criar_um_professor_com_nome_correto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        const string name = "Chico Science";

        // Act
        var professor = new Professor(userId, institutionId, name);

        // Assert
        professor.Name.Should().Be(name);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidNames))]
    public void Nao_deve_criar_um_professor_com_nome_invalido(string name)
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Act
        Action act = () => new Professor(userId, institutionId, name);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE001);
    }

    [Test]
    public void Deve_converter_o_professor_corretamente_pro_out()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        const string name = "Chico Science";
        var professor = new Professor(userId, institutionId, name);

        // Act
        var professorOut = professor.ToOut();

        // Assert
        professorOut.Id.Should().Be(professor.Id);
        professorOut.Name.Should().Be(professor.Name);
    }
}
