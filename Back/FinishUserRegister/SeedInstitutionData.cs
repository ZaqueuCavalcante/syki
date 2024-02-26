using Syki.Back.Domain;
using Syki.Back.CreateCampus;

namespace Syki.Back.FinishUserRegister;

public class SeedInstitutionData
{
    public Guid InstitutionId { get; set; }
}

public class SeedInstitutionDataHandler(SykiDbContext ctx, IProfessoresService professoresService) : ISykiTaskHandler<SeedInstitutionData>
{
    public async Task Handle(SeedInstitutionData task)
    {
        var id = task.InstitutionId;
        var faculdade = await ctx.Institutions.FirstAsync(f => f.Id == id);

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
            faculdade.Periodos[0].Id,
            Turno.Noturno
        );
        ctx.Add(ofertaAds);

        await professoresService.Create(faculdade.Id, ProfessorIn.Demo("Davi Pessoa Ferraz"));
        await professoresService.Create(faculdade.Id, ProfessorIn.Demo("Luciete Bezerra Alves"));
        await professoresService.Create(faculdade.Id, ProfessorIn.Demo("Antonio Marques da Costa Júnior"));
        await professoresService.Create(faculdade.Id, ProfessorIn.Demo("Paulo Marcelo Pedrosa de Almeida"));
        await professoresService.Create(faculdade.Id, ProfessorIn.Demo("Josélia Pachêco de Santana"));
        await professoresService.Create(faculdade.Id, ProfessorIn.Demo("Manuela Abath Valença"));

        await ctx.SaveChangesAsync();
    }
}
