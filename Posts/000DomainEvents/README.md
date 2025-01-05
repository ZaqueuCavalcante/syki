# Domain Events + Outbox Pattern

Estou desenvolvendo um projeto open source, voltado para o gerenciamento acadêmico de instituições de ensino. Ele possui fluxos naturalmente assíncronos, como envio de emails, notificações e seed de dados.

Uma solução comum para implementação desses fluxos é a utilização de Eventos de Domínio: cada evento representa algo que aconteceu no sistema e serve como trigger para realização de uma ou mais tarefas em outro processo. Eles também podem servir para o disparo de WebHooks, deixando a integração com outros sistemas mais dinâmica.

## Exemplos

1️⃣ Quando o usuário acessa o endpoint de redefinição de senha, o evento ResetPasswordTokenCreatedDomainEvent é emitido.
O handler desse evento enfileira uma tarefa que envia para o email do usuário o link de redefinição de senha.

2️⃣ Quando um professor adiciona a nota de um aluno no sistema, o evento ExamGradeNoteAddedDomainEvent é emitido.
O handler desse evento enfileira uma tarefa que cria uma notificação pro aluno, informando que uma nova nota naquela disciplina foi adicionada.

## Como funciona

- O Outbox Pattern é utilizado para garantir a consistência dos dados, processando os eventos posteriormente em background
- A API processa os requests de maneira atômica, ou seja, salva os dados no banco utilizando uma única transação
- Uso o recurso LISTEN/NOTIFY do PostgreSQL para notificar o Daemon que existem eventos pendentes de processamento no banco
- O Daemon processa os eventos pendentes e enfileira as respectivas tarefas
- O mesmo Daemon é notificado da existência de tarefas pendentes, as busca e as processa

## Gerenciamento

- É possível filtrar os eventos e as tarefas por diversos atributos, como tipo, status e instituição
- Dá pra reprocessar tarefas com erro, enfileirando uma nova tarefa com os mesmos dados da anterior
- Criei duas rotinas que migram os eventos e tarefas já processadas para outras tabelas, deixando as consultas mais performáticas
