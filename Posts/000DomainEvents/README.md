# Processamento Assíncrono - Eventos, Comandos e Lotes

Estou desenvolvendo um projeto open source, voltado para o gerenciamento acadêmico de instituições de ensino.

A arquitetura é bem simples: frontend, api, daemon e banco.

GIF DOS COMPONENTES AQUI

Ele possui algumas funcionalidades que necessitam de processamento assíncrono, como envio de emails, gestão de notificações e seed de dados. Neste artigo veremos como utilizar eventos de domínio, comandos e lotes para implementar essas funcionalidades.

## Exemplos

1️⃣ Quando um professor publica uma nova atividade, todos os alunos da turma devem ser notificados via email. O envio dos emails deve ser feito de maneira assíncrona, assim a API pode responder rapidamente que os alunos estão sendo notificados e caso algum envio dê errado, podemos realizar retentativas isoladamente. Se todos os envios forem feitos com sucesso, o sistema notifica internamente o professor, sinalizando que todos os alunos receberam o email de nova atividade.

2️⃣ Durante o desenvolvimento, é muito comum realizar o seed de dados no banco para conseguir testar o sistema via navegador, exatamente como o usuário final fará. No caso, eu repetidamente preciso fazer o seed de cursos, disciplinas, grades curriculares, alunos, professores, turmas, aulas, chamadas, atividades e notas. Como você pode imaginar, fica cada vez mais complexo realizar o seed se tudo isso for feito num único processo, por isso dividi o seed em comandos que executam sequencialmente.

## Como funciona

Componentes fundamentais:

- Entidade: uma classe do sistema capaz de emitir um Evento de Domínio. Exemplo: ClassActivity (Atividade dentro de uma turma)
- Evento de Domínio: representa que algo aconteceu no sistema. Exemplo: ClassActivityCreatedDomainEvent (Atividade criada)
- Comando: representa um processamento assíncrono qualquer dentro do sistema. Exemplo: CreateNewClassActivityNotificationCommand (Criar notificação de nova atividade)
- Lote: um agrupamento lógico de comandos. Exemplo: SendNewClassActivityEmailCommands (Enviar emails de nova atividade)

- Triggers: utilizados para chamar funções no banco de dados quando algo acontece
- Postgres LISTEN/NOTIFY: quando um novo evento é salvo no banco de dados, o Daemon é notificado e vai no banco buscar os eventos pendentes
- Tudo é feito usando locks e transações, evitando que os eventos sejam processados em duplicidade

- Seguindo a mesma lógica os comandos são processados

- Existe um trigger especial para a gestão dos lotes



## Gerenciamento

- É possível filtrar os eventos e as tarefas por diversos atributos, como tipo, status e instituição
- Dá pra reprocessar tarefas com erro, enfileirando uma nova tarefa com os mesmos dados da anterior
- Criei duas rotinas que migram os eventos e tarefas já processadas para outras tabelas, deixando as consultas mais performáticas










## Eventos de Domínio + Comandos

- Diagramas no Draw.io
    - Eventos
    - Comandos
    - Lotes
    - Listeners + Processors

- Eventos de Domínio
    - Interceptor
    - Sempre estão associados à uma Instituição e à uma Entidade

- Comandos
    - São sempre processados atomicamente
    - Podem ser processados com sucesso ou falha
    - Podem enfileirar outros comandos
    - Podem estar dentro de um lote

- Batch de Comandos
    - Vários comandos que continuam a ser processados atomicamente, mas estão vinculados à um mesmo lote
    - O lote só é processado com sucesso caso todos os seus comandos sejam processados com sucesso
    - Quando o lote é finalizado com sucesso, podemos executar um outro comando em seguida

- Reprocessamento de comandos com erro
    - Quando um comando dá erro, podemos reprocessá-lo de maneira automática ou manual
    - Podemos definir políticas de retry para o reprocessamento do comando com erro
    - A análisa deve ser feita caso a caso, evitando inconsistência nos dados

- Limpeza dos dados
    - Podemos ter tabelas específicas para armazenar os eventos e comandos já processados
    - Essas tabelas são otimizadas para leitura, já que seus dados são imutáveis
    - Rotinas para limpezado dos dados que rodam toda madrugada

- Análise em tela dos eventos e comandos pendentes/processados/sucessos/erros
    - Cada evento pode enfileirar um ou mais comandos
    - Um comando pode enfileirar um ou mais comandos
    - Um comando com erro pode ser reprocessado, gerando um novo comando com os mesmos dados
    - Mostrar em tela os lotes de comandos

- Hangfire em memória dando erro?
    - Subir instância do Redis?




