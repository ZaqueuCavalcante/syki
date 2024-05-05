using Syki.Back.CreateOferta;
using Syki.Back.Features.Academico.CreateGrade;

namespace Syki.Tests.Unit;

public class OfertasUnitTests
{
    [Test]
    public void Deve_criar_uma_oferta_com_id()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var campusId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var gradeId = Guid.NewGuid();
        const string periodo = "2024.1";
        var turno = Turno.Matutino;

        // Act
        var oferta = new Oferta(institutionId, campusId, cursoId, gradeId, periodo, turno);

        // Assert
        oferta.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_uma_oferta_com_institution_id_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var campusId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var gradeId = Guid.NewGuid();
        const string periodo = "2024.1";
        var turno = Turno.Matutino;

        // Act
        var oferta = new Oferta(institutionId, campusId, cursoId, gradeId, periodo, turno);

        // Assert
        oferta.InstitutionId.Should().Be(institutionId);
    }

    [Test]
    public void Deve_criar_uma_oferta_com_campus_id_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var campusId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var gradeId = Guid.NewGuid();
        const string periodo = "2024.1";
        var turno = Turno.Matutino;

        // Act
        var oferta = new Oferta(institutionId, campusId, cursoId, gradeId, periodo, turno);

        // Assert
        oferta.CampusId.Should().Be(campusId);
    }

    [Test]
    public void Deve_criar_uma_oferta_com_curso_id_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var campusId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var gradeId = Guid.NewGuid();
        const string periodo = "2024.1";
        var turno = Turno.Matutino;

        // Act
        var oferta = new Oferta(institutionId, campusId, cursoId, gradeId, periodo, turno);

        // Assert
        oferta.CursoId.Should().Be(cursoId);
    }

    [Test]
    public void Deve_criar_uma_oferta_com_grade_id_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var campusId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var gradeId = Guid.NewGuid();
        const string periodo = "2024.1";
        var turno = Turno.Matutino;

        // Act
        var oferta = new Oferta(institutionId, campusId, cursoId, gradeId, periodo, turno);

        // Assert
        oferta.GradeId.Should().Be(gradeId);
    }

    [Test]
    public void Deve_criar_uma_oferta_com_periodo_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var campusId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var gradeId = Guid.NewGuid();
        const string periodo = "2024.1";
        var turno = Turno.Matutino;

        // Act
        var oferta = new Oferta(institutionId, campusId, cursoId, gradeId, periodo, turno);

        // Assert
        oferta.Periodo.Should().Be(periodo);
    }

    [Test]
    public void Deve_criar_uma_oferta_com_turno_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var campusId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var gradeId = Guid.NewGuid();
        const string periodo = "2024.1";
        var turno = Turno.Matutino;

        // Act
        var oferta = new Oferta(institutionId, campusId, cursoId, gradeId, periodo, turno);

        // Assert
        oferta.Turno.Should().Be(turno);
    }

    [Test]
    public void Deve_converter_a_oferta_corretamente_pro_out()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var campusId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        const string periodo = "2024.1";
        var turno = Turno.Matutino;

        var grade = new Grade(institutionId, cursoId, "Grade de ADS - 1.0");

        var oferta = new Oferta(institutionId, campusId, cursoId, grade.Id, periodo, turno)
        {
            Campus = new(institutionId, "Agreste I", "Caruaru - PE"),
            Curso = new(institutionId, "ADS", CourseType.Bacharelado),
            Grade = grade,
        };

        // Act
        var ofertaOut = oferta.ToOut();

        // Assert
        ofertaOut.Id.Should().Be(oferta.Id);
        ofertaOut.Campus.Should().Be(oferta.Campus.Name);
        ofertaOut.Curso.Should().Be(oferta.Curso.Name);
        ofertaOut.GradeId.Should().Be(oferta.GradeId);
        ofertaOut.Grade.Should().Be(oferta.Grade.Name);
        ofertaOut.Periodo.Should().Be(oferta.Periodo);
        ofertaOut.Turno.Should().Be(oferta.Turno);
    }

    [Test]
    public void Deve_converter_o_get_line_correto()
    {
        // Arrange
        var ofertaOut = new OfertaOut
        {
            Grade = "Grade de ADS - 1.0",
            Campus = "Agreste I",
            Periodo = "2024.1",
            Turno = Turno.Matutino,
        };

        // Act
        var getLine = ofertaOut.ToString();

        // Assert
        getLine.Should().Be("Grade de ADS - 1.0 | Agreste I | 2024.1 | Matutino");
    }
}
