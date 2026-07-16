using Estud.Tests.Integration.Clients;
using Estud.Back.Features.Parents.CreateParent;
using Estud.Back.Features.CourseCurriculums.CreateCourseCurriculum;

namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Dev_Should_create_initial_institution_data_for_easy_development_debugging()
    {
        const string email = "zaqueu@gmail.com";
        var client = await _back.LoggedAsDirector(email: email);

        var data = new DevInstitutionData();

        await DevCreateCampi(client, data);
        await DevCreateCourses(client, data);
        await DevCreateAdsDisciplines(client, data);
        await DevCreateDireitoDisciplines(client, data);
        await DevCreateAdsCourseCurriculum(client, data);
        await DevCreateDireitoCourseCurriculum(client, data);
        await DevCreateAdsTeachers(client, data);
        await DevCreateDireitoTeachers(client, data);

        await DevCreateAcademicPeriods(client, data);

        await DevCreateCourseOfferings(client, data);
        await DevCreateStudents(client, data);
        await DevCreateParents(client, data);
    }

    private static async Task DevCreateCampi(TestsHttpClient client, DevInstitutionData data)
    {
        data.CampiIds.Add((await client.CreateCampus("Garoa", BrazilState.PE, "Garanhuns", 150)).Success.Id);
        data.CampiIds.Add((await client.CreateCampus("Sertão", BrazilState.PE, "Petrolina", 500)).Success.Id);
        data.CampiIds.Add((await client.CreateCampus("Agreste", BrazilState.PE, "Caruaru", 750)).Success.Id);
        data.CampiIds.Add((await client.CreateCampus("Suassuna", BrazilState.PE, "Recife", 1200)).Success.Id);
    }

    private static async Task DevCreateCourses(TestsHttpClient client, DevInstitutionData data)
    {
        await client.CreateCourse("Pedagogia", CourseType.Licenciatura);
        await client.CreateCourse("Administração", CourseType.Mestrado);
        await client.CreateCourse("Engenharia Civil", CourseType.Bacharelado);
        await client.CreateCourse("Engenharia Mecânica", CourseType.Bacharelado);
        await client.CreateCourse("Arquitetura e Urbanismo", CourseType.Tecnologo);
        await client.CreateCourse("Ciência da Computação", CourseType.Bacharelado);
        await client.CreateCourse("Engenharia de Produção", CourseType.PosDoutorado);

        data.DireitoCourseId = (await client.CreateCourse("Direito", CourseType.Bacharelado)).Success.Id;
        data.AdsCourseId = (await client.CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Bacharelado)).Success.Id;
    }

    private static async Task DevCreateAdsDisciplines(TestsHttpClient client, DevInstitutionData data)
    {
        string[] names =
        [
            // Período 1
            "Matemática Discreta",
            "Pensamento Computacional e Algoritmos",
            "Design de Interação Humano-Máquina",
            "Introdução à Redes de Computadores",
            "Introdução ao Desenvolvimento Web",
            "Projeto Integrador I: Concepção e Prototipação",
            // Período 2
            "Arquitetura de Computadores e Sistemas Operacionais",
            "Banco de Dados",
            "Estrutura de Dados",
            "Informática e Sociedade",
            "Programação Orientada a Objetos",
            "Projeto Integrador II: Modelagem de Banco de Dados",
            // Período 3
            "Análise e Projeto de Software",
            "Arquitetura de Software",
            "Computação em Nuvem e Web Services",
            "Estatística Aplicada",
            "Projeto Integrador III: Desenvolvimento Full Stack",
            "Configuração e Manutenção de Software",
            // Período 4
            "Inovação e Empreendedorismo",
            "Análise e Visualização de Dados",
            "Desenvolvimentos de Aplicações Móveis",
            "Gestão de Projetos e Governança de TI",
            "Projeto Integrador IV: Aplicações Móveis",
            "Sistemas Distribuídos",
            // Período 5
            "Big Data e Data Science",
            "Inteligência Artificial",
            "Libras",
            "Projeto Integrador V: Sistemas Inteligentes",
            "Segurança da Informação",
            "Sistemas Embarcados",
            "Testes e Verificação de Software",
        ];

        foreach (var name in names)
        {
            var result = await client.CreateDiscipline(name);
            data.AdsDisciplinesIds.Add(result.Success.Id);
        }

        await client.AddCourseDisciplines(data.AdsCourseId, data.AdsDisciplinesIds);
    }

    private static async Task DevCreateDireitoDisciplines(TestsHttpClient client, DevInstitutionData data)
    {
        string[] names =
        [
            // Período 1
            "Bases Filosóficas",
            "Comunicação e Argumentação Jurídica",
            "Homem, Sociedade e Direito",
            "Política e Estado em Foco",
            "Teoria Geral do Direito",
            // Período 2
            "Constituição: Teoria e Lógica Central",
            "Economia",
            "Processo e Justiça em Foco: Teoria Geral e Sistema",
            "Psicologia e Direito",
            "Teoria do Crime",
            "Teoria Geral do Direito Privado",
            // Período 3
            "Das Relações Obrigacionais",
            "Empresa: Teoria Geral",
            "Organização do Estado e Carreiras Jurídicas em Foco",
            "Processo de Conhecimento",
            "Teoria da Pena",
            // Período 4
            "Contratos Teoria e Prática em Foco",
            "Crimes em Espécie I",
            "Processo de Conhecimento II",
            "Relações Individuais de Trabalho",
            "Societário e Contratos Mercantis",
            "Tópicos De Direito Empresarial e Societário",
            // Período 5
            "Administração Pública: Teoria Geral",
            "Crimes em Espécie II",
            "Direito Civil das Coisas",
            "Recursos Cíveis",
            "Relações Coletivas de Trabalho",
            "Soluções Adequadas de Conflitos em Foco",
            // Período 6
            "Atuação na Área Cível em Foco",
            "Direito das Famílias",
            "Estado e Finanças Públicas",
            "Execução Civil",
            "Negócios Jurídicos Administrativos",
            "Ordem e Previdência Social",
            // Período 7
            "Atendimento na Área Cível em Foco",
            "Direito Internacional",
            "Processo Penal: Teoria Geral",
            "Responsabilidade Civil e Consumidor",
            "Sistema Tributário Nacional",
            "Sucessões",
            // Período 8
            "Atuação na Àrea Trabalhista em Foco",
            "Deontologia e Ética",
            "Direito Constitucional Avançado",
            "Direito Processual do Trabalho",
            "Pesquisa e Projeto",
            "Processo Penal Especial",
            "Tributos",
            // Período 9
            "Atuação na Área Criminal em Foco",
            "Direito Ambiental",
            "Direito e Inovação",
            "Hermenêutica Jurídica",
            "Optativa I",
            "Recuperação Judicial e Falência",
            "Trabalho de Conclusão de Curso I",
            // Período 10
            "Atuação na Área Pública em Foco",
            "Direito da Criança e do Adolescente e do Idoso",
            "Direitos Humanos",
            "Optativa II",
            "Optativa III",
            "Processo Constitucional",
            "Trabalho de Conclusão de Curso II",
        ];

        foreach (var name in names)
        {
            var result = await client.CreateDiscipline(name);
            data.DireitoDisciplinesIds.Add(result.Success.Id);
        }

        await client.AddCourseDisciplines(data.DireitoCourseId, data.DireitoDisciplinesIds);
    }

    private static async Task DevCreateAdsCourseCurriculum(TestsHttpClient client, DevInstitutionData data)
    {
        var d = data.AdsDisciplinesIds;
        List<CreateCourseCurriculumDisciplineIn> links =
        [
            new(d[00], 1, 4, 72), new(d[01], 1, 4, 72), new(d[02], 1, 4, 72),
            new(d[03], 1, 4, 72), new(d[04], 1, 4, 72), new(d[05], 1, 4, 60),
            //
            new(d[06], 2, 4, 72), new(d[07], 2, 4, 72), new(d[08], 2, 4, 72),
            new(d[09], 2, 4, 72), new(d[10], 2, 4, 72), new(d[11], 2, 4, 60),
            //
            new(d[12], 3, 4, 72), new(d[13], 3, 4, 72), new(d[14], 3, 4, 72),
            new(d[15], 3, 4, 72), new(d[16], 3, 4, 60), new(d[17], 3, 4, 72),
            //
            new(d[18], 4, 4, 72), new(d[19], 4, 4, 72), new(d[20], 4, 4, 72),
            new(d[21], 4, 4, 72), new(d[22], 4, 4, 60), new(d[23], 4, 4, 72),
            //
            new(d[24], 5, 4, 72), new(d[25], 5, 4, 72), new(d[26], 5, 2, 30),
            new(d[27], 5, 4, 60), new(d[28], 5, 4, 72), new(d[29], 5, 4, 72),
            new(d[30], 5, 4, 72),
        ];

        var result = await client.CreateCourseCurriculum(data.AdsCourseId, "Grade ADS 1.0", links);
        data.AdsCourseCurriculumId = result.Success.Id;
    }

    private static async Task DevCreateDireitoCourseCurriculum(TestsHttpClient client, DevInstitutionData data)
    {
        var d = data.DireitoDisciplinesIds;
        List<CreateCourseCurriculumDisciplineIn> links =
        [
            new(d[00], 1, 4, 72), new(d[01], 1, 4, 72), new(d[02], 1, 4, 72),
            new(d[03], 1, 4, 72), new(d[04], 1, 4, 72),
            //
            new(d[05], 2, 4, 72), new(d[06], 2, 2, 36), new(d[07], 2, 4, 72),
            new(d[08], 2, 2, 36), new(d[09], 2, 4, 72), new(d[10], 2, 4, 72),
            //
            new(d[11], 3, 4, 72), new(d[12], 3, 4, 72), new(d[13], 3, 4, 72),
            new(d[14], 3, 4, 72), new(d[15], 3, 4, 72),
            //
            new(d[16], 4, 4, 72), new(d[17], 4, 4, 72), new(d[18], 4, 4, 72),
            new(d[19], 4, 4, 72), new(d[20], 4, 4, 72), new(d[21], 4, 4, 72),
            //
            new(d[22], 5, 4, 72), new(d[23], 5, 4, 72), new(d[24], 5, 4, 72),
            new(d[25], 5, 3, 60), new(d[26], 5, 3, 60), new(d[27], 5, 4, 72),
            //
            new(d[28], 6, 4, 72), new(d[29], 6, 4, 72), new(d[30], 6, 4, 72),
            new(d[31], 6, 3, 60), new(d[32], 6, 4, 72), new(d[33], 6, 4, 72),
            //
            new(d[34], 7, 4, 72), new(d[35], 7, 4, 72), new(d[36], 7, 4, 72),
            new(d[37], 7, 3, 60), new(d[38], 7, 4, 72), new(d[39], 7, 4, 72),
            //
            new(d[40], 8, 4, 72), new(d[41], 8, 3, 72), new(d[42], 8, 2, 36),
            new(d[43], 8, 4, 72), new(d[44], 8, 2, 36), new(d[45], 8, 4, 72),
            new(d[46], 8, 4, 72),
            //
            new(d[47], 9, 2, 72), new(d[48], 9, 2, 36), new(d[49], 9, 2, 36),
            new(d[50], 9, 2, 36), new(d[51], 9, 2, 72), new(d[52], 9, 4, 72),
            new(d[53], 9, 2, 36),
            //
            new(d[54], 10, 2, 72), new(d[55], 10, 3, 60), new(d[56], 10, 2, 40),
            new(d[57], 10, 2, 36), new(d[58], 10, 2, 36), new(d[59], 10, 2, 72),
            new(d[60], 10, 4, 36),
        ];

        var result = await client.CreateCourseCurriculum(data.DireitoCourseId, "Grade Direito 1.0", links);
        data.DireitoCourseCurriculumId = result.Success.Id;
    }

    private static async Task DevCreateAdsTeachers(TestsHttpClient client, DevInstitutionData data)
    {
        // Professores das disciplinas do período 1 de ADS
        (string Name, string Email, int DisciplineIndex)[] teachers =
        [
            ("Alan Turing", "alan.turing@estud.com", 0),
            ("Ada Lovelace", "ada.lovelace@estud.com", 1),
            ("Grace Hopper", "grace.hopper@estud.com", 2),
            ("Vinton Cerf", "vinton.cerf@estud.com", 3),
            ("Tim Berners-Lee", "tim.berners.lee@estud.com", 4),
            ("Barbara Liskov", "barbara.liskov@estud.com", 5),
        ];

        foreach (var teacher in teachers)
        {
            var result = await client.CreateTeacher(teacher.Name, teacher.Email);
            var teacherId = result.Success.Id;

            await client.AssignCampiToTeacher(teacherId, data.CampiIds);
            await client.AssignDisciplinesToTeacher(teacherId, [data.AdsDisciplinesIds[teacher.DisciplineIndex]]);
        }
    }

    private static async Task DevCreateDireitoTeachers(TestsHttpClient client, DevInstitutionData data)
    {
        // Professores das disciplinas do período 1 de Direito
        (string Name, string Email, int DisciplineIndex)[] teachers =
        [
            ("Hans Kelsen", "hans.kelsen@estud.com", 0),
            ("Rui Barbosa", "rui.barbosa@estud.com", 1),
            ("Nelson Mandela", "nelson.mandela@estud.com", 2),
            ("Ruth Ginsburg", "ruth.ginsburg@estud.com", 3),
            ("Miguel Reale", "miguel.reale@estud.com", 4),
        ];

        foreach (var teacher in teachers)
        {
            var result = await client.CreateTeacher(teacher.Name, teacher.Email);
            var teacherId = result.Success.Id;

            await client.AssignCampiToTeacher(teacherId, data.CampiIds);
            await client.AssignDisciplinesToTeacher(teacherId, [data.DireitoDisciplinesIds[teacher.DisciplineIndex]]);
        }
    }

    private static async Task DevCreateAcademicPeriods(TestsHttpClient client, DevInstitutionData data)
    {
        var year = DateTime.Now.Year;

        var firstStartAt = new DateOnly(year, 01, 02);
        var secondStartAt = new DateOnly(year, 06, 03);

        await client.CreateAcademicPeriod($"{year}.1", firstStartAt, new DateOnly(year, 06, 01));
        var second = await client.CreateAcademicPeriod($"{year}.2", secondStartAt, new DateOnly(year, 12, 20));
        data.AcademicPeriodId = second.Success.Id;

        // Matrículas abrem 2 semanas antes de cada período acadêmico e ficam abertas por 2 semanas
        await client.CreateEnrollmentPeriod($"Matrículas {year}.1", firstStartAt.AddDays(-14), firstStartAt);
        await client.CreateEnrollmentPeriod($"Matrículas {year}.2", secondStartAt.AddDays(-14), secondStartAt);
    }

    private static async Task DevCreateCourseOfferings(TestsHttpClient client, DevInstitutionData data)
    {
        var ads = await client.CreateCourseOffering(
            data.CampiIds[0],
            data.AdsCourseId,
            data.AdsCourseCurriculumId,
            data.AcademicPeriodId,
            CourseSession.Evening
        );
        data.AdsCourseOfferingId = ads.Success.Id;

        var direito = await client.CreateCourseOffering(
            data.CampiIds[0],
            data.DireitoCourseId,
            data.DireitoCourseCurriculumId,
            data.AcademicPeriodId,
            CourseSession.Morning
        );
        data.DireitoCourseOfferingId = direito.Success.Id;
    }

    private static async Task DevCreateStudents(TestsHttpClient client, DevInstitutionData data)
    {
        (string Name, string Email)[] adsStudents =
        [
            ("Linus Torvalds", "linus.torvalds@estud.com"),
            ("Margaret Hamilton", "margaret.hamilton@estud.com"),
            ("Dennis Ritchie", "dennis.ritchie@estud.com"),
        ];

        foreach (var student in adsStudents)
        {
            var result = await client.CreateStudent(student.Name, student.Email);
            await client.EnrollStudentInCourseOffering(result.Success.Id, data.AdsCourseOfferingId);
            data.AdsStudentsIds.Add(result.Success.Id);
        }

        (string Name, string Email)[] direitoStudents =
        [
            ("Clarice Lispector", "clarice.lispector@estud.com"),
            ("Machado de Assis", "machado.assis@estud.com"),
            ("Cecília Meireles", "cecilia.meireles@estud.com"),
        ];

        foreach (var student in direitoStudents)
        {
            var result = await client.CreateStudent(student.Name, student.Email);
            await client.EnrollStudentInCourseOffering(result.Success.Id, data.DireitoCourseOfferingId);
        }
    }

    private static async Task DevCreateParents(TestsHttpClient client, DevInstitutionData data)
    {
        List<CreateParentStudentIn> students = [
            new() { StudentId = data.AdsStudentsIds[0], Relationship = ParentRelationship.Mother },
            new() { StudentId = data.AdsStudentsIds[1], Relationship = ParentRelationship.Mother },
        ];

        await client.CreateParent("Anna Torvalds", "parent@gmail.com", students, "81988887777");
    }
}

internal class DevInstitutionData
{
    public List<int> CampiIds { get; set; } = [];
    public int AdsCourseId { get; set; }
    public int DireitoCourseId { get; set; }
    public List<int> AdsDisciplinesIds { get; set; } = [];
    public List<int> DireitoDisciplinesIds { get; set; } = [];
    public int AdsCourseCurriculumId { get; set; }
    public int DireitoCourseCurriculumId { get; set; }
    public int AcademicPeriodId { get; set; }
    public int AdsCourseOfferingId { get; set; }
    public int DireitoCourseOfferingId { get; set; }

    public List<int> AdsStudentsIds { get; set; } = [];
}
