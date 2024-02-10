using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;
using Syki.Back.Services;

namespace Syki.Back.Tasks;

public class SeedFaculdadeTestData
{
    public Guid FaculdadeId { get; set; }
}

public class SeedFaculdadeTestDataHandler : ISykiTaskHandler<SeedFaculdadeTestData>
{
    private readonly SykiDbContext _ctx;
    private readonly IProfessoresService _professoresService;
    public SeedFaculdadeTestDataHandler(SykiDbContext ctx, IProfessoresService professoresService)
    {
        _ctx = ctx;
        _professoresService = professoresService;
    }

    public async Task Handle(SeedFaculdadeTestData task)
    {
        var id = task.FaculdadeId;
        var faculdade = await _ctx.Faculdades.FirstAsync(f => f.Id == id);

        var year = DateTime.Now.Year;
        faculdade.Periodos =
        [
            new Periodo($"{year}.1", id, new DateOnly(year, 02, 01), new DateOnly(year, 06, 01)),
            new Periodo($"{year}.2", id, new DateOnly(year, 07, 01), new DateOnly(year, 12, 01)),
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
            new Disciplina(id, "Matemática Discreta", 55),
            new Disciplina(id, "Introdução ao Desenvolvimento Web", 40),
            new Disciplina(id, "Design de Interação Humano-Máquina", 60),
            new Disciplina(id, "Introdução à Redes de Computadores", 50),
            new Disciplina(id, "Pensamento Computacional e Algoritmos", 45),
            new Disciplina(id, "Projeto Integrador I: Concepção e Prototipação", 25),
            //
            new Disciplina(id, "Banco de Dados", 80),
            new Disciplina(id, "Estrutura de Dados", 50),
            new Disciplina(id, "Informática e Sociedade", 30),
            new Disciplina(id, "Programação Orientada a Objetos", 40),
            new Disciplina(id, "Projeto Integrador II: Modelagem de Banco de Dados", 20),
            new Disciplina(id, "Arquitetura de Computadores e Sistemas Operacionais", 60),
            //
            new Disciplina(id, "Estatística Aplicada", 55),
            new Disciplina(id, "Arquitetura de Software", 65),
            new Disciplina(id, "Análise e Projeto de Software", 45),
            new Disciplina(id, "Computação em Nuvem e Web Services", 50),
            new Disciplina(id, "Configuração e Manutenção de Software", 60),
            new Disciplina(id, "Projeto Integrador III: Desenvolvimento Full Stack", 30),
            //
            new Disciplina(id, "Sistemas Distribuídos", 60),
            new Disciplina(id, "Inovação e Empreendedorismo", 60),
            new Disciplina(id, "Análise e Visualização de Dados", 60),
            new Disciplina(id, "Desenvolvimentos de Aplicações Móveis", 60),
            new Disciplina(id, "Gestão de Projetos e Governança de TI", 60),
            new Disciplina(id, "Projeto Integrador IV: Aplicações Móveis", 35),
            //
            new Disciplina(id, "Libras", 60),
            new Disciplina(id, "Sistemas Embarcados", 60),
            new Disciplina(id, "Big Data e Data Science", 60),
            new Disciplina(id, "Inteligência Artificial", 60),
            new Disciplina(id, "Segurança da Informação", 60),
            new Disciplina(id, "Testes e Verificação de Software", 60),
            new Disciplina(id, "Projeto Integrador V: Sistemas Inteligentes", 30),
            //
            //
            new Disciplina(id, "Direito e Economia", 60),
            new Disciplina(id, "Introdução ao Direito", 60),
            new Disciplina(id, "História das Instituições Jurídicas", 60),
            new Disciplina(id, "Teoria do Estado, Política e Direito", 60),
            new Disciplina(id, "Sociologia Jurídica", 60),
            new Disciplina(id, "Antropologia Jurídica", 60),
            //
            new Disciplina(id, "Direito Civil I (parte geral)", 60),
            new Disciplina(id, "Direito Constitucional", 60),
            new Disciplina(id, "Direito Financeiro", 60),
            new Disciplina(id, "Direito Penal I (parte geral)", 60),
            new Disciplina(id, "Filosofia Geral e Jurídica", 60),
            //
            new Disciplina(id, "Direito Civil II (obrigações e contratos)", 60),
            new Disciplina(id, "Direito Administrativo", 60),
            new Disciplina(id, "Direito Penal II (teoria da pena)", 60),
            new Disciplina(id, "Direito Internacional Público", 60),
            new Disciplina(id, "Teoria Geral do Processo", 60),
            new Disciplina(id, "Hermenêutica Jurídica", 60),
            //
            new Disciplina(id, "Direito Civil III (contratos em espécie)", 60),
            new Disciplina(id, "Direito Civil IV (direitos reais)", 60),
            new Disciplina(id, "Direito Processual Constitucional", 60),
            new Disciplina(id, "Direito Processual III (crimes em espécie)", 60),
            new Disciplina(id, "Direito Processual Civil I", 60),
            new Disciplina(id, "Metodologia da Pesquisa", 60),
            new Disciplina(id, "Estágio I - Laboratório de Prática Jurídica I", 60),
            //
            new Disciplina(id, "Direito Civil V", 60),
            new Disciplina(id, "Direito Empresarial I", 60),
            new Disciplina(id, "Direito do Trabalho I", 60),
            new Disciplina(id, "Direito Processual Penal I", 60),
            new Disciplina(id, "Direito Processual Civil II", 60),
            new Disciplina(id, "Estágio II - Laboratório de Prática Jurídica II", 60),
            new Disciplina(id, "Estágio II - Serviço de Assistência Judiciária I", 60),
            //
            new Disciplina(id, "Direito Empresarial II", 60),
            new Disciplina(id, "Direito Tributário", 60),
            new Disciplina(id, "Direito Internacional Privado", 60),
            new Disciplina(id, "Direito Processual Penal II", 60),
            new Disciplina(id, "Direito do Trabalho II", 60),
            new Disciplina(id, "Ética (geral e jurídica)", 60),
            new Disciplina(id, "Estágio III - Serviço de Assistência Judiciária II", 60),
            new Disciplina(id, "Monografia Final", 60),
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
        _ctx.Add(gradeAds);

        var ofertaAds = new Oferta(
            id,
            faculdade.Campi[2].Id,
            faculdade.Cursos[1].Id,
            gradeAds.Id,
            faculdade.Periodos[0].Id,
            Turno.Noturno
        );
        _ctx.Add(ofertaAds);

        await _professoresService.Create(faculdade.Id, new ProfessorIn { Nome = "Davi Pessoa Ferraz", Email = $"{Guid.NewGuid().ToString().OnlyNumbers()}@syki.com" });
        await _professoresService.Create(faculdade.Id, new ProfessorIn { Nome = "Luciete Bezerra Alves", Email = $"{Guid.NewGuid().ToString().OnlyNumbers()}@syki.com" });
        await _professoresService.Create(faculdade.Id, new ProfessorIn { Nome = "Antonio Marques da Costa Júnior", Email = $"{Guid.NewGuid().ToString().OnlyNumbers()}@syki.com" });
        await _professoresService.Create(faculdade.Id, new ProfessorIn { Nome = "Paulo Marcelo Pedrosa de Almeida", Email = $"{Guid.NewGuid().ToString().OnlyNumbers()}@syki.com" });
        await _professoresService.Create(faculdade.Id, new ProfessorIn { Nome = "Josélia Pachêco de Santana", Email = $"{Guid.NewGuid().ToString().OnlyNumbers()}@syki.com" });
        await _professoresService.Create(faculdade.Id, new ProfessorIn { Nome = "Manuela Abath Valença", Email = $"{Guid.NewGuid().ToString().OnlyNumbers()}@syki.com" });

        await _ctx.SaveChangesAsync();
    }
}
