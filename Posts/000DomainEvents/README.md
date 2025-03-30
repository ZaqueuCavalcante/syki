# Processamento Assíncrono - Eventos, Comandos, Lotes e Workflows

Estou desenvolvendo um projeto open source, voltado para o gerenciamento acadêmico de instituições de ensino.

Nele, quando um professor de uma turma publica uma nova atividade (trabalho, pesquisa, apresentação...), todos os alunos da turma precisam ser notificados.
Essa notificação é feita de duas formas:
    - Dentro do próprio sistema, via notificações internas vinculadas à cada aluno
    - Fora do sistema, enviando um email para cada aluno da turma através de um serviço externo (Brevo, Mailchimp, SendGrid...)
Ao final, quando todos os emails forem enviados, o sistema deve notificar internamente o professor, informando que a atividade foi publicada com sucesso.

Ficaria muito complicado fazer tudo isso na mesma requisição né? Sem contar que a api de envio de email pode retornar algum erro quando for chamada. Logo, seria interessante ter algum mecanismo de retry automático, que tentasse reenviar o email mais uma vez, por exemplo.

Agora vamos pensar em outro caso de uso, dessa vez mais relacionado com o fluxo de desenvolvimento: frequentemente preciso subir o sistema na minha máquina para testar as funcionalidades como um usuário final faria. Por exemplo, para poder publicar uma atividade como no caso acima, são necessários alguns passos antes:

- Cadastrar uma nova instituição de ensino + usuário acadêmico
- Logado como usuário acadêmico, preciso realizar o cadastro de:
    - Campus
    - Cursos
    - Disciplinas
    - Grades Curriculares
    - Alunos
    - Professores
    - Período Acadêmico
    - Turmas
    - Aulas
- Os alunos precisam logar no sistema e realizar sua matrícula nas turmas que foram abertas.
- Novamente como usuário acadêmico, precisa encerrar o período de matrícula e iniciar as turmas.

Somente ao final de tudo isso, posso logar como professor e publicar uma nova atividade para a turma.

Visando facilitar minha vida e trazer agilidade pro desenvolvimento, criei um único método para realizar esse seed de dados inicial, mas como no caso anterior, é muito código para ser executado de uma vez só. Seria mais interessante ter como dividir o seed em uma sequência de passos menores (worflow), onde cada um executasse ao final do outro, de maneira atômica.

Acompanhe abaixo como resolvi esses problemas e comente como você os resolveria também!

## Sumário
- 0️⃣ - Arquitetura do sistema
- 1️⃣ - Conceitos fundamentais
- 2️⃣ - Criação de nova atividade
- 3️⃣ - Seed de dados
- 4️⃣ - Visão do Adm



## 0️⃣ - Arquitetura do sistema

Os sistema é basicamente composto por 4 componentes:
- **Client**: frontend feito em Blazor WebAssembly
- **Back**: api feita em ASP.NET
- **Daemon**: worker para execução de tarefas em background, usando o Hangfire
- **Banco**: um PostgreSQL da vida

## 1️⃣ - Conceitos fundamentais

Acompanhe no diagrama abaixo todos os conceitos que fazem parte da solução final:

<p align="center">
  <img src="./clean_async_processing.gif" style="display: block; margin: 0 auto" />
</p>

- **Entidade**: uma classe do sistema capaz de emitir um evento de domínio.
    - Exemplo: *ClassActivity* (atividade dentro de uma turma)
- **Evento de Domínio**: representa que algo aconteceu no sistema.
    - Exemplo: *ClassActivityCreatedDomainEvent* (emitido quando uma nova atividade é criada pelo professor)
- **Comando**: representa um processamento assíncrono qualquer dentro do sistema.
    - Exemplo: *SendNewClassActivityEmailCommand* (comando que envia um email de nova atividade para determinado aluno da turma)
- **Lote**: um agrupamento lógico de comandos.
    - Exemplo: *SendNewClassActivityEmailCommandsBatch* (lote que agrupa todos os comandos *SendNewClassActivityEmailCommand* de uma atividade)
- **Workflow**: encadeamento lógico de comandos e/ou lotes.
    - Examplo: quando todos os comandos do lote *SendNewClassActivityEmailCommandsBatch* são executados com sucesso, um novo comando é enfileirado em sequência para notificar o professor que todos os alunos da turma receberam o email.

- **Event Listener**: componente do Daemon que é notificado toda vez que um novo evento de domínio é inserido no banco de dados.
    - Essa notificação é feita através de um trigger na tabela de eventos, que ao ser disparado chama uma função que utiliza a feature de LISTEN/NOTIFY do Postgres para informar o Daemon que um novo evento precisa ser processado.
- **Events Processor**: componente do Daemon que busca os eventos pendentes de processamento do banco de dados e os processa sequencialmente.
    - Sendo mais específico, cada Processor busca os 1000 eventos pendentes mais antigos do banco, processa todos em memória e os salva utilizando uma única transação.
- **Event Handler**: método que contém a lógica executada no processamento de um evento.
    - Normalmente é responsável por criar comandos ou lotes de comandos.

- **Command Listener**: componente do Daemon que é notificado toda vez que um novo comando é inserido no banco de dados.
    - Segue a mesma ideia do Event Listener.
- **Commands Processor**: componente do Daemon que busca os 100 comandos pendentes de processamento (priorizando os mais antigos) do banco de dados e os processa sequencialmente.
    - Cada comando é executado de maneira atômica, ou seja, dentro de uma transação exclusiva com o Postgres.
- **Command Handler**: método que contém a lógica executada no processamento de um comando.
    - Aqui podemos realizar praticamente qualquer ação, como envio de emails e seed de dados.

- **Batch Trigger**: existe um trigger específico para a gestão dos lotes de comandos, mas que não foi representado no diagrama.
    - Ele é responsável por atualizar o status do lote a cada comando processado, bem como liberar o processamento de comandos posteriores à sua conclusão com sucesso.

## 2️⃣ - Criação de nova atividade

Vamos alterar um pouco o diagrama anterior e usá-lo para entender como todo o fluxo de criação de nova atividade foi implementado. Agrupei os passos relacionados em cores específicas para facilitar o entendimento.

<p align="center">
  <img src="./steps_async_processing.gif" style="display: block; margin: 0 auto" />
</p>

- **(0)** - Professor preenche os dados da nova atividade no Client, que envia essas informações para a API no endpoint POST /activities
- **(1)** - API valida os dados, cria a nova atividade + evento de nova atividade criada e envia os dados para serem salvos no banco
- **(2)** - Banco retorna sucesso na inserção
- **(3)** - API retorna sucesso pro Client

[]()

- **(4)** - Após a inserção do novo evento, um trigger notifica o Event Listener que existe um novo evento para ser processado
- **(5)** - O Events Processor busca o evento pendente no banco
- **(6)** - O Event Handler cria um novo comando, que vai notificar os alunos da turma sobre a nova atividade
- **(7)** - O comando é salvo no banco de dados para ser processado em seguida

[]()

- **(8)** - Após a inserção do novo comando, um trigger notifica o Command Listener que existe um novo comando para ser processado
- **(9)** - O Commands Processor busca o comando pendente no banco
- **(10)** - O Command Handler cria as notificações internas pros alunos da turma + lote com os comandos para envio de emails + comando final que notifica o professor quando o lote é processado com sucesso
- **(11)** - Tudo que foi criado no passo anterior é então salvo no banco de dados

[]()

- **(A)** - À medida que cada comando do lote é processado, o Batch Trigger realiza a gestão do fluxo de vida do lote, alterando seu status com base no sucesso ou falha de cada um de seus comandos
- **(B)** - Quando todos os comandos do lote são executados com sucesso, o comando que notifica o professor é enfim liberado para execução, encerrando o workflow

Perceba que todo esse aparato de eventos, comandos e lotes pode ser utilizado em outros casos de uso, como por exemplo:

- Realizar chamadas de webhooks
- Publicar eventos para uma fila (RabbitMQ)
- Integrar com sistemas externos, como gateways de pagamento

## 3️⃣ - Seed de dados

O seed de dados foi dividido em uma sequência de passos menores, onde cada um executa ao final do outro de maneira atômica (worflow). Dessa forma, quando uma nova instituição é criada, emitimos um evento de domínio que enfilera o primeiro comando no seu handler. A partir daí, cada comando enfilera o próximo a ser executado, formando toda cadeia de processamento.

<p align="center">
  <img src="./event_commands.gif" height="500" style="display: block; margin: 0 auto" />
</p>

- **InstitutionCreatedDomainEvent**: Instituição Criada
- **SeedInstitutionBasicDataCommand**: Realizar seed de dados básicos da instituição (Campus, Cursos, Disciplinas e Grades Curriculares)
- **SeedInstitutionUsersCommand**: Realizar seed de usuários da instituição (Alunos e Professores)
- **SeedInstitutionClassesCommand**: Realizar seed de turmas da instituição (abertura das 6 primeiras turmas do curso de ADS)
- **SeedInstitutionEnrollmentsCommand**: Realizar seed de matrículas da instituição (todos os alunos do curso de ADS se matriculam nas turmas abertas no passo anterior)
- **SeedInstitutionLessonAttendancesCommand**: Realizar seed de chamadas da instituição (o professor de cada turma realiza uma chamada randômica em todas as aulas que já foram dadas, com base na data atual)

## 4️⃣ - Visão do Adm

Criei algumas telas para que o Adm do sistema possa acompanhar o processamento de todos os eventos, comandos e lotes.

- **Listagem de eventos**
    - Quantidade total, pendentes, processando, erros e sucessos
    - Dashboard com os últimos eventos + gráfico de pizza da quantidade de cada tipo de evento
    - Filtros por status, tipo, instituição e status dos comandos enfileirados pelo evento

- **Detalhes de um evento**
    - Quando ocorreu, quando foi processado e quantos milisegundos durou o processamento
    - Os dados do evento no formato json
    - A entidade que originou o evento de domínio
    - Listagem com os comandos enfileirados pelo evento (+ acesso aos detalhes de cada comando)

- **Listagem de comandos**
    - Quantidade total, pendentes, processando, erros e sucessos
    - Filtros por status, tipo e instituição
    - Acesso aos detalhes de cada comando

- **Detalhes de um comando**
    - Quando foi criado, quando foi processado e quantos milisegundos durou o processamento
    - Os dados do comando no formato json
    - Uma mensagem de erro, caso tenha dado algum no seu processamento
    - A origem do comando, podendo ser:
        - Um evento de domínio
        - Outro comando
        - Reprocessamento de um comando do mesmo tipo, que foi processado com erro
        - Finalização com sucesso de todos os comandos de um lote
    - O lote do comando, caso ele esteja contido em um
    - Listagem com os reprocessamentos do comando, caso ele tenha algum
    - Listagem com os subcomandos do comando, caso ele tenha algum
    - Listagem com os lotes criados pelo comando, caso exista algum

- **Listagem de lotes**
    - Quantidade total, pendentes, processando, erros e sucessos
    - Filtros por status, tipo e instituição
    - Acesso aos detalhes de cada lote

- **Detalhes de um lote**
    - Quando foi criado, quando foi processado e quantos comandos o lote contém
    - Próximo comando a ser executado, caso o lote possua um
    - A origem do lote, podendo ser:
        - Um evento de domínio
        - Um comando

Perceba que é possível navegar tanto no sentido cronológico de processamento dos componentes quanto no sentido contrário, chegando na origem de cada um.

## Pontos de melhoria

- Digamos que o envio do email deu errado (a api de envio estava fora do ar no momento do request):
    - O sistema poderia aguardar alguns segundos e tentar reprocessar o comando, certo?
    - Daria pra utilizar alguma lib para criar regras customizadas de retry para cada comando.
    - Como você faria isso?

- Com o passar do tempo, as tabelas de eventos e comandos devem ficar enormes, causando lentidão no processamento:
    - Podemos utilizar a feature de Table Partitioning do Postgres para mitigar isso.
    - Também podemos criar novas tabelas para armazenar apenas os eventos e comando já processados com sucesso, juntamente com uma rotina que move os dados entre as tabelas semanalmente, por exemplo.
    - Como você faria isso?
