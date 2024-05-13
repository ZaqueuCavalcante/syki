using Syki.Back.Features.Academic.CreateCampus;
using Syki.Back.Features.Academic.CreateCourse;
using Syki.Back.Features.Academic.CreateTeacher;
using Syki.Back.Features.Cross.FinishUserRegister;
using Syki.Back.Features.Academic.CreateDiscipline;
using Syki.Back.Features.Academic.CreateAcademicPeriod;
using Syki.Back.Features.Academic.CreateCourseOffering;
using Syki.Back.Features.Academic.CreateCourseCurriculum;

namespace Syki.Daemon.Tasks;

public class SeedInstitutionDataHandler(SykiDbContext ctx, CreateTeacherService service) : ISykiTaskHandler<SeedInstitutionData>
{
    public async Task Handle(SeedInstitutionData task)
    {
        var id = task.InstitutionId;
        var institution = await ctx.Institutions.FirstAsync(f => f.Id == id);

        var year = DateTime.Now.Year;
        institution.AcademicPeriods =
        [
            new AcademicPeriod($"{year}.1", id, new DateOnly(year, 02, 01), new DateOnly(year, 06, 01)),
            new AcademicPeriod($"{year}.2", id, new DateOnly(year, 07, 01), new DateOnly(year, 12, 01)),
        ];

        institution.Campi =
        [
            new Campus(id, "Garoa", "Garanhuns - PE"),
            new Campus(id, "Sertão", "Petrolina - PE"),
            new Campus(id, "Agreste", "Caruaru - PE"),
            new Campus(id, "Suassuna", "Recife - PE"),
        ];

        institution.Courses =
        [
            new Course(id, "Administração", CourseType.Mestrado),
            new Course(id, "Análise e Desenvolvimento de Sistemas", CourseType.Bacharelado),
            new Course(id, "Arquitetura e Urbanismo", CourseType.Tecnologo),
            new Course(id, "Ciência da Computação", CourseType.Bacharelado),
            new Course(id, "Direito", CourseType.Licenciatura),
            new Course(id, "Engenharia Civil", CourseType.Bacharelado),
            new Course(id, "Engenharia Mecânica", CourseType.Bacharelado),
            new Course(id, "Engenharia de Produção", CourseType.PosDoutorado),
            new Course(id, "Pedagogia", CourseType.Licenciatura),
        ];

        institution.Disciplines =
        [
            new Discipline(id, "Matemática Discreta"),
            new Discipline(id, "Introdução ao Desenvolvimento Web"),
            new Discipline(id, "Design de Interação Humano-Máquina"),
            new Discipline(id, "Introdução à Redes de Computadores"),
            new Discipline(id, "Pensamento Computacional e Algoritmos"),
            new Discipline(id, "Projeto Integrador I: Concepção e Prototipação"),
            //
            new Discipline(id, "Banco de Dados"),
            new Discipline(id, "Estrutura de Dados"),
            new Discipline(id, "Informática e Sociedade"),
            new Discipline(id, "Programação Orientada a Objetos"),
            new Discipline(id, "Projeto Integrador II: Modelagem de Banco de Dados"),
            new Discipline(id, "Arquitetura de Computadores e Sistemas Operacionais"),
            //
            new Discipline(id, "Estatística Aplicada"),
            new Discipline(id, "Arquitetura de Software"),
            new Discipline(id, "Análise e Projeto de Software"),
            new Discipline(id, "Computação em Nuvem e Web Services"),
            new Discipline(id, "Configuração e Manutenção de Software"),
            new Discipline(id, "Projeto Integrador III: Desenvolvimento Full Stack"),
            //
            new Discipline(id, "Sistemas Distribuídos"),
            new Discipline(id, "Inovação e Empreendedorismo"),
            new Discipline(id, "Análise e Visualização de Dados"),
            new Discipline(id, "Desenvolvimentos de Aplicações Móveis"),
            new Discipline(id, "Gestão de Projetos e Governança de TI"),
            new Discipline(id, "Projeto Integrador IV: Aplicações Móveis"),
            //
            new Discipline(id, "Libras"),
            new Discipline(id, "Sistemas Embarcados"),
            new Discipline(id, "Big Data e Data Science"),
            new Discipline(id, "Inteligência Artificial"),
            new Discipline(id, "Segurança da Informação"),
            new Discipline(id, "Testes e Verificação de Software"),
            new Discipline(id, "Projeto Integrador V: Sistemas Inteligentes"),
            //
            //
            new Discipline(id, "Direito e Economia"),
            new Discipline(id, "Introdução ao Direito"),
            new Discipline(id, "História das Instituições Jurídicas"),
            new Discipline(id, "Teoria do Estado, Política e Direito"),
            new Discipline(id, "Sociologia Jurídica"),
            new Discipline(id, "Antropologia Jurídica"),
            //
            new Discipline(id, "Direito Civil I (parte geral)"),
            new Discipline(id, "Direito Constitucional"),
            new Discipline(id, "Direito Financeiro"),
            new Discipline(id, "Direito Penal I (parte geral)"),
            new Discipline(id, "Filosofia Geral e Jurídica"),
            //
            new Discipline(id, "Direito Civil II (obrigações e contratos)"),
            new Discipline(id, "Direito Administrativo"),
            new Discipline(id, "Direito Penal II (teoria da pena)"),
            new Discipline(id, "Direito Internacional Público"),
            new Discipline(id, "Teoria Geral do Processo"),
            new Discipline(id, "Hermenêutica Jurídica"),
            //
            new Discipline(id, "Direito Civil III (contratos em espécie)"),
            new Discipline(id, "Direito Civil IV (direitos reais)"),
            new Discipline(id, "Direito Processual Constitucional"),
            new Discipline(id, "Direito Processual III (crimes em espécie)"),
            new Discipline(id, "Direito Processual Civil I"),
            new Discipline(id, "Metodologia da Pesquisa"),
            new Discipline(id, "Estágio I - Laboratório de Prática Jurídica I"),
            //
            new Discipline(id, "Direito Civil V"),
            new Discipline(id, "Direito Empresarial I"),
            new Discipline(id, "Direito do Trabalho I"),
            new Discipline(id, "Direito Processual Penal I"),
            new Discipline(id, "Direito Processual Civil II"),
            new Discipline(id, "Estágio II - Laboratório de Prática Jurídica II"),
            new Discipline(id, "Estágio II - Serviço de Assistência Judiciária I"),
            //
            new Discipline(id, "Direito Empresarial II"),
            new Discipline(id, "Direito Tributário"),
            new Discipline(id, "Direito Internacional Privado"),
            new Discipline(id, "Direito Processual Penal II"),
            new Discipline(id, "Direito do Trabalho II"),
            new Discipline(id, "Ética (geral e jurídica)"),
            new Discipline(id, "Estágio III - Serviço de Assistência Judiciária II"),
            new Discipline(id, "Monografia Final"),
        ];

        institution.Courses[1].Disciplines = institution.Disciplines.Take(31).ToList(); // ADS
        institution.Courses[4].Disciplines = institution.Disciplines.Skip(31).ToList(); // Direito

        var gradeAds = new CourseCurriculum(id, institution.Courses[1].Id, "Grade ADS 1.0");
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[00].Id, 1, 5, 60));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[01].Id, 1, 4, 40));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[02].Id, 1, 5, 60));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[03].Id, 1, 5, 55));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[04].Id, 1, 4, 45));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[05].Id, 1, 3, 25));
        //
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[06].Id, 2, 5, 60));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[07].Id, 2, 4, 40));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[08].Id, 2, 5, 60));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[09].Id, 2, 5, 55));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[10].Id, 2, 5, 45));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[11].Id, 2, 2, 25));
        //
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[12].Id, 3, 5, 60));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[13].Id, 3, 4, 40));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[14].Id, 3, 5, 60));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[15].Id, 3, 5, 55));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[16].Id, 3, 5, 45));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[17].Id, 3, 2, 25));
        //
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[18].Id, 4, 5, 60));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[19].Id, 4, 4, 40));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[20].Id, 4, 5, 60));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[21].Id, 4, 5, 55));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[22].Id, 4, 5, 45));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[23].Id, 4, 2, 25));
        //
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[24].Id, 5, 5, 60));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[25].Id, 5, 4, 40));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[26].Id, 5, 5, 60));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[27].Id, 5, 5, 55));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[28].Id, 5, 5, 45));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[29].Id, 5, 6, 50));
        gradeAds.Links.Add(new CourseCurriculumDiscipline(institution.Disciplines[30].Id, 5, 2, 25));
        ctx.Add(gradeAds);

        var ofertaAds = new CourseOffering(
            id,
            institution.Campi[2].Id,
            institution.Courses[1].Id,
            gradeAds.Id,
            institution.AcademicPeriods[0].Id,
            Shift.Noturno
        );
        ctx.Add(ofertaAds);

        await service.Create(institution.Id, CreateTeacherIn.Demo("Davi Pessoa Ferraz"));
        await service.Create(institution.Id, CreateTeacherIn.Demo("Luciete Bezerra Alves"));
        await service.Create(institution.Id, CreateTeacherIn.Demo("Antonio Marques da Costa Júnior"));
        await service.Create(institution.Id, CreateTeacherIn.Demo("Paulo Marcelo Pedrosa de Almeida"));
        await service.Create(institution.Id, CreateTeacherIn.Demo("Josélia Pachêco de Santana"));
        await service.Create(institution.Id, CreateTeacherIn.Demo("Manuela Abath Valença"));

        await ctx.SaveChangesAsync();
    }
}
