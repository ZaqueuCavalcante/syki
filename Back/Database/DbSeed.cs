using Syki.Domain;

namespace Syki.Database;

public static class DbSeed
{
    public static Faculdade NovaRoma = new()
    {
        Id = Guid.Parse("8d08e437-8b18-4a15-a231-4a2260e60432"),
        Nome = "Nova Roma",
        Campi = new()
        {
            new Campus("Caruaru"),
            new Campus("Recife"),
            new Campus("Garanhuns"),
        },
        Cursos = new()
        {
            new Curso("Administração", Dtos.TipoDeCurso.PosGraduacao),
            new Curso("Análise e Desenvolvimento de Sistemas", Dtos.TipoDeCurso.Graduacao),
            new Curso("Arquitetura e Urbanismo", Dtos.TipoDeCurso.Graduacao),
            new Curso("Ciência da Computação", Dtos.TipoDeCurso.Graduacao),
            new Curso("Direito", Dtos.TipoDeCurso.PosGraduacao),
            new Curso("Engenharia Civil", Dtos.TipoDeCurso.Graduacao),
            new Curso("Engenharia Mecânica", Dtos.TipoDeCurso.Graduacao),
            new Curso("Engenharia de Produção", Dtos.TipoDeCurso.Graduacao),
            new Curso("Pedagogia", Dtos.TipoDeCurso.PosGraduacao),
        },
        Disciplinas = new()
        {
            new Disciplina("Matemática Discreta", 55),  // 001
            new Disciplina("Introdução ao Desenvolvimento Web", 40),
            new Disciplina("Design de Interação Humano-Máquina", 60),
            new Disciplina("Introdução à Redes de Computadores", 60),
            new Disciplina("Pensamento Computacional e Algoritmos", 60),
            new Disciplina("Projeto Integrador I: Concepção e Prototipação", 25),
            //
            new Disciplina("Banco de Dados", 60),  // 007
            new Disciplina("Estrutura de Dados", 60),
            new Disciplina("Informática e Sociedade", 60),
            new Disciplina("Programação Orientada a Objetos", 60),
            new Disciplina("Projeto Integrador II: Modelagem de Banco de Dados", 30),
            new Disciplina("Arquitetura de Computadores e Sistemas Operacionais", 60),
            //
            new Disciplina("Estatística Aplicada", 60),  // 013
            new Disciplina("Arquitetura de Software", 60),
            new Disciplina("Análise e Projeto de Software", 60),
            new Disciplina("Computação em Nuvem e Web Services", 60),
            new Disciplina("Configuração e Manutenção de Software", 60),
            new Disciplina("Projeto Integrador III: Desenvolvimento Full Stack", 30),
            //
            new Disciplina("Sistemas Distribuídos", 60),  // 019
            new Disciplina("Inovação e Empreendedorismo", 60),
            new Disciplina("Análise e Visualização de Dados", 60),
            new Disciplina("Desenvolvimentos de Aplicações Móveis", 60),
            new Disciplina("Gestão de Projetos e Governança de TI", 60),
            new Disciplina("Projeto Integrador IV: Aplicações Móveis", 35),
            //
            new Disciplina("Libras", 60),  // 25
            new Disciplina("Sistemas Embarcados", 60),
            new Disciplina("Big Data e Data Science", 60),
            new Disciplina("Inteligência Artificial", 60),
            new Disciplina("Segurança da Informação", 60),
            new Disciplina("Testes e Verificação de Software", 60),
            new Disciplina("Projeto Integrador V: Sistemas Inteligentes", 30),
            //
            //
            new Disciplina("Direito e Economia", 60),  // 032
            new Disciplina("Introdução ao Direito", 60),
            new Disciplina("História das Instituições Jurídicas", 60),
            new Disciplina("Teoria do Estado, Política e Direito", 60),
            new Disciplina("Sociologia Jurídica", 60),
            new Disciplina("Antropologia Jurídica", 60),
            //
            new Disciplina("Direito Civil I (parte geral)", 60),
            new Disciplina("Direito Constitucional", 60),
            new Disciplina("Direito Financeiro", 60),
            new Disciplina("Direito Penal I (parte geral)", 60),
            new Disciplina("Filosofia Geral e Jurídica", 60),
            //
            new Disciplina("Direito Civil II (obrigações e contratos)", 60),
            new Disciplina("Direito Administrativo", 60),
            new Disciplina("Direito Penal II (teoria da pena)", 60),
            new Disciplina("Direito Internacional Público", 60),
            new Disciplina("Teoria Geral do Processo", 60),
            new Disciplina("Hermenêutica Jurídica", 60),
            //
            new Disciplina("Direito Civil III (contratos em espécie)", 60),
            new Disciplina("Direito Civil IV (direitos reais)", 60),
            new Disciplina("Direito Processual Constitucional", 60),
            new Disciplina("Direito Processual III (crimes em espécie)", 60),
            new Disciplina("Direito Processual Civil I", 60),
            new Disciplina("Metodologia da Pesquisa", 60),
            new Disciplina("Estágio I - Laboratório de Prática Jurídica I", 60),
            //
            new Disciplina("Direito Civil V", 60),
            new Disciplina("Direito Empresarial I", 60),
            new Disciplina("Direito do Trabalho I", 60),
            new Disciplina("Direito Processual Penal I", 60),
            new Disciplina("Direito Processual Civil II", 60),
            new Disciplina("Estágio II - Laboratório de Prática Jurídica II", 60),
            new Disciplina("Estágio II - Serviço de Assistência Judiciária I", 60),
            //
            new Disciplina("Direito Empresarial II", 60),
            new Disciplina("Direito Tributário", 60),
            new Disciplina("Direito Internacional Privado", 60),
            new Disciplina("Direito Processual Penal II", 60),
            new Disciplina("Direito do Trabalho II", 60),
            new Disciplina("Ética (geral e jurídica)", 60),
            new Disciplina("Estágio III - Serviço de Assistência Judiciária II", 60),
            new Disciplina("Monografia Final", 60),
            //
        },
    };

    public static List<Faculdade> Faculdades = new()
    {
        new Faculdade("Nova Roma"),
        new Faculdade("UPE"),
        new Faculdade("UFPE"),
        new Faculdade("UFRPE"),
        new Faculdade("IFPE"),
        new Faculdade("USP"),
        new Faculdade("ITA"),
        new Faculdade("Unicamp"),
    };

    public static List<Periodo> Periodos = new()
    {
        new Periodo { Id = "2019.1", FaculdadeId = NovaRoma.Id, Start = new DateOnly(2019, 02, 01), End = new DateOnly(2019, 06, 01) },
        new Periodo { Id = "2019.2", FaculdadeId = NovaRoma.Id, Start = new DateOnly(2019, 07, 01), End = new DateOnly(2019, 12, 01) },
        new Periodo { Id = "2020.1", FaculdadeId = NovaRoma.Id, Start = new DateOnly(2020, 02, 01), End = new DateOnly(2020, 06, 01) },
        new Periodo { Id = "2020.2", FaculdadeId = NovaRoma.Id, Start = new DateOnly(2020, 07, 01), End = new DateOnly(2020, 12, 01) },
        new Periodo { Id = "2021.1", FaculdadeId = NovaRoma.Id, Start = new DateOnly(2021, 02, 01), End = new DateOnly(2021, 06, 01) },
        new Periodo { Id = "2021.2", FaculdadeId = NovaRoma.Id, Start = new DateOnly(2021, 07, 01), End = new DateOnly(2021, 12, 01) },
        new Periodo { Id = "2022.1", FaculdadeId = NovaRoma.Id, Start = new DateOnly(2022, 02, 01), End = new DateOnly(2022, 06, 01) },
        new Periodo { Id = "2022.2", FaculdadeId = NovaRoma.Id, Start = new DateOnly(2022, 07, 01), End = new DateOnly(2022, 12, 01) },
        new Periodo { Id = "2023.1", FaculdadeId = NovaRoma.Id, Start = new DateOnly(2023, 02, 01), End = new DateOnly(2023, 06, 01) },
        new Periodo { Id = "2023.2", FaculdadeId = NovaRoma.Id, Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) },
    };

    public static List<Professor> Professores = new()
    {
        new Professor { Id = Guid.NewGuid(), FaculdadeId = NovaRoma.Id, Nome = "Conde Bregoso", },
        new Professor { Id = Guid.NewGuid(), FaculdadeId = NovaRoma.Id, Nome = "Reginaldo Rossi", },
        new Professor { Id = Guid.NewGuid(), FaculdadeId = NovaRoma.Id, Nome = "Chico Science", },
    };

    public static List<Aluno> Alunos = new()
    {
        new Aluno(NovaRoma.Id, "Ednaldo Pereira"),
        new Aluno(NovaRoma.Id, "Manoel Gomes"),
        new Aluno(NovaRoma.Id, "Zezo"),
    };
}
