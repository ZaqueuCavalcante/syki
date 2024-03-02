# Features

https://youtu.be/xrDPFEH9yIk

- Toda feature possui:
    - Back:
        - Domain
        - Database
        - Service
        - Endpoints
    - Front:
        - Components
        - Pages
    - Tests:
        - Unit
        - Auth
        - Integration
        - Components
        - E2E

- Namespaces:
    - Back.FEATURE.lalala
    - Front.FEATURE.lalala
    - Tests.FEATURE.lalala

# Backlog

- Logs (Serilog)
- Datadog (free?)
- Reduzir custo de infra por uso excessivo de memoria
- Remover todas as snackbars pq sao dificeis de testar????


# Demo

- Retirar o conceito de Demo do sistema
- Adicionar o conceito de Registro/Cadastro de usuario
- TODO usuario vai seguir o fluxo:
    - Pagina inicial -> EXPERIMENTE AGORA -> Pagina de cadastro
    - Informa nome da sua instituticao de ensino + seu email
    - Salvar como cadastro pendente
    - Enviar email (layout simples) com link para definir senha
    - Ao clicar, definir senha e botao pra ir pro login -> login
    - Informar que aquele acesso:
        - Vai ficar ativo por 7 dias
            - Expirar token? mas sempre tem q gerar com a data limite fixa?
            - Gerar token com validade baixa (1 dia?) e no refresh validar se pode obter um novo
            - Deslogar usuario ao receber um 401
            - Nao deixar logar mais depois dessa data e redirecionar pra compra de planos
        - Ja tem dados de seed para facilitar o uso
        - Todos os dados cadastrados durante esses dias serao descartados ao final dos 7 dias
    - Ao assinar um dos planos:
        - Vai receber um sistema totalmente novo/limpo de dados
        - 

# Acesso a outras contas

- Criar endpoint onde um Academico pode chamar (informando o user_id do aluno ou prof) e recebe um link com um token 
- Ao clicar no link, loga como o aluno ou prof
- Riscos de seguranca????



- [] Login
- [] Login MFA











# Features

## Notas

- Uma faculdade pode definir qual a nota media para ser aprovado
    - Essa nota sera usada globalmente para todos os cursos e disciplinas
    - Salvar na propria faculdade, pensar numa tabela especifica mais tarde
    - Valor default = 7

- As notas vao servir para:
    - Determinar se um aluno foi aprovado ou nao numa disciplina que cursou
    - Determinar a media da turma numa disciplina
    - Determinar a media do curso naquela disciplina

- Avaliacoes:
    - Prova 1 + Prova 2
    - Prova Final

## Setup completo pra iniciar um periodo

- Logar como 'Adm'
- Criar uma faculdade (criar periodos do ano corrente + proximo???)
- Criar um usuario Academico pra ela

- Logar como Academico
- Criar um campus
- Criar periodos
- Criar um curso
- Criar disciplinas pro curso
- Criar uma grade pro curso, com as disciplinas
- Criar uma oferta de curso
- Criar professores
- Criar alunos
- Criar turmas e vinculos

- Quais os horarios?
- Aulas?
- Provas?
- Chamadas?

## Periodo completo de um curso

- Professor precisa ver horarios
- Professor precisar dar aula, fazer chamada, e aplicar provas (e notas)



