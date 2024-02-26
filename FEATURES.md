# Features

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
- Criar CLIENTS no front e usar em todos os testes pra chamada de API...


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



- [] Finish register
- [] Get Mfa key
- [] Setup MFA
- [] Login
- [] Login MFA



