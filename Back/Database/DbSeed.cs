using Syki.Shared;
using Syki.Back.Domain;

namespace Syki.Back.Database;

public static class DbSeed
{
    public static Faculdade NovaRoma = new()
    {
        Id = Guid.Parse("8d08e437-8b18-4a15-a231-4a2260e60432"),
        Nome = "Nova Roma",
        Campi = new()
        {
            new Campus("Agreste I", "Caruaru - PE"),
            new Campus("Suassuna", "Recife - PE"),
            new Campus("Garoa II", "Garanhuns - PE"),
        },
        Cursos = new()
        {
            new Curso("Administração", TipoDeCurso.Mestrado),
            new Curso("Análise e Desenvolvimento de Sistemas", TipoDeCurso.Bacharelado),
            new Curso("Arquitetura e Urbanismo", TipoDeCurso.Tecnologo),
            new Curso("Ciência da Computação", TipoDeCurso.Bacharelado),
            new Curso("Direito", TipoDeCurso.Licenciatura),
            new Curso("Engenharia Civil", TipoDeCurso.Bacharelado),
            new Curso("Engenharia Mecânica", TipoDeCurso.Bacharelado),
            new Curso("Engenharia de Produção", TipoDeCurso.PosDoutorado),
            new Curso("Pedagogia", TipoDeCurso.Licenciatura),
        },
        Disciplinas = new()
        {
            new Disciplina("Matemática Discreta", 55),
            new Disciplina("Introdução ao Desenvolvimento Web", 40),
            new Disciplina("Design de Interação Humano-Máquina", 60),
            new Disciplina("Introdução à Redes de Computadores", 50),
            new Disciplina("Pensamento Computacional e Algoritmos", 45),
            new Disciplina("Projeto Integrador I: Concepção e Prototipação", 25),
            //
            new Disciplina("Banco de Dados", 80),
            new Disciplina("Estrutura de Dados", 50),
            new Disciplina("Informática e Sociedade", 30),
            new Disciplina("Programação Orientada a Objetos", 40),
            new Disciplina("Projeto Integrador II: Modelagem de Banco de Dados", 20),
            new Disciplina("Arquitetura de Computadores e Sistemas Operacionais", 60),
            //
            new Disciplina("Estatística Aplicada", 55),
            new Disciplina("Arquitetura de Software", 65),
            new Disciplina("Análise e Projeto de Software", 45),
            new Disciplina("Computação em Nuvem e Web Services", 50),
            new Disciplina("Configuração e Manutenção de Software", 60),
            new Disciplina("Projeto Integrador III: Desenvolvimento Full Stack", 30),
            //
            new Disciplina("Sistemas Distribuídos", 60),
            new Disciplina("Inovação e Empreendedorismo", 60),
            new Disciplina("Análise e Visualização de Dados", 60),
            new Disciplina("Desenvolvimentos de Aplicações Móveis", 60),
            new Disciplina("Gestão de Projetos e Governança de TI", 60),
            new Disciplina("Projeto Integrador IV: Aplicações Móveis", 35),
            //
            new Disciplina("Libras", 60),
            new Disciplina("Sistemas Embarcados", 60),
            new Disciplina("Big Data e Data Science", 60),
            new Disciplina("Inteligência Artificial", 60),
            new Disciplina("Segurança da Informação", 60),
            new Disciplina("Testes e Verificação de Software", 60),
            new Disciplina("Projeto Integrador V: Sistemas Inteligentes", 30),
            //
            //
            new Disciplina("Direito e Economia", 60),
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

    public static List<Periodo> Periodos = new()
    {
        new Periodo { Id = "2023.1", FaculdadeId = NovaRoma.Id, Start = new DateOnly(2023, 02, 01), End = new DateOnly(2023, 06, 01) },
        new Periodo { Id = "2023.2", FaculdadeId = NovaRoma.Id, Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) },
        new Periodo { Id = "2024.1", FaculdadeId = NovaRoma.Id, Start = new DateOnly(2024, 02, 01), End = new DateOnly(2024, 06, 01) },
    };

    // public static List<Professor> Professores = new()
    // {
    //     new Professor(NovaRoma.Id, "Richard Feynman"),
    //     new Professor(NovaRoma.Id, "Robert Oppenheimer"),
    //     new Professor(NovaRoma.Id, "Reginaldo Rossi"),
    //     new Professor(NovaRoma.Id, "Chico Science"),
    // };

    // public static List<Aluno> Alunos = new()
    // {
    //     new Aluno(NovaRoma.Id, "Marieli Lemes"),
    //     new Aluno(NovaRoma.Id, "Bianca Rios"),
    //     new Aluno(NovaRoma.Id, "Ednaldo Pereira"),
    //     new Aluno(NovaRoma.Id, "Manoel Gomes"),
    //     new Aluno(NovaRoma.Id, "Zezo Potiguar"),
    //     new Aluno(NovaRoma.Id, "Carlos Alberto"),
    // };
}
