# TODOS





Adicionei Eventos de Domínio no projeto!

Estou desenvolvendo...

Cada evento representa algo que aconteceu no sistema e serve como trigger para realização de outras tarefas.

Exemplos:

1️⃣ Quando o usuário acessa o endpoit de redefinição de senha, o evento ResetPasswordTokenCreatedDomainEvent é emitido.
O handler desse evento enfileira enfileira uma tarefa que envia para o email do usuário o link de redefinição de senha.

2️⃣ Quando um professor adiciona a nota de um aluno no sistema, o evento ExamGradeNoteAddedDomainEvent é emitido.
O handler desse evento enfileira uma tarefa que cria uma notificação pro aluno, informando que uma nova nota naquela disciplina foi adicionada.

3️⃣ Quando uma instituição é criada, o evento InstitutionCreatedDomainEvent é emitido.
O handler desse evento enfileira uma tarefa que realiza o seed de dados para aquela instituição, facilitando a vida de quem está apenas testando o sistema.

Esses eventos podem servir no futuro para o disparo de WebHooks, deixando a integração com outros sistemas mais dinâmica.

- A API gera os eventos
- O Daemon os processa e enfileira as tarefas
- O Daemon também processa as tarefas depois
- Uso o LISTEN/NOTIFY do PostgreSQL
- Tenho testes de integração validando emissão e processamento de eventos e tarefas
- Criei uma tela pro usuário Admin conseguir visualizar tudo isso


- Filtrar por instituição / por evento / por tarefa / por erros
- Consigo reprocessar um evento / tarefa






Exemplos de eventos:

- StudentCreatedDomainEvent
- TeacherCreatedDomainEvent
- ExamGradeNoteAddedDomainEvent
- InstitutionCreatedDomainEvent
- PendingUserRegisterCreatedDomainEvent
- ResetPasswordTokenCreatedDomainEvent














- Elestio Moodle: Free Open-source Learning Management System (LMS)
- Diferenças entre LMS (Moodle, Classroom e Teams)
- Classroom, Edmodo, Moodle, Schoology e Teams... Que LMS escolher?

## Domain Events + Outbox Pattern + Background Tasks

- Salvar os eventos de dominio na mesma transacao com o banco

- Algo vai processar os eventos depois
    - Mostrar no front tudo que ta sendo processado
    - Mostrar coisas com erro tbm

- Implementar retentativas automaticas e manuais






## Observabilidade

- OpenTelemetry + Jaeger
- Contar quantos requests foram feitos durante a execucao dos testes de integracao
- Endpoints + quantas chamadas tiveram + qual o retorno (sucesso vs erro)



## Identity

- Token de reset de senha expirado (setar pro teste)
- Locked out




# 15 Years of Software Engineer Knowledge

## 1 - Fundamental skills of coding

- Code, not tutorial hell

## 2 - Netwotk feedback and growth

## 3 - Tech interviwes

## 4 - Succeeding in your job

## 5 - Skillful self-advocacy and managing your career




