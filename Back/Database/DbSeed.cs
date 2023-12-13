using Syki.Shared;
using Syki.Back.Domain;

namespace Syki.Back.Database;

public static class DbSeed
{
    public static Guid Id = Guid.Parse("8d08e437-8b18-4a15-a231-4a2260e60432");

    public static Faculdade NovaRoma = new()
    {
        Id = Id,
        Nome = "Nova Roma",
        Campi = new()
        {
            new Campus(Id, "Agreste I", "Caruaru - PE"),
            new Campus(Id, "Suassuna", "Recife - PE"),
            new Campus(Id, "Garoa II", "Garanhuns - PE"),
        },
        Cursos = new()
        {
            new Curso(Id, "Administração", TipoDeCurso.Mestrado),
            new Curso(Id, "Análise e Desenvolvimento de Sistemas", TipoDeCurso.Bacharelado),
            new Curso(Id, "Arquitetura e Urbanismo", TipoDeCurso.Tecnologo),
            new Curso(Id, "Ciência da Computação", TipoDeCurso.Bacharelado),
            new Curso(Id, "Direito", TipoDeCurso.Licenciatura),
            new Curso(Id, "Engenharia Civil", TipoDeCurso.Bacharelado),
            new Curso(Id, "Engenharia Mecânica", TipoDeCurso.Bacharelado),
            new Curso(Id, "Engenharia de Produção", TipoDeCurso.PosDoutorado),
            new Curso(Id, "Pedagogia", TipoDeCurso.Licenciatura),
        },
        Disciplinas = new()
        {
            new Disciplina(Id, "Matemática Discreta", 55),
            new Disciplina(Id, "Introdução ao Desenvolvimento Web", 40),
            new Disciplina(Id, "Design de Interação Humano-Máquina", 60),
            new Disciplina(Id, "Introdução à Redes de Computadores", 50),
            new Disciplina(Id, "Pensamento Computacional e Algoritmos", 45),
            new Disciplina(Id, "Projeto Integrador I: Concepção e Prototipação", 25),
            //
            new Disciplina(Id, "Banco de Dados", 80),
            new Disciplina(Id, "Estrutura de Dados", 50),
            new Disciplina(Id, "Informática e Sociedade", 30),
            new Disciplina(Id, "Programação Orientada a Objetos", 40),
            new Disciplina(Id, "Projeto Integrador II: Modelagem de Banco de Dados", 20),
            new Disciplina(Id, "Arquitetura de Computadores e Sistemas Operacionais", 60),
            //
            new Disciplina(Id, "Estatística Aplicada", 55),
            new Disciplina(Id, "Arquitetura de Software", 65),
            new Disciplina(Id, "Análise e Projeto de Software", 45),
            new Disciplina(Id, "Computação em Nuvem e Web Services", 50),
            new Disciplina(Id, "Configuração e Manutenção de Software", 60),
            new Disciplina(Id, "Projeto Integrador III: Desenvolvimento Full Stack", 30),
            //
            new Disciplina(Id, "Sistemas Distribuídos", 60),
            new Disciplina(Id, "Inovação e Empreendedorismo", 60),
            new Disciplina(Id, "Análise e Visualização de Dados", 60),
            new Disciplina(Id, "Desenvolvimentos de Aplicações Móveis", 60),
            new Disciplina(Id, "Gestão de Projetos e Governança de TI", 60),
            new Disciplina(Id, "Projeto Integrador IV: Aplicações Móveis", 35),
            //
            new Disciplina(Id, "Libras", 60),
            new Disciplina(Id, "Sistemas Embarcados", 60),
            new Disciplina(Id, "Big Data e Data Science", 60),
            new Disciplina(Id, "Inteligência Artificial", 60),
            new Disciplina(Id, "Segurança da Informação", 60),
            new Disciplina(Id, "Testes e Verificação de Software", 60),
            new Disciplina(Id, "Projeto Integrador V: Sistemas Inteligentes", 30),
            //
            //
            new Disciplina(Id, "Direito e Economia", 60),
            new Disciplina(Id, "Introdução ao Direito", 60),
            new Disciplina(Id, "História das Instituições Jurídicas", 60),
            new Disciplina(Id, "Teoria do Estado, Política e Direito", 60),
            new Disciplina(Id, "Sociologia Jurídica", 60),
            new Disciplina(Id, "Antropologia Jurídica", 60),
            //
            new Disciplina(Id, "Direito Civil I (parte geral)", 60),
            new Disciplina(Id, "Direito Constitucional", 60),
            new Disciplina(Id, "Direito Financeiro", 60),
            new Disciplina(Id, "Direito Penal I (parte geral)", 60),
            new Disciplina(Id, "Filosofia Geral e Jurídica", 60),
            //
            new Disciplina(Id, "Direito Civil II (obrigações e contratos)", 60),
            new Disciplina(Id, "Direito Administrativo", 60),
            new Disciplina(Id, "Direito Penal II (teoria da pena)", 60),
            new Disciplina(Id, "Direito Internacional Público", 60),
            new Disciplina(Id, "Teoria Geral do Processo", 60),
            new Disciplina(Id, "Hermenêutica Jurídica", 60),
            //
            new Disciplina(Id, "Direito Civil III (contratos em espécie)", 60),
            new Disciplina(Id, "Direito Civil IV (direitos reais)", 60),
            new Disciplina(Id, "Direito Processual Constitucional", 60),
            new Disciplina(Id, "Direito Processual III (crimes em espécie)", 60),
            new Disciplina(Id, "Direito Processual Civil I", 60),
            new Disciplina(Id, "Metodologia da Pesquisa", 60),
            new Disciplina(Id, "Estágio I - Laboratório de Prática Jurídica I", 60),
            //
            new Disciplina(Id, "Direito Civil V", 60),
            new Disciplina(Id, "Direito Empresarial I", 60),
            new Disciplina(Id, "Direito do Trabalho I", 60),
            new Disciplina(Id, "Direito Processual Penal I", 60),
            new Disciplina(Id, "Direito Processual Civil II", 60),
            new Disciplina(Id, "Estágio II - Laboratório de Prática Jurídica II", 60),
            new Disciplina(Id, "Estágio II - Serviço de Assistência Judiciária I", 60),
            //
            new Disciplina(Id, "Direito Empresarial II", 60),
            new Disciplina(Id, "Direito Tributário", 60),
            new Disciplina(Id, "Direito Internacional Privado", 60),
            new Disciplina(Id, "Direito Processual Penal II", 60),
            new Disciplina(Id, "Direito do Trabalho II", 60),
            new Disciplina(Id, "Ética (geral e jurídica)", 60),
            new Disciplina(Id, "Estágio III - Serviço de Assistência Judiciária II", 60),
            new Disciplina(Id, "Monografia Final", 60),
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
