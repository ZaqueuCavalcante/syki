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
        },
        Cursos = new()
        {
            new Curso { Nome = "Análise e Desenvolvimento de Sistemas" },
            new Curso { Nome = "Direito" },
            new Curso { Nome = "Pedagogia" },
            new Curso { Nome = "Engenharia Civil" },
        },
        Disciplinas = new()
        {
            new Disciplina { Nome = "Matemática Discreta" },
            new Disciplina { Nome = "Introdução ao Desenvolvimento Web" },
            new Disciplina { Nome = "Design de Interação Humano-Máquina" },
            new Disciplina { Nome = "Introdução à Redes de Computadores" },
            new Disciplina { Nome = "Pensamento Computacional e Algoritmos" },
            new Disciplina { Nome = "Projeto Integrador I: Concepção e Prototipação" },
            //
            new Disciplina { Nome = "Banco de Dados" },
            new Disciplina { Nome = "Estrutura de Dados" },
            new Disciplina { Nome = "Informática e Sociedade" },
            new Disciplina { Nome = "Programação Orientada a Objetos" },
            new Disciplina { Nome = "Projeto Integrador 2: Modelagem de Banco de Dados" },
            new Disciplina { Nome = "Arquitetura de Computadores e Sistemas Operacionais" },
            //
            new Disciplina { Nome = "Estatística Aplicada" },
            new Disciplina { Nome = "Arquitetura de Software" },
            new Disciplina { Nome = "Análise e Projeto de Software" },
            new Disciplina { Nome = "Computação em Nuvem e Web Services" },
            new Disciplina { Nome = "Configuração e Manutenção de Software" },
            new Disciplina { Nome = "Projeto Integrador 3: Desenvolvimento Full Stack" },
            //
            new Disciplina { Nome = "Sistemas Distribuídos" },
            new Disciplina { Nome = "Inovação e Empreendedorismo" },
            new Disciplina { Nome = "Análise e Visualização de Dados" },
            new Disciplina { Nome = "Desenvolvimentos de Aplicações Móveis" },
            new Disciplina { Nome = "Gestão de Projetos e Governança de TI" },
            new Disciplina { Nome = "Projeto Integrador 4: Aplicações Móveis" },
            //
            new Disciplina { Nome = "Libras" },
            new Disciplina { Nome = "Sistemas Embarcados" },
            new Disciplina { Nome = "Big Data e Data Science" },
            new Disciplina { Nome = "Inteligência Artificial" },
            new Disciplina { Nome = "Segurança da Informação" },
            new Disciplina { Nome = "Testes e Verificação de Software" },
            new Disciplina { Nome = "Projeto Integrador 5: Sistemas Inteligentes" },
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
}
