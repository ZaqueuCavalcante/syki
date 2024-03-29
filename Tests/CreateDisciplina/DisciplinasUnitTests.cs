using Syki.Back.CreateDisciplina;

namespace Syki.Tests.Unit;

public class DisciplinasUnitTests
{
    [Test]
    public void Deve_criar_uma_disciplina_com_id()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();

        // Act
        var disciplina = new Disciplina(faculdadeId, "Banco de Dados");

        // Assert
        disciplina.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_uma_disciplina_com_faculdade_id_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();

        // Act
        var disciplina = new Disciplina(faculdadeId, "Banco de Dados");

        // Assert
        disciplina.FaculdadeId.Should().Be(faculdadeId);
    }

    [Test]
    public void Deve_criar_uma_disciplina_com_nome_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string nome = "Banco de Dados";

        // Act
        var disciplina = new Disciplina(faculdadeId, nome);

        // Assert
        disciplina.Nome.Should().Be(nome);
    }

    [Test]
    public void Deve_converter_a_disciplina_corretamente_pro_out()
    {
        // Arrange
        var disciplina = new Disciplina(Guid.NewGuid(), "Banco de Dados");
        disciplina.Vinculos.Add(new() { CursoId = Guid.NewGuid() });
        disciplina.Vinculos.Add(new() { CursoId = Guid.NewGuid() });

        // Act
        var disciplinaOut = disciplina.ToOut();

        // Assert
        disciplinaOut.Id.Should().Be(disciplina.Id);
        disciplinaOut.Nome.Should().Be(disciplina.Nome);
        disciplinaOut.Cursos.Should().BeEquivalentTo(disciplina.Vinculos.ConvertAll(x => x.CursoId));
    }

    [Test]
    public void Deve_retornar_true_quando_for_a_mesma_disciplina()
    {
        // Arrange
        var disciplina = new Disciplina(Guid.NewGuid(), "Banco de Dados");
        var disciplinaOut1 = disciplina.ToOut();
        var disciplinaOut2 = disciplina.ToOut();

        // Act
        var equals = disciplinaOut1.Equals(disciplinaOut2);

        // Assert
        equals.Should().BeTrue();
    }

    [Test]
    public void Deve_retornar_false_quando_nao_for_a_mesma_disciplina()
    {
        // Arrange
        var disciplina1 = new Disciplina(Guid.NewGuid(), "Banco de Dados");
        var disciplina2 = new Disciplina(Guid.NewGuid(), "Banco de Dados");
        var disciplinaOut1 = disciplina1.ToOut();
        var disciplinaOut2 = disciplina2.ToOut();

        // Act
        var equals = disciplinaOut1.Equals(disciplinaOut2);

        // Assert
        equals.Should().BeFalse();
    }

    [Test]
    public void Deve_retornar_false_quando_a_outra_disciplina_for_nula()
    {
        // Arrange
        var disciplina = new Disciplina(Guid.NewGuid(), "Banco de Dados");
        var disciplinaOut = disciplina.ToOut();

        // Act
        var equals = disciplinaOut.Equals(null);

        // Assert
        equals.Should().BeFalse();
    }

    [Test]
    public void Deve_retornar_o_hash_code_correto()
    {
        // Arrange
        var disciplinaOut = new DisciplinaOut { Id = Guid.Parse("ef45239e-0d02-4eb0-b759-47331cfd1a8e") };

        // Act
        var hashCode = disciplinaOut.GetHashCode();

        // Assert
        hashCode.Should().Be(4523_9002);
    }

    [Test]
    public void Deve_retornar_o_nome_da_disciplina_como_to_string()
    {
        // Arrange
        var disciplinaOut = new DisciplinaOut { Nome = "Banco de Dados" };

        // Act
        var nome = disciplinaOut.ToString();

        // Assert
        nome.Should().Be("Banco de Dados");
    }
}
