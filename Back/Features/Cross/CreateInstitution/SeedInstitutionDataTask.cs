using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.StartClasses;
using Syki.Back.Features.Academic.CreateTeacher;
using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Teacher.AddExamGradeNote;
using Syki.Back.Features.Academic.FinalizeClasses;
using Syki.Back.Features.Academic.CreateDiscipline;
using Syki.Back.Features.Academic.CreateCourseOffering;
using Syki.Back.Features.Teacher.CreateLessonAttendance;
using Syki.Back.Features.Academic.CreateCourseCurriculum;
using Syki.Back.Features.Student.CreateStudentEnrollment;
using Syki.Back.Features.Academic.CreateEnrollmentPeriod;
using Syki.Back.Features.Academic.UpdateEnrollmentPeriod;
using Syki.Back.Features.Academic.ReleaseClassesForEnrollment;

namespace Syki.Back.Features.Cross.CreateInstitution;

[SykiTaskDescription("Realizar seed de dados da instituição")]
public record SeedInstitutionDataTask(Guid InstitutionId) : ISykiTask;

public class SeedInstitutionDataTaskHandler(
    SykiDbContext ctx,
    CreateClassService createClassService,
    StartClassesService startClassesService,
    CreateStudentService createStudentService,
    CreateTeacherService createTeacherService,
    FinalizeClassesService finalizeClassesService,
    AddExamGradeNoteService addExamGradeNoteService,
    CreateEnrollmentPeriodService createEnrollmentPeriodService,
    UpdateEnrollmentPeriodService updateEnrollmentPeriodService,
    CreateLessonAttendanceService createLessonAttendanceService,
    CreateStudentEnrollmentService createStudentEnrollmentService,
    ReleaseClassesForEnrollmentService releaseClassesForEnrollmentService) : ISykiTaskHandler<SeedInstitutionDataTask>
{
    public async Task Handle(SeedInstitutionDataTask task)
    {
        if (Env.IsTesting()) return;

        var id = task.InstitutionId;
        var institution = await ctx.Institutions.FirstAsync(f => f.Id == id);

        AddAcademicPeriods(institution);
        AddCampi(institution);
        AddCourses(institution);

        var adsDisciplines = GetAdsDisciplines(id);
        var direitoDisciplines = GetDireitoDisciplines(id);

        institution.Disciplines = [];
        institution.Disciplines.AddRange(adsDisciplines);
        institution.Disciplines.AddRange(direitoDisciplines);

        institution.Courses[1].Disciplines = adsDisciplines;
        institution.Courses[4].Disciplines = direitoDisciplines;

        var adsCC = GetAdsCourseCurriculum(institution, adsDisciplines);
        ctx.Add(adsCC);
        var courseOfferingAds = new CourseOffering(
            id,
            institution.Campi[2].Id,
            institution.Courses[1].Id,
            adsCC.Id,
            institution.AcademicPeriods[0].Id,
            Shift.Noturno
        );
        ctx.Add(courseOfferingAds);

        var direitoCC = GetDireitoCourseCurriculum(institution, direitoDisciplines);
        ctx.Add(direitoCC);
        var courseOfferingDireito = new CourseOffering(
            id,
            institution.Campi[2].Id,
            institution.Courses[4].Id,
            direitoCC.Id,
            institution.AcademicPeriods[0].Id,
            Shift.Noturno
        );
        ctx.Add(courseOfferingDireito);

        var teachers = new List<Guid>()
        {
            (await createTeacherService.Create(institution.Id, CreateTeacherIn.Seed("Davi Pessoa Ferraz"))).GetSuccess().Id,
            (await createTeacherService.Create(institution.Id, CreateTeacherIn.Seed("Luciete Bezerra Alves"))).GetSuccess().Id,
            (await createTeacherService.Create(institution.Id, CreateTeacherIn.Seed("Paulo Marcelo Pedrosa de Almeida"))).GetSuccess().Id,
            (await createTeacherService.Create(institution.Id, CreateTeacherIn.Seed("Manuela Abath Valença"))).GetSuccess().Id,
        };
        var direiters = new List<Guid>()
        {
            (await createStudentService.Create(institution.Id, CreateStudentIn.Seed("Maria Júlia de Oliveira Melo", courseOfferingDireito.Id))).GetSuccess().Id,
            (await createStudentService.Create(institution.Id, CreateStudentIn.Seed("Everton Ian de Galhardo Filho", courseOfferingDireito.Id))).GetSuccess().Id,
            (await createStudentService.Create(institution.Id, CreateStudentIn.Seed("Alisson Aranda de Aguiar", courseOfferingDireito.Id))).GetSuccess().Id,
            (await createStudentService.Create(institution.Id, CreateStudentIn.Seed("Alma Celeste Maldonado Mendonça", courseOfferingDireito.Id))).GetSuccess().Id,
        };
        var adsers = new List<Guid>()
        {
            (await createStudentService.Create(institution.Id, CreateStudentIn.Seed("Zaqueu do Vale Cavalcante", courseOfferingAds.Id))).GetSuccess().Id,
            (await createStudentService.Create(institution.Id, CreateStudentIn.Seed("Marlene de Oliveira", courseOfferingAds.Id))).GetSuccess().Id,
            (await createStudentService.Create(institution.Id, CreateStudentIn.Seed("Simone Bezerra", courseOfferingDireito.Id))).GetSuccess().Id,
            (await createStudentService.Create(institution.Id, CreateStudentIn.Seed("Marcelo Lima Filho", courseOfferingAds.Id))).GetSuccess().Id,
            (await createStudentService.Create(institution.Id, CreateStudentIn.Seed("Josilda Aragão Paz", courseOfferingAds.Id))).GetSuccess().Id,
            (await createStudentService.Create(institution.Id, CreateStudentIn.Seed("Miguel Gomes da Silva", courseOfferingAds.Id))).GetSuccess().Id
        };

        // TURMAS 2025.1
        var classesIds = new List<Guid>();
        for (int i = 0; i < 6; i++)
        {
            var disciplineId = adsDisciplines[i].Id;
            var response = await createClassService.Create(id, new()
            {
                DisciplineId = disciplineId,
                TeacherId = teachers.PickRandom(),
                Period = institution.AcademicPeriods[0].Id,
                Vacancies = new List<int>{25, 30, 35}.PickRandom(),
                Schedules = [new() { Day = (Day)i, Start = Hour.H19_00, End = Hour.H22_00 }]
            });
            classesIds.Add(response.GetSuccess().Id);
        }

        var today = DateTime.Now.ToDateOnly();
        var firstDay = new DateTime(today.Year, 1, 1).ToDateOnly();
        var lastDay = new DateTime(today.Year, 12, 31).ToDateOnly();
        await createEnrollmentPeriodService.Create(id, new()
        {
            Id = institution.AcademicPeriods[0].Id,
            StartAt = firstDay,
            EndAt = lastDay,
        });

        await releaseClassesForEnrollmentService.Release(id, new() { Classes = classesIds });

        foreach (var item in adsers)
        {
            await createStudentEnrollmentService.Create(id, item, adsCC.Id, new() { Classes = classesIds });
        }

        await updateEnrollmentPeriodService.Update(id, institution.AcademicPeriods[0].Id, new() { StartAt = new DateTime(today.Year, 3, 1).ToDateOnly(), EndAt = new DateTime(today.Year, 3, 15).ToDateOnly() });

        await startClassesService.Start(id, new() { Classes = classesIds });

        // Chamadas + Notas
        var classes = await ctx.Classes.AsNoTracking()
            .Include(c => c.Lessons)
            .Include(c => c.Students)
            .Include(c => c.ExamGrades)
            .Where(c => c.InstitutionId == id && classesIds.Contains(c.Id))
            .ToListAsync();

        var random = new Random();
        foreach (var item in classes)
        {
            foreach (var lesson in item.Lessons.Where(l => l.Date < today))
            {
                await createLessonAttendanceService.Create(item.TeacherId, lesson.Id, new(item.Students.Select(s => s.Id).PickRandom(random.Next(3, 7)).ToList()));
            }

            foreach (var student in item.Students)
            {
                foreach (var examGrade in item.ExamGrades.Where(g => g.ClassId == item.Id && g.StudentId == student.Id))
                {
                    var note = Convert.ToDecimal(Math.Round(random.NextDouble()*10, 2));
                    await addExamGradeNoteService.Add(item.TeacherId, examGrade.Id, new(note < 4 ? note+5 : note));
                }
            }
        }

        await finalizeClassesService.Finalize(id, new() { Classes = classesIds });

        // ----------------------------------------------------------------------------------------

        // TURMAS 2025.2
        classesIds = [];
        for (int i = 6; i < 12; i++)
        {
            var disciplineId = adsDisciplines[i].Id;
            var response = await createClassService.Create(id, new()
            {
                DisciplineId = disciplineId,
                TeacherId = teachers.PickRandom(),
                Period = institution.AcademicPeriods[1].Id,
                Vacancies = new List<int>{25, 30, 35}.PickRandom(),
                Schedules = [new() { Day = (Day)(i-6), Start = Hour.H19_00, End = Hour.H22_00 }]
            });
            classesIds.Add(response.GetSuccess().Id);
        }

        today = DateTime.Now.ToDateOnly();
        await createEnrollmentPeriodService.Create(id, new()
        {
            Id = institution.AcademicPeriods[1].Id,
            StartAt = firstDay,
            EndAt = lastDay,
        });

        await releaseClassesForEnrollmentService.Release(id, new() { Classes = classesIds });

        foreach (var item in adsers)
        {
            await createStudentEnrollmentService.Create(id, item, adsCC.Id, new() { Classes = classesIds });
        }

        await updateEnrollmentPeriodService.Update(id, institution.AcademicPeriods[1].Id, new() { StartAt = today.AddDays(-20), EndAt = today.AddDays(-10) });

        await startClassesService.Start(id, new() { Classes = classesIds });

        // Chamadas + Notas
        classes = await ctx.Classes.AsNoTracking()
            .Include(c => c.Lessons)
            .Include(c => c.Students)
            .Include(c => c.ExamGrades)
            .Where(c => c.InstitutionId == id && classesIds.Contains(c.Id))
            .ToListAsync();

        random = new Random();
        foreach (var item in classes)
        {
            foreach (var lesson in item.Lessons.Where(l => l.Date < today))
            {
                await createLessonAttendanceService.Create(item.TeacherId, lesson.Id, new(item.Students.Select(s => s.Id).PickRandom(random.Next(3, 7)).ToList()));
            }

            foreach (var student in item.Students)
            {
                var examGrade = item.ExamGrades.First(g => g.ClassId == item.Id && g.StudentId == student.Id && g.ExamType == ExamType.N1);
                var note = Convert.ToDecimal(Math.Round(random.NextDouble()*10, 2));
                await addExamGradeNoteService.Add(item.TeacherId, examGrade.Id, new(note < 4 ? note+5 : note));
            }
        }

        await ctx.SaveChangesAsync();
    }

    private static void AddAcademicPeriods(Institution institution)
    {
        var year = DateTime.Now.Year;
        institution.AcademicPeriods =
        [
            new($"{year}.1", institution.Id, new DateOnly(year, 02, 01), new DateOnly(year, 06, 01)),
            new($"{year}.2", institution.Id, new DateOnly(year, 07, 03), new DateOnly(year, 12, 05)),
        ];
    }

    private static void AddCampi(Institution institution)
    {

        institution.Campi =
        [
            new(institution.Id, "Garoa", "Garanhuns - PE"),
            new(institution.Id, "Sertão", "Petrolina - PE"),
            new(institution.Id, "Agreste", "Caruaru - PE"),
            new(institution.Id, "Suassuna", "Recife - PE"),
        ];
    }
    
    private static void AddCourses(Institution institution)
    {
        institution.Courses =
        [
            new(institution.Id, "Administração", CourseType.Mestrado),
            new(institution.Id, "Análise e Desenvolvimento de Sistemas", CourseType.Bacharelado),
            new(institution.Id, "Arquitetura e Urbanismo", CourseType.Tecnologo),
            new(institution.Id, "Ciência da Computação", CourseType.Bacharelado),
            new(institution.Id, "Direito", CourseType.Bacharelado),
            new(institution.Id, "Engenharia Civil", CourseType.Bacharelado),
            new(institution.Id, "Engenharia Mecânica", CourseType.Bacharelado),
            new(institution.Id, "Engenharia de Produção", CourseType.PosDoutorado),
            new(institution.Id, "Pedagogia", CourseType.Licenciatura),
        ];
    }

    private static List<Discipline> GetAdsDisciplines(Guid id)
    {
        return
        [
            new(id, "Matemática Discreta"),
            new(id, "Pensamento Computacional e Algoritmos"),
            new(id, "Design de Interação Humano-Máquina"),
            new(id, "Introdução à Redes de Computadores"),
            new(id, "Introdução ao Desenvolvimento Web"),
            new(id, "Projeto Integrador I: Concepção e Prototipação"),
            //
            new(id, "Arquitetura de Computadores e Sistemas Operacionais"),
            new(id, "Banco de Dados"),
            new(id, "Estrutura de Dados"),
            new(id, "Informática e Sociedade"),
            new(id, "Programação Orientada a Objetos"),
            new(id, "Projeto Integrador II: Modelagem de Banco de Dados"),
            //
            new(id, "Análise e Projeto de Software"),
            new(id, "Arquitetura de Software"),
            new(id, "Computação em Nuvem e Web Services"),
            new(id, "Estatística Aplicada"),
            new(id, "Projeto Integrador III: Desenvolvimento Full Stack"),
            new(id, "Configuração e Manutenção de Software"),
            //
            new(id, "Inovação e Empreendedorismo"),
            new(id, "Análise e Visualização de Dados"),
            new(id, "Desenvolvimentos de Aplicações Móveis"),
            new(id, "Gestão de Projetos e Governança de TI"),
            new(id, "Projeto Integrador IV: Aplicações Móveis"),
            new(id, "Sistemas Distribuídos"),
            //
            new(id, "Big Data e Data Science"),
            new(id, "Inteligência Artificial"),
            new(id, "Libras"),
            new(id, "Projeto Integrador V: Sistemas Inteligentes"),
            new(id, "Segurança da Informação"),
            new(id, "Sistemas Embarcados"),
            new(id, "Testes e Verificação de Software")
        ];
    }

    private static List<Discipline> GetDireitoDisciplines(Guid id)
    {
        return
        [
            new(id, "Bases Filosóficas"),
            new(id, "Comunicação e Argumentação Jurídica"),
            new(id, "Homem, Sociedade e Direito"),
            new(id, "Política e Estado em Foco"),
            new(id, "Teoria Geral do Direito"),
            //
            new(id, "Constituição: Teoria e Lógica Central"),
            new(id, "Economia"),
            new(id, "Processo e Justiça em Foco: Teoria Geral e Sistema"),
            new(id, "Psicologia e Direito"),
            new(id, "Teoria do Crime"),
            new(id, "Teoria Geral do Direito Privado"),
            //
            new(id, "Das Relações Obrigacionais"),
            new(id, "Empresa: Teoria Geral"),
            new(id, "Organização do Estado e Carreiras Jurídicas em Foco"),
            new(id, "Processo de Conhecimento"),
            new(id, "Teoria da Pena"),
            //
            new(id, "Contratos Teoria e Prática em Foco"),
            new(id, "Crimes em Espécie I"),
            new(id, "Processo de Conhecimento II"),
            new(id, "Relações Individuais de Trabalho"),
            new(id, "Societário e Contratos Mercantis"),
            new(id, "Tópicos De Direito Empresarial e Societário"),
            //
            new(id, "Administração Pública: Teoria Geral"),
            new(id, "Crimes em Espécie II"),
            new(id, "Direito Civil das Coisas"),
            new(id, "Recursos Cíveis"),
            new(id, "Relações Coletivas de Trabalho"),
            new(id, "Soluções Adequadas de Conflitos em Foco"),
            //
            new(id, "Atuação na Área Cível em Foco"),
            new(id, "Direito das Famílias"),
            new(id, "Estado e Finanças Públicas"),
            new(id, "Execução Civil"),
            new(id, "Negócios Jurídicos Administrativos"),
            new(id, "Ordem e Previdência Social"),
            //
            new(id, "Atendimento na Área Cível em Foco"),
            new(id, "Direito Internacional"),
            new(id, "Processo Penal: Teoria Geral"),
            new(id, "Responsabilidade Civil e Consumidor"),
            new(id, "Sistema Tributário Nacional"),
            new(id, "Sucessões"),
            //
            new(id, "Atuação na Àrea Trabalhista em Foco"),
            new(id, "Deontologia e Ética"),
            new(id, "Direito Constitucional Avançado"),
            new(id, "Direito Processual do Trabalho"),
            new(id, "Pesquisa e Projeto "),
            new(id, "Processo Penal Especial"),
            new(id, "Tributos"),
            //
            new(id, "Atuação na Área Criminal em Foco"),
            new(id, "Direito Ambiental"),
            new(id, "Direito e Inovação"),
            new(id, "Hermenêutica Jurídica"),
            new(id, "Optativa I"),
            new(id, "Recuperação Judicial e Falência"),
            new(id, "Trabalho de Conclusão de Curso I"),
            //
            new(id, "Atuação na Área Pública em Foco"),
            new(id, "Direito da Criança e do Adolescente e do Idoso"),
            new(id, "Direitos Humanos"),
            new(id, "Optativa II"),
            new(id, "Optativa III"),
            new(id, "Processo Constitucional"),
            new(id, "Trabalho de Conclusão de Curso II"),
        ];
    }

    private static CourseCurriculum GetAdsCourseCurriculum(Institution institution, List<Discipline> disciplines)
    {
        var adsCC = new CourseCurriculum(institution.Id, institution.Courses[1].Id, "Grade ADS 1.0");
        adsCC.Links.Add(new(disciplines[00].Id, 1, 4, 72));
        adsCC.Links.Add(new(disciplines[01].Id, 1, 4, 72));
        adsCC.Links.Add(new(disciplines[02].Id, 1, 4, 72));
        adsCC.Links.Add(new(disciplines[03].Id, 1, 4, 72));
        adsCC.Links.Add(new(disciplines[04].Id, 1, 4, 72));
        adsCC.Links.Add(new(disciplines[05].Id, 1, 4, 60));
        //
        adsCC.Links.Add(new(disciplines[06].Id, 2, 4, 72));
        adsCC.Links.Add(new(disciplines[07].Id, 2, 4, 72));
        adsCC.Links.Add(new(disciplines[08].Id, 2, 4, 72));
        adsCC.Links.Add(new(disciplines[09].Id, 2, 4, 72));
        adsCC.Links.Add(new(disciplines[10].Id, 2, 4, 72));
        adsCC.Links.Add(new(disciplines[11].Id, 2, 4, 60));
        //
        adsCC.Links.Add(new(disciplines[12].Id, 3, 4, 72));
        adsCC.Links.Add(new(disciplines[13].Id, 3, 4, 72));
        adsCC.Links.Add(new(disciplines[14].Id, 3, 4, 72));
        adsCC.Links.Add(new(disciplines[15].Id, 3, 4, 72));
        adsCC.Links.Add(new(disciplines[16].Id, 3, 4, 60));
        adsCC.Links.Add(new(disciplines[17].Id, 3, 4, 72));
        //
        adsCC.Links.Add(new(disciplines[18].Id, 4, 4, 72));
        adsCC.Links.Add(new(disciplines[19].Id, 4, 4, 72));
        adsCC.Links.Add(new(disciplines[20].Id, 4, 4, 72));
        adsCC.Links.Add(new(disciplines[21].Id, 4, 4, 72));
        adsCC.Links.Add(new(disciplines[22].Id, 4, 4, 60));
        adsCC.Links.Add(new(disciplines[23].Id, 4, 4, 72));
        //
        adsCC.Links.Add(new(disciplines[24].Id, 5, 4, 72));
        adsCC.Links.Add(new(disciplines[25].Id, 5, 4, 72));
        adsCC.Links.Add(new(disciplines[26].Id, 5, 2, 30));
        adsCC.Links.Add(new(disciplines[27].Id, 5, 4, 60));
        adsCC.Links.Add(new(disciplines[28].Id, 5, 4, 72));
        adsCC.Links.Add(new(disciplines[29].Id, 5, 4, 72));
        adsCC.Links.Add(new(disciplines[30].Id, 5, 4, 72));

        return adsCC;
    }

    private static CourseCurriculum GetDireitoCourseCurriculum(Institution institution, List<Discipline> disciplines)
    {
        var direitoCC = new CourseCurriculum(institution.Id, institution.Courses[4].Id, "Grade Direito 1.0");

        direitoCC.Links.Add(new(disciplines[00].Id, 1, 4, 72));
        direitoCC.Links.Add(new(disciplines[01].Id, 1, 4, 72));
        direitoCC.Links.Add(new(disciplines[02].Id, 1, 4, 72));
        direitoCC.Links.Add(new(disciplines[03].Id, 1, 4, 72));
        direitoCC.Links.Add(new(disciplines[04].Id, 1, 4, 72));
        //
        direitoCC.Links.Add(new(disciplines[05].Id, 2, 4, 72));
        direitoCC.Links.Add(new(disciplines[06].Id, 2, 2, 36));
        direitoCC.Links.Add(new(disciplines[07].Id, 2, 4, 72));
        direitoCC.Links.Add(new(disciplines[08].Id, 2, 2, 36));
        direitoCC.Links.Add(new(disciplines[09].Id, 2, 4, 72));
        direitoCC.Links.Add(new(disciplines[10].Id, 2, 4, 72));
        //
        direitoCC.Links.Add(new(disciplines[11].Id, 3, 4, 72));
        direitoCC.Links.Add(new(disciplines[12].Id, 3, 4, 72));
        direitoCC.Links.Add(new(disciplines[13].Id, 3, 4, 72));
        direitoCC.Links.Add(new(disciplines[14].Id, 3, 4, 72));
        direitoCC.Links.Add(new(disciplines[15].Id, 3, 4, 72));
        //
        direitoCC.Links.Add(new(disciplines[16].Id, 4, 4, 72));
        direitoCC.Links.Add(new(disciplines[17].Id, 4, 4, 72));
        direitoCC.Links.Add(new(disciplines[18].Id, 4, 4, 72));
        direitoCC.Links.Add(new(disciplines[19].Id, 4, 4, 72));
        direitoCC.Links.Add(new(disciplines[20].Id, 4, 4, 72));
        direitoCC.Links.Add(new(disciplines[21].Id, 4, 4, 72));
        //
        direitoCC.Links.Add(new(disciplines[22].Id, 5, 4, 72));
        direitoCC.Links.Add(new(disciplines[23].Id, 5, 4, 72));
        direitoCC.Links.Add(new(disciplines[24].Id, 5, 4, 72));
        direitoCC.Links.Add(new(disciplines[25].Id, 5, 3, 60));
        direitoCC.Links.Add(new(disciplines[26].Id, 5, 3, 60));
        direitoCC.Links.Add(new(disciplines[27].Id, 5, 4, 72));
        //
        //
        direitoCC.Links.Add(new(disciplines[28].Id, 6, 4, 72));
        direitoCC.Links.Add(new(disciplines[29].Id, 6, 4, 72));
        direitoCC.Links.Add(new(disciplines[30].Id, 6, 4, 72));
        direitoCC.Links.Add(new(disciplines[31].Id, 6, 3, 60));
        direitoCC.Links.Add(new(disciplines[32].Id, 6, 4, 72));
        direitoCC.Links.Add(new(disciplines[33].Id, 6, 4, 72));
        //
        direitoCC.Links.Add(new(disciplines[34].Id, 7, 4, 72));
        direitoCC.Links.Add(new(disciplines[35].Id, 7, 4, 72));
        direitoCC.Links.Add(new(disciplines[36].Id, 7, 4, 72));
        direitoCC.Links.Add(new(disciplines[37].Id, 7, 3, 60));
        direitoCC.Links.Add(new(disciplines[38].Id, 7, 4, 72));
        direitoCC.Links.Add(new(disciplines[39].Id, 7, 4, 72));
        //
        direitoCC.Links.Add(new(disciplines[40].Id, 8, 4, 72));
        direitoCC.Links.Add(new(disciplines[41].Id, 8, 3, 72));
        direitoCC.Links.Add(new(disciplines[42].Id, 8, 2, 36));
        direitoCC.Links.Add(new(disciplines[43].Id, 8, 4, 72));
        direitoCC.Links.Add(new(disciplines[44].Id, 8, 2, 36));
        direitoCC.Links.Add(new(disciplines[45].Id, 8, 4, 72));
        direitoCC.Links.Add(new(disciplines[46].Id, 8, 4, 72));
        //
        direitoCC.Links.Add(new(disciplines[47].Id, 9, 2, 72));
        direitoCC.Links.Add(new(disciplines[48].Id, 9, 2, 36));
        direitoCC.Links.Add(new(disciplines[49].Id, 9, 2, 36));
        direitoCC.Links.Add(new(disciplines[50].Id, 9, 2, 36));
        direitoCC.Links.Add(new(disciplines[51].Id, 9, 2, 72));
        direitoCC.Links.Add(new(disciplines[52].Id, 9, 4, 72));
        direitoCC.Links.Add(new(disciplines[53].Id, 9, 2, 36));
        //
        direitoCC.Links.Add(new(disciplines[54].Id, 10, 2, 72));
        direitoCC.Links.Add(new(disciplines[55].Id, 10, 3, 60));
        direitoCC.Links.Add(new(disciplines[56].Id, 10, 2, 40));
        direitoCC.Links.Add(new(disciplines[57].Id, 10, 2, 36));
        direitoCC.Links.Add(new(disciplines[58].Id, 10, 2, 36));
        direitoCC.Links.Add(new(disciplines[59].Id, 10, 2, 72));
        direitoCC.Links.Add(new(disciplines[60].Id, 10, 4, 36));

        return direitoCC;
    }
}
