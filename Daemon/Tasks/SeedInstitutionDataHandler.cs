using Syki.Back.CreateOferta;
using Syki.Back.CreateProfessor;
using Syki.Back.Features.Academic.CreateCurso;
using Syki.Back.Features.Academic.CreateGrade;
using Syki.Back.Features.Academic.CreateCampus;
using Syki.Back.Features.Cross.FinishUserRegister;
using Syki.Back.Features.Academic.CreateDisciplina;
using Syki.Back.Features.Academic.CreateAcademicPeriod;

namespace Syki.Daemon.Tasks;

public class SeedInstitutionDataHandler(SykiDbContext ctx, CreateProfessorService service) : ISykiTaskHandler<SeedInstitutionData>
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
            new Curso(id, "Administração", CourseType.Mestrado),
            new Curso(id, "Análise e Desenvolvimento de Sistemas", CourseType.Bacharelado),
            new Curso(id, "Arquitetura e Urbanismo", CourseType.Tecnologo),
            new Curso(id, "Ciência da Computação", CourseType.Bacharelado),
            new Curso(id, "Direito", CourseType.Licenciatura),
            new Curso(id, "Engenharia Civil", CourseType.Bacharelado),
            new Curso(id, "Engenharia Mecânica", CourseType.Bacharelado),
            new Curso(id, "Engenharia de Produção", CourseType.PosDoutorado),
            new Curso(id, "Pedagogia", CourseType.Licenciatura),
        ];

        institution.Disciplines =
        [
            new Disciplina(id, "Matemática Discreta"),
            new Disciplina(id, "Introdução ao Desenvolvimento Web"),
            new Disciplina(id, "Design de Interação Humano-Máquina"),
            new Disciplina(id, "Introdução à Redes de Computadores"),
            new Disciplina(id, "Pensamento Computacional e Algoritmos"),
            new Disciplina(id, "Projeto Integrador I: Concepção e Prototipação"),
            //
            new Disciplina(id, "Banco de Dados"),
            new Disciplina(id, "Estrutura de Dados"),
            new Disciplina(id, "Informática e Sociedade"),
            new Disciplina(id, "Programação Orientada a Objetos"),
            new Disciplina(id, "Projeto Integrador II: Modelagem de Banco de Dados"),
            new Disciplina(id, "Arquitetura de Computadores e Sistemas Operacionais"),
            //
            new Disciplina(id, "Estatística Aplicada"),
            new Disciplina(id, "Arquitetura de Software"),
            new Disciplina(id, "Análise e Projeto de Software"),
            new Disciplina(id, "Computação em Nuvem e Web Services"),
            new Disciplina(id, "Configuração e Manutenção de Software"),
            new Disciplina(id, "Projeto Integrador III: Desenvolvimento Full Stack"),
            //
            new Disciplina(id, "Sistemas Distribuídos"),
            new Disciplina(id, "Inovação e Empreendedorismo"),
            new Disciplina(id, "Análise e Visualização de Dados"),
            new Disciplina(id, "Desenvolvimentos de Aplicações Móveis"),
            new Disciplina(id, "Gestão de Projetos e Governança de TI"),
            new Disciplina(id, "Projeto Integrador IV: Aplicações Móveis"),
            //
            new Disciplina(id, "Libras"),
            new Disciplina(id, "Sistemas Embarcados"),
            new Disciplina(id, "Big Data e Data Science"),
            new Disciplina(id, "Inteligência Artificial"),
            new Disciplina(id, "Segurança da Informação"),
            new Disciplina(id, "Testes e Verificação de Software"),
            new Disciplina(id, "Projeto Integrador V: Sistemas Inteligentes"),
            //
            //
            new Disciplina(id, "Direito e Economia"),
            new Disciplina(id, "Introdução ao Direito"),
            new Disciplina(id, "História das Instituições Jurídicas"),
            new Disciplina(id, "Teoria do Estado, Política e Direito"),
            new Disciplina(id, "Sociologia Jurídica"),
            new Disciplina(id, "Antropologia Jurídica"),
            //
            new Disciplina(id, "Direito Civil I (parte geral)"),
            new Disciplina(id, "Direito Constitucional"),
            new Disciplina(id, "Direito Financeiro"),
            new Disciplina(id, "Direito Penal I (parte geral)"),
            new Disciplina(id, "Filosofia Geral e Jurídica"),
            //
            new Disciplina(id, "Direito Civil II (obrigações e contratos)"),
            new Disciplina(id, "Direito Administrativo"),
            new Disciplina(id, "Direito Penal II (teoria da pena)"),
            new Disciplina(id, "Direito Internacional Público"),
            new Disciplina(id, "Teoria Geral do Processo"),
            new Disciplina(id, "Hermenêutica Jurídica"),
            //
            new Disciplina(id, "Direito Civil III (contratos em espécie)"),
            new Disciplina(id, "Direito Civil IV (direitos reais)"),
            new Disciplina(id, "Direito Processual Constitucional"),
            new Disciplina(id, "Direito Processual III (crimes em espécie)"),
            new Disciplina(id, "Direito Processual Civil I"),
            new Disciplina(id, "Metodologia da Pesquisa"),
            new Disciplina(id, "Estágio I - Laboratório de Prática Jurídica I"),
            //
            new Disciplina(id, "Direito Civil V"),
            new Disciplina(id, "Direito Empresarial I"),
            new Disciplina(id, "Direito do Trabalho I"),
            new Disciplina(id, "Direito Processual Penal I"),
            new Disciplina(id, "Direito Processual Civil II"),
            new Disciplina(id, "Estágio II - Laboratório de Prática Jurídica II"),
            new Disciplina(id, "Estágio II - Serviço de Assistência Judiciária I"),
            //
            new Disciplina(id, "Direito Empresarial II"),
            new Disciplina(id, "Direito Tributário"),
            new Disciplina(id, "Direito Internacional Privado"),
            new Disciplina(id, "Direito Processual Penal II"),
            new Disciplina(id, "Direito do Trabalho II"),
            new Disciplina(id, "Ética (geral e jurídica)"),
            new Disciplina(id, "Estágio III - Serviço de Assistência Judiciária II"),
            new Disciplina(id, "Monografia Final"),
        ];

        institution.Courses[1].Disciplinas = institution.Disciplines.Take(31).ToList(); // ADS
        institution.Courses[4].Disciplinas = institution.Disciplines.Skip(31).ToList(); // Direito

        var gradeAds = new Grade(id, institution.Courses[1].Id, "Grade ADS 1.0");
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[00].Id, 1, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[01].Id, 1, 4, 40));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[02].Id, 1, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[03].Id, 1, 5, 55));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[04].Id, 1, 4, 45));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[05].Id, 1, 3, 25));
        //
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[06].Id, 2, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[07].Id, 2, 4, 40));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[08].Id, 2, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[09].Id, 2, 5, 55));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[10].Id, 2, 5, 45));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[11].Id, 2, 2, 25));
        //
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[12].Id, 3, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[13].Id, 3, 4, 40));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[14].Id, 3, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[15].Id, 3, 5, 55));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[16].Id, 3, 5, 45));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[17].Id, 3, 2, 25));
        //
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[18].Id, 4, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[19].Id, 4, 4, 40));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[20].Id, 4, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[21].Id, 4, 5, 55));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[22].Id, 4, 5, 45));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[23].Id, 4, 2, 25));
        //
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[24].Id, 5, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[25].Id, 5, 4, 40));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[26].Id, 5, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[27].Id, 5, 5, 55));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[28].Id, 5, 5, 45));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[29].Id, 5, 6, 50));
        gradeAds.Vinculos.Add(new GradeDisciplina(institution.Disciplines[30].Id, 5, 2, 25));
        ctx.Add(gradeAds);

        var ofertaAds = new Oferta(
            id,
            institution.Campi[2].Id,
            institution.Courses[1].Id,
            gradeAds.Id,
            institution.AcademicPeriods[0].Id,
            Turno.Noturno
        );
        ctx.Add(ofertaAds);

        await service.Create(institution.Id, ProfessorIn.Demo("Davi Pessoa Ferraz"));
        await service.Create(institution.Id, ProfessorIn.Demo("Luciete Bezerra Alves"));
        await service.Create(institution.Id, ProfessorIn.Demo("Antonio Marques da Costa Júnior"));
        await service.Create(institution.Id, ProfessorIn.Demo("Paulo Marcelo Pedrosa de Almeida"));
        await service.Create(institution.Id, ProfessorIn.Demo("Josélia Pachêco de Santana"));
        await service.Create(institution.Id, ProfessorIn.Demo("Manuela Abath Valença"));

        await ctx.SaveChangesAsync();
    }
}
