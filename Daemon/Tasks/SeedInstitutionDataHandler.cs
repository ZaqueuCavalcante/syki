using Syki.Shared;
using Syki.Back.Tasks;
using Syki.Back.Domain;
using Syki.Back.Database;
using Syki.Back.CreateOferta;
using Syki.Back.CreateProfessor;
using Microsoft.EntityFrameworkCore;
using Syki.Back.CreateAcademicPeriod;
using Syki.Back.Features.Academico.CreateCurso;
using Syki.Back.Features.Academico.CreateCampus;
using Syki.Back.Features.Cross.FinishUserRegister;
using Syki.Back.Features.Academico.CreateDisciplina;
using Syki.Back.Features.Academico.CreateGrade;

namespace Syki.Daemon.Tasks;

public class SeedInstitutionDataHandler(SykiDbContext ctx, CreateProfessorService service) : ISykiTaskHandler<SeedInstitutionData>
{
    public async Task Handle(SeedInstitutionData task)
    {
        var id = task.InstitutionId;
        var faculdade = await ctx.Institutions.FirstAsync(f => f.Id == id);

        var year = DateTime.Now.Year;
        faculdade.AcademicPeriods =
        [
            new AcademicPeriod($"{year}.1", id, new DateOnly(year, 02, 01), new DateOnly(year, 06, 01)),
            new AcademicPeriod($"{year}.2", id, new DateOnly(year, 07, 01), new DateOnly(year, 12, 01)),
        ];

        faculdade.Campi =
        [
            new Campus(id, "Garoa", "Garanhuns - PE"),
            new Campus(id, "Sertão", "Petrolina - PE"),
            new Campus(id, "Agreste", "Caruaru - PE"),
            new Campus(id, "Suassuna", "Recife - PE"),
        ];

        faculdade.Cursos =
        [
            new Curso(id, "Administração", TipoDeCurso.Mestrado),
            new Curso(id, "Análise e Desenvolvimento de Sistemas", TipoDeCurso.Bacharelado),
            new Curso(id, "Arquitetura e Urbanismo", TipoDeCurso.Tecnologo),
            new Curso(id, "Ciência da Computação", TipoDeCurso.Bacharelado),
            new Curso(id, "Direito", TipoDeCurso.Licenciatura),
            new Curso(id, "Engenharia Civil", TipoDeCurso.Bacharelado),
            new Curso(id, "Engenharia Mecânica", TipoDeCurso.Bacharelado),
            new Curso(id, "Engenharia de Produção", TipoDeCurso.PosDoutorado),
            new Curso(id, "Pedagogia", TipoDeCurso.Licenciatura),
        ];

        faculdade.Disciplinas =
        [
            new Disciplina(id, "Matemática Discreta", "MD"),
            new Disciplina(id, "Introdução ao Desenvolvimento Web", "IDW"),
            new Disciplina(id, "Design de Interação Humano-Máquina", "DIHM"),
            new Disciplina(id, "Introdução à Redes de Computadores", "IRC"),
            new Disciplina(id, "Pensamento Computacional e Algoritmos", "PCA"),
            new Disciplina(id, "Projeto Integrador I: Concepção e Prototipação", "PI-CP"),
            //
            new Disciplina(id, "Banco de Dados", "BD"),
            new Disciplina(id, "Estrutura de Dados", "ED"),
            new Disciplina(id, "Informática e Sociedade", "IS"),
            new Disciplina(id, "Programação Orientada a Objetos", "POO"),
            new Disciplina(id, "Projeto Integrador II: Modelagem de Banco de Dados", "PI-MBD"),
            new Disciplina(id, "Arquitetura de Computadores e Sistemas Operacionais", "ACSO"),
            //
            new Disciplina(id, "Estatística Aplicada", "EA"),
            new Disciplina(id, "Arquitetura de Software", "AS"),
            new Disciplina(id, "Análise e Projeto de Software", "APS"),
            new Disciplina(id, "Computação em Nuvem e Web Services", "CNWS"),
            new Disciplina(id, "Configuração e Manutenção de Software", "CMS"),
            new Disciplina(id, "Projeto Integrador III: Desenvolvimento Full Stack", "PI-DFS"),
            //
            new Disciplina(id, "Sistemas Distribuídos", "SD"),
            new Disciplina(id, "Inovação e Empreendedorismo", "IE"),
            new Disciplina(id, "Análise e Visualização de Dados", "AVD"),
            new Disciplina(id, "Desenvolvimentos de Aplicações Móveis", "DAM"),
            new Disciplina(id, "Gestão de Projetos e Governança de TI", "GPGT"),
            new Disciplina(id, "Projeto Integrador IV: Aplicações Móveis", "PI-AM"),
            //
            new Disciplina(id, "Libras", "LIB"),
            new Disciplina(id, "Sistemas Embarcados", "SE"),
            new Disciplina(id, "Big Data e Data Science", "BDDS"),
            new Disciplina(id, "Inteligência Artificial", "IA"),
            new Disciplina(id, "Segurança da Informação", "SI"),
            new Disciplina(id, "Testes e Verificação de Software", "TVS"),
            new Disciplina(id, "Projeto Integrador V: Sistemas Inteligentes", "PI-SI"),
            //
            //
            new Disciplina(id, "Direito e Economia", "DE"),
            new Disciplina(id, "Introdução ao Direito", "ID"),
            new Disciplina(id, "História das Instituições Jurídicas", "HIJ"),
            new Disciplina(id, "Teoria do Estado, Política e Direito", "TEPD"),
            new Disciplina(id, "Sociologia Jurídica", "SJ"),
            new Disciplina(id, "Antropologia Jurídica", "AJ"),
            //
            new Disciplina(id, "Direito Civil I (parte geral)", "DCI"),
            new Disciplina(id, "Direito Constitucional", "DC"),
            new Disciplina(id, "Direito Financeiro", "DF"),
            new Disciplina(id, "Direito Penal I (parte geral)", "DPI"),
            new Disciplina(id, "Filosofia Geral e Jurídica", "FGJ"),
            //
            new Disciplina(id, "Direito Civil II (obrigações e contratos)", "DCII"),
            new Disciplina(id, "Direito Administrativo", "DA"),
            new Disciplina(id, "Direito Penal II (teoria da pena)", "DPII"),
            new Disciplina(id, "Direito Internacional Público", "DIP"),
            new Disciplina(id, "Teoria Geral do Processo", "TGP"),
            new Disciplina(id, "Hermenêutica Jurídica", "HJ"),
            //
            new Disciplina(id, "Direito Civil III (contratos em espécie)", "DCIII"),
            new Disciplina(id, "Direito Civil IV (direitos reais)", "DCIV"),
            new Disciplina(id, "Direito Processual Constitucional", "DPC"),
            new Disciplina(id, "Direito Processual III (crimes em espécie)", "DPIII"),
            new Disciplina(id, "Direito Processual Civil I", "DPCI"),
            new Disciplina(id, "Metodologia da Pesquisa", "MP"),
            new Disciplina(id, "Estágio I - Laboratório de Prática Jurídica I", "EI-LPJ"),
            //
            new Disciplina(id, "Direito Civil V", "DCV"),
            new Disciplina(id, "Direito Empresarial I", "DEI"),
            new Disciplina(id, "Direito do Trabalho I", "DTI"),
            new Disciplina(id, "Direito Processual Penal I", "DPPI"),
            new Disciplina(id, "Direito Processual Civil II", "DPCII"),
            new Disciplina(id, "Estágio II - Laboratório de Prática Jurídica II", "EII-LPJ"),
            new Disciplina(id, "Estágio II - Serviço de Assistência Judiciária I", "EII-SAJ"),
            //
            new Disciplina(id, "Direito Empresarial II", "DEII"),
            new Disciplina(id, "Direito Tributário", "DT"),
            new Disciplina(id, "Direito Internacional Privado", "DIP"),
            new Disciplina(id, "Direito Processual Penal II", "DPPII"),
            new Disciplina(id, "Direito do Trabalho II", "DTII"),
            new Disciplina(id, "Ética (geral e jurídica)", "EGJ"),
            new Disciplina(id, "Estágio III - Serviço de Assistência Judiciária II", "EIII-SAJ"),
            new Disciplina(id, "Monografia Final", "MF"),
        ];

        faculdade.Cursos[1].Disciplinas = faculdade.Disciplinas.Take(31).ToList(); // ADS
        faculdade.Cursos[4].Disciplinas = faculdade.Disciplinas.Skip(31).ToList(); // Direito

        var gradeAds = new Grade(id, faculdade.Cursos[1].Id, "Grade ADS 1.0");
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[00].Id, 1, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[01].Id, 1, 4, 40));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[02].Id, 1, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[03].Id, 1, 5, 55));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[04].Id, 1, 4, 45));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[05].Id, 1, 3, 25));
        //
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[06].Id, 2, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[07].Id, 2, 4, 40));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[08].Id, 2, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[09].Id, 2, 5, 55));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[10].Id, 2, 5, 45));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[11].Id, 2, 2, 25));
        //
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[12].Id, 3, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[13].Id, 3, 4, 40));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[14].Id, 3, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[15].Id, 3, 5, 55));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[16].Id, 3, 5, 45));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[17].Id, 3, 2, 25));
        //
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[18].Id, 4, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[19].Id, 4, 4, 40));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[20].Id, 4, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[21].Id, 4, 5, 55));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[22].Id, 4, 5, 45));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[23].Id, 4, 2, 25));
        //
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[24].Id, 5, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[25].Id, 5, 4, 40));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[26].Id, 5, 5, 60));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[27].Id, 5, 5, 55));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[28].Id, 5, 5, 45));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[29].Id, 5, 6, 50));
        gradeAds.Vinculos.Add(new GradeDisciplina(faculdade.Disciplinas[30].Id, 5, 2, 25));
        ctx.Add(gradeAds);

        var ofertaAds = new Oferta(
            id,
            faculdade.Campi[2].Id,
            faculdade.Cursos[1].Id,
            gradeAds.Id,
            faculdade.AcademicPeriods[0].Id,
            Turno.Noturno
        );
        ctx.Add(ofertaAds);

        await service.Create(faculdade.Id, ProfessorIn.Demo("Davi Pessoa Ferraz"));
        await service.Create(faculdade.Id, ProfessorIn.Demo("Luciete Bezerra Alves"));
        await service.Create(faculdade.Id, ProfessorIn.Demo("Antonio Marques da Costa Júnior"));
        await service.Create(faculdade.Id, ProfessorIn.Demo("Paulo Marcelo Pedrosa de Almeida"));
        await service.Create(faculdade.Id, ProfessorIn.Demo("Josélia Pachêco de Santana"));
        await service.Create(faculdade.Id, ProfessorIn.Demo("Manuela Abath Valença"));

        await ctx.SaveChangesAsync();
    }
}
