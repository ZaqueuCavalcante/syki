using Syki.Back.Features.Academic.CreateCourseOffering;
using Syki.Back.Features.Academic.CreateGrade;

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
        var courseCurriculumId = Guid.NewGuid();
        const string period = "2024.1";
        var shift = Shift.Matutino;

        // Act
        var oferta = new CourseOffering(institutionId, campusId, cursoId, courseCurriculumId, period, shift);

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
        var courseCurriculumId = Guid.NewGuid();
        const string period = "2024.1";
        var shift = Shift.Matutino;

        // Act
        var oferta = new CourseOffering(institutionId, campusId, cursoId, courseCurriculumId, period, shift);

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
        var courseCurriculumId = Guid.NewGuid();
        const string period = "2024.1";
        var shift = Shift.Matutino;

        // Act
        var oferta = new CourseOffering(institutionId, campusId, cursoId, courseCurriculumId, period, shift);

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
        var courseCurriculumId = Guid.NewGuid();
        const string period = "2024.1";
        var shift = Shift.Matutino;

        // Act
        var oferta = new CourseOffering(institutionId, campusId, cursoId, courseCurriculumId, period, shift);

        // Assert
        oferta.CourseId.Should().Be(cursoId);
    }

    [Test]
    public void Deve_criar_uma_oferta_com_grade_id_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var campusId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var courseCurriculumId = Guid.NewGuid();
        const string period = "2024.1";
        var shift = Shift.Matutino;

        // Act
        var oferta = new CourseOffering(institutionId, campusId, cursoId, courseCurriculumId, period, shift);

        // Assert
        oferta.CourseCurriculumId.Should().Be(courseCurriculumId);
    }

    [Test]
    public void Deve_criar_uma_oferta_com_periodo_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var campusId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var courseCurriculumId = Guid.NewGuid();
        const string period = "2024.1";
        var shift = Shift.Matutino;

        // Act
        var oferta = new CourseOffering(institutionId, campusId, cursoId, courseCurriculumId, period, shift);

        // Assert
        oferta.Period.Should().Be(period);
    }

    [Test]
    public void Deve_criar_uma_oferta_com_turno_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var campusId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var courseCurriculumId = Guid.NewGuid();
        const string period = "2024.1";
        var shift = Shift.Matutino;

        // Act
        var oferta = new CourseOffering(institutionId, campusId, cursoId, courseCurriculumId, period, shift);

        // Assert
        oferta.Shift.Should().Be(shift);
    }

    [Test]
    public void Deve_converter_a_oferta_corretamente_pro_out()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var campusId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        const string period = "2024.1";
        var shift = Shift.Matutino;

        var grade = new Grade(institutionId, cursoId, "Grade de ADS - 1.0");

        var oferta = new CourseOffering(institutionId, campusId, cursoId, grade.Id, period, shift)
        {
            Campus = new(institutionId, "Agreste I", "Caruaru - PE"),
            Course = new(institutionId, "ADS", CourseType.Bacharelado),
            CourseCurriculum = grade,
        };

        // Act
        var CourseOfferingOut = oferta.ToOut();

        // Assert
        CourseOfferingOut.Id.Should().Be(oferta.Id);
        CourseOfferingOut.Campus.Should().Be(oferta.Campus.Name);
        CourseOfferingOut.Curso.Should().Be(oferta.Course.Name);
        CourseOfferingOut.CourseCurriculumId.Should().Be(oferta.CourseCurriculumId);
        CourseOfferingOut.Grade.Should().Be(oferta.CourseCurriculum.Name);
        CourseOfferingOut.Period.Should().Be(oferta.Period);
        CourseOfferingOut.Shift.Should().Be(oferta.Shift);
    }

    [Test]
    public void Deve_converter_o_get_line_correto()
    {
        // Arrange
        var CourseOfferingOut = new CourseOfferingOut
        {
            Grade = "Grade de ADS - 1.0",
            Campus = "Agreste I",
            Period = "2024.1",
            Shift = Shift.Matutino,
        };

        // Act
        var getLine = CourseOfferingOut.ToString();

        // Assert
        getLine.Should().Be("Grade de ADS - 1.0 | Agreste I | 2024.1 | Matutino");
    }
}
