using Syki.Domain;

namespace Syki.Database;

public static class DbSeed
{
    public static Faculdade NovaRoma = new()
    {
        Nome = "Nova Roma",
        Campi = new()
        {
            new Campus { Nome = "Caruaru" },
            new Campus { Nome = "Recife" },
            new Campus { Nome = "Garanhuns" },
        },
        Cursos = new()
        {
            new Curso { Nome = "Administração" },
            new Curso { Nome = "Análise e Desenvolvimento de Sistemas" },
            new Curso { Nome = "Arquitetura e Urbanismo" },
            new Curso { Nome = "Ciência da Computação" },
            new Curso { Nome = "Direito" },
            new Curso { Nome = "Engenharia Civil" },
            new Curso { Nome = "Engenharia Mecânica" },
            new Curso { Nome = "Engenharia de Produção" },
            new Curso { Nome = "Pedagogia" },
        },
        Disciplinas = new()
        {
            new Disciplina { Nome = "Matemática Discreta", CargaHoraria = 55 },  // 001
            new Disciplina { Nome = "Introdução ao Desenvolvimento Web", CargaHoraria = 40 },
            new Disciplina { Nome = "Design de Interação Humano-Máquina", CargaHoraria = 60 },
            new Disciplina { Nome = "Introdução à Redes de Computadores", CargaHoraria = 60 },
            new Disciplina { Nome = "Pensamento Computacional e Algoritmos", CargaHoraria = 60 },
            new Disciplina { Nome = "Projeto Integrador I: Concepção e Prototipação", CargaHoraria = 25 },
            //
            new Disciplina { Nome = "Banco de Dados", CargaHoraria = 60 },  // 007
            new Disciplina { Nome = "Estrutura de Dados", CargaHoraria = 60 },
            new Disciplina { Nome = "Informática e Sociedade", CargaHoraria = 60 },
            new Disciplina { Nome = "Programação Orientada a Objetos", CargaHoraria = 60 },
            new Disciplina { Nome = "Projeto Integrador II: Modelagem de Banco de Dados", CargaHoraria = 30 },
            new Disciplina { Nome = "Arquitetura de Computadores e Sistemas Operacionais", CargaHoraria = 60 },
            //
            new Disciplina { Nome = "Estatística Aplicada", CargaHoraria = 60 },  // 013
            new Disciplina { Nome = "Arquitetura de Software", CargaHoraria = 60 },
            new Disciplina { Nome = "Análise e Projeto de Software", CargaHoraria = 60 },
            new Disciplina { Nome = "Computação em Nuvem e Web Services", CargaHoraria = 60 },
            new Disciplina { Nome = "Configuração e Manutenção de Software", CargaHoraria = 60 },
            new Disciplina { Nome = "Projeto Integrador III: Desenvolvimento Full Stack", CargaHoraria = 30 },
            //
            new Disciplina { Nome = "Sistemas Distribuídos", CargaHoraria = 60 },  // 019
            new Disciplina { Nome = "Inovação e Empreendedorismo", CargaHoraria = 60 },
            new Disciplina { Nome = "Análise e Visualização de Dados", CargaHoraria = 60 },
            new Disciplina { Nome = "Desenvolvimentos de Aplicações Móveis", CargaHoraria = 60 },
            new Disciplina { Nome = "Gestão de Projetos e Governança de TI", CargaHoraria = 60 },
            new Disciplina { Nome = "Projeto Integrador IV: Aplicações Móveis", CargaHoraria = 35 },
            //
            new Disciplina { Nome = "Libras", CargaHoraria = 60 },  // 25
            new Disciplina { Nome = "Sistemas Embarcados", CargaHoraria = 60 },
            new Disciplina { Nome = "Big Data e Data Science", CargaHoraria = 60 },
            new Disciplina { Nome = "Inteligência Artificial", CargaHoraria = 60 },
            new Disciplina { Nome = "Segurança da Informação", CargaHoraria = 60 },
            new Disciplina { Nome = "Testes e Verificação de Software", CargaHoraria = 60 },
            new Disciplina { Nome = "Projeto Integrador V: Sistemas Inteligentes", CargaHoraria = 30 },
            //
            //
            new Disciplina { Nome = "Direito e Economia", CargaHoraria = 60 },  // 032
            new Disciplina { Nome = "Introdução ao Direito", CargaHoraria = 60 },
            new Disciplina { Nome = "História das Instituições Jurídicas", CargaHoraria = 60 },
            new Disciplina { Nome = "Teoria do Estado, Política e Direito", CargaHoraria = 60 },
            new Disciplina { Nome = "Sociologia Jurídica", CargaHoraria = 60 },
            new Disciplina { Nome = "Antropologia Jurídica", CargaHoraria = 60 },
            //
            new Disciplina { Nome = "Direito Civil I (parte geral)", CargaHoraria = 60 },
            new Disciplina { Nome = "Direito Constitucional", CargaHoraria = 60 },
            new Disciplina { Nome = "Direito Financeiro", CargaHoraria = 60 },
            new Disciplina { Nome = "Direito Penal I (parte geral)", CargaHoraria = 60 },
            new Disciplina { Nome = "Filosofia Geral e Jurídica", CargaHoraria = 60 },
            //
            new Disciplina { Nome = "Direito Civil II (obrigações e contratos)", CargaHoraria = 60 },
            new Disciplina { Nome = "Direito Administrativo", CargaHoraria = 60 },
            new Disciplina { Nome = "Direito Penal II (teoria da pena)", CargaHoraria = 60 },
            new Disciplina { Nome = "Direito Internacional Público", CargaHoraria = 60 },
            new Disciplina { Nome = "Teoria Geral do Processo", CargaHoraria = 60 },
            new Disciplina { Nome = "Hermenêutica Jurídica", CargaHoraria = 60 },
            //
            new Disciplina { Nome = "Direito Civil III (contratos em espécie)", CargaHoraria = 60 },
            new Disciplina { Nome = "Direito Civil IV (direitos reais)", CargaHoraria = 60 },
            new Disciplina { Nome = "Direito Processual Constitucional", CargaHoraria = 60 },
            new Disciplina { Nome = "Direito Processual III (crimes em espécie)", CargaHoraria = 60 },
            new Disciplina { Nome = "Direito Processual Civil I", CargaHoraria = 60 },
            new Disciplina { Nome = "Metodologia da Pesquisa", CargaHoraria = 60 },
            new Disciplina { Nome = "Estágio I - Laboratório de Prática Jurídica I", CargaHoraria = 60 },
            //
            new Disciplina { Nome = "Direito Civil V", CargaHoraria = 60 },
            new Disciplina { Nome = "Direito Empresarial I", CargaHoraria = 60 },
            new Disciplina { Nome = "Direito do Trabalho I", CargaHoraria = 60 },
            new Disciplina { Nome = "Direito Processual Penal I", CargaHoraria = 60 },
            new Disciplina { Nome = "Direito Processual Civil II", CargaHoraria = 60 },
            new Disciplina { Nome = "Estágio II - Laboratório de Prática Jurídica II", CargaHoraria = 60 },
            new Disciplina { Nome = "Estágio II - Serviço de Assistência Judiciária I", CargaHoraria = 60 },
            //
            new Disciplina { Nome = "Direito Empresarial II", CargaHoraria = 60 },
            new Disciplina { Nome = "Direito Tributário", CargaHoraria = 60 },
            new Disciplina { Nome = "Direito Internacional Privado", CargaHoraria = 60 },
            new Disciplina { Nome = "Direito Processual Penal II", CargaHoraria = 60 },
            new Disciplina { Nome = "Direito do Trabalho II", CargaHoraria = 60 },
            new Disciplina { Nome = "Ética (geral e jurídica)", CargaHoraria = 60 },
            new Disciplina { Nome = "Estágio III - Serviço de Assistência Judiciária II", CargaHoraria = 60 },
            new Disciplina { Nome = "Monografia Final", CargaHoraria = 60 },
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
        new Periodo { Id = "2019.1", FaculdadeId = 1, Start = new DateOnly(2019, 02, 01), End = new DateOnly(2019, 06, 01) },
        new Periodo { Id = "2019.2", FaculdadeId = 1, Start = new DateOnly(2019, 07, 01), End = new DateOnly(2019, 12, 01) },
        new Periodo { Id = "2020.1", FaculdadeId = 1, Start = new DateOnly(2020, 02, 01), End = new DateOnly(2020, 06, 01) },
        new Periodo { Id = "2020.2", FaculdadeId = 1, Start = new DateOnly(2020, 07, 01), End = new DateOnly(2020, 12, 01) },
        new Periodo { Id = "2021.1", FaculdadeId = 1, Start = new DateOnly(2021, 02, 01), End = new DateOnly(2021, 06, 01) },
        new Periodo { Id = "2021.2", FaculdadeId = 1, Start = new DateOnly(2021, 07, 01), End = new DateOnly(2021, 12, 01) },
        new Periodo { Id = "2022.1", FaculdadeId = 1, Start = new DateOnly(2022, 02, 01), End = new DateOnly(2022, 06, 01) },
        new Periodo { Id = "2022.2", FaculdadeId = 1, Start = new DateOnly(2022, 07, 01), End = new DateOnly(2022, 12, 01) },
        new Periodo { Id = "2023.1", FaculdadeId = 1, Start = new DateOnly(2023, 02, 01), End = new DateOnly(2023, 06, 01) },
        new Periodo { Id = "2023.2", FaculdadeId = 1, Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) },
    };

    public static List<Professor> Professores = new()
    {
        new Professor { FaculdadeId = 1, Nome = "Conde Bregoso", },
        new Professor { FaculdadeId = 1, Nome = "Reginaldo Rossi", },
        new Professor { FaculdadeId = 1, Nome = "Chico Science", },
    };

    public static List<Aluno> Alunos = new()
    {
        new Aluno(1, "Ednaldo Pereira"),
        new Aluno(1, "Manoel Gomes"),
        new Aluno(1, "Zezo"),
    };
}
