# Processamento Assíncrono - Eventos, Comandos, Lotes e Workflows

Estou desenvolvendo um projeto open source, voltado para o gerenciamento acadêmico de instituições de ensino.

Nele, quando um professor de uma turma publica uma nova atividade (trabalho, pesquisa, apresentação...), todos os alunos da turma precisam ser notificados.
Essa notificação é feita de duas formas:
    - Dentro do próprio sistema, via notificações internas vinculadas à cada aluno
    - Fora do sistema, enviando um email para cada aluno da turma através de um serviço externo (Brevo, Mailchimp, SendGrid...)
Ao final, quando todos os emails forem enviados, o sistema deve notificar internamente o professor, informando que a atividade foi publicada com sucesso.

Ficaria muito complicado fazer tudo no mesmo request né? Sem contar que a api de envio de email pode retornar algum erro, de modo que alunos podem não receber o email. Nesse caso seria interessante ter algum mecanismo de retry automático também. Como você implementaria isso?

Agora vamos pensar em outro caso de uso, dessa vez mais relacionado com o fluxo de desenvolvimento: frequentemente preciso subir o sistema localmente para testar as funcionalidades como um usuário final. Por exemplo, para poder publicar uma atividade como no caso acima, são necessários alguns passos antes:

- Cadastrar uma nova instituição de ensino + usuário acadêmico
- Logado como usuário acadêmico, preciso realizar o cadastro de campus, cursos, disciplinas, grades curriculares, alunos, professores, período acadêmico, turmas e aulas.
- Os alunos precisam logar no sistema e realizar sua matrícula nas turmas que foram abertas.

Somente ao final de tudo isso, posso logar como professor e publicar uma nova atividade. Para facilitar minha vida e trazer agilidade pro desenvolvimento, criei um único método para realizar esse seed de dados básicos, mas como no caso anterior, é muito código para ser executado de uma vez só. Seria mais interessante ter como dividir o seed em uma sequência de passos menores, onde cada um executasse ao final do outro. Como você implementaria isso?










 



Então é bem simples: o professor logado no sistema informa dados sobre a atividade e clica em publicar. Nesse momento o front chama a api com os dados, onde temos um serviço que vai criar a atividade, 


Como você implementaria isso? Quais pontos levaria em conta na sua solução?




Sua arquitetura atual é bem simples: frontend (Blazor), api (.NET), daemon (.NET) e banco (PostgreSQL).

Ele possui algumas funcionalidades que necessitam de processamento assíncrono, como:
- Envio de emails
- Gestão de notificações
- Seed de dados (workflow)
- Chamadas de webhooks
- Integração com outros sistemas

Neste artigo veremos como utilizar eventos de domínio, comandos e lotes para implementar essas funcionalidades de maneira performática e resiliente.





## Exemplos

1️⃣ Quando um professor publica uma nova atividade, todos os alunos da turma devem ser notificados via email. O envio dos emails deve ser feito de maneira assíncrona, assim a API pode responder rapidamente que os alunos estão sendo notificados e caso algum envio dê errado, podemos realizar retentativas isoladamente. Se todos os envios forem feitos com sucesso, o sistema notifica internamente o professor, sinalizando que todos os alunos receberam o email de nova atividade.

2️⃣ Durante o desenvolvimento, é muito comum realizar o seed de dados no banco para conseguir testar o sistema via navegador, exatamente como o usuário final fará. No caso, eu repetidamente preciso fazer o seed de campus, cursos, disciplinas, grades curriculares, alunos, professores, turmas, aulas, chamadas, atividades e notas. Como você pode imaginar, fica cada vez mais complexo realizar o seed se tudo isso for feito num único processo, por isso dividi o seed em comandos que executam sequencialmente.

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




