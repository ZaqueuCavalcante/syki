# TODOS

- Feature para enviar email pros alunos toda vez que for postada uma nova atividade


- Plano de aulas (conteudos)
- Melhorar design da paginação das tabelas


- HttpClientFactory
- DbConnectionFactory
- Pool de conexoes com o banco de dados
- RabbitMQ use case (project)
- Redis use case (project)



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






