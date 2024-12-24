# Eventos de Domínio

Estou desenvolvendo um projeto open source, voltado para o gerenciamento acadêmico de faculdades. Ele possui fluxos naturalmente assíncronos, como envio de emails, notificações e seed de dados.

Uma solução comum para implementação desses fluxos é a utilização de Eventos de Domínio: cada evento representa algo que aconteceu no sistema e serve como trigger para realização de tarefas em outro processo.

Exemplos:

1️⃣ Quando o usuário acessa o endpoit de redefinição de senha, o evento ResetPasswordTokenCreatedDomainEvent é emitido.
O handler desse evento enfileira enfileira uma tarefa que envia para o email do usuário o link de redefinição de senha.

2️⃣ Quando um professor adiciona a nota de um aluno no sistema, o evento ExamGradeNoteAddedDomainEvent é emitido.
O handler desse evento enfileira uma tarefa que cria uma notificação pro aluno, informando que uma nova nota naquela disciplina foi adicionada.

3️⃣ Quando uma instituição é criada, o evento InstitutionCreatedDomainEvent é emitido.
O handler desse evento enfileira uma tarefa que realiza o seed de dados para aquela instituição, facilitando a vida de quem está apenas testando o sistema.

Esses eventos podem ter falhas no processamento, então é possível reprocessá-los. Eles são imutáveis, ou seja, seu reprocessamento é feito com a emissão de um novo evento, como os mesmos dados do anterior.

Eles também podem servir no futuro para o disparo de WebHooks, deixando a integração com outros sistemas mais dinâmica.

## Como funciona

- 0. Quando uma requisição é feita para a API, as entidades e os eventos do domínio são salvos em uma única transação com o banco de dados
- Uso o LISTEN/NOTIFY do PostgreSQL
- 1. O Daemon processa os eventos pendentes e enfileira as tarefas
- 2. Ele também é responsável por processar as tarefas pendentes

- Tenho testes de integração validando emissão e processamento de eventos e tarefas
- Criei uma tela pro usuário Admin conseguir visualizar tudo isso


- Filtrar por instituição / por evento / por tarefa / por erros
- Consigo reprocessar um evento / tarefa

- Implementar retentativas automaticas e manuais

- Rotinas para limpar essas tabelas todo dia meia noite?
    - Jogar para tabelas __processed


