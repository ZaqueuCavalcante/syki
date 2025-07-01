# 🔍 Tracing com OpenTelemetry + Jaeger

O **OpenTelemetry** é um projeto open-source que define uma abordagem consistente para instrumentação de aplicações em diferentes linguagens e frameworks. Uma aplicação instrumentada usando OpenTelemetry é capaz de exportar métricas, traces e logs de maneira fácil e intuitiva.

Um **trace** representa a jornada de um request através de vários componentes e serviços em um sistema distribuído. Ele ajuda a identificar gargalos de performance, latência e erros, trazendo uma visão unificada de como diferentes serviços interagem nos fluxos de negócio.

Já um **span** representa uma operação dentro de um trace. Exemplos: query feita no banco de dados ou uma chamada HTTP para uma API externa.

O **Jaeger** é uma ferramenta open-source para pesquisa, análise e visualização de traces/spans.

---

Na prática, como tudo isso funciona?

A imagem abaixo mostra o trace de um fluxo de negócio pertencente ao Syki (github.com/ZaqueuCavalcante/syki), um projeto para gerenciamento educacional que estou desenvolvendo.

Trata-se da criação de uma atividade pelo professor, decomposta em detalhes a seguir:

- 1️⃣ Chamada HTTP para a API (Back), criando a atividade:
    - POST teacher/classes/{id}/activities
    - Faz alguns selects no banco para validar dados
    - Insere a atividade + o evento de domínio ClassActivityCreatedDomainEvent

- 2️⃣ O Daemon é outra aplicação, responsável por processar os eventos:
    - Ele pega o evento pendente do banco e executa seu handler
    - Os comandos CreateNewClassActivityNotificationCommand e CreateClassActivityCreatedWebhookCallCommand são salvos como pendentes

- 3️⃣ O Daemon então processa os comandos:
    - CreateNewClassActivityNotificationCommand notifica todos os alunos sobre a nova atividade
    - CreateClassActivityCreatedWebhookCallCommand cria um WebhookCall e emite o evento WebhookCallCreatedDomainEvent

- 4️⃣ O Daemon novamente processa o evento pendente:
    - O handler do evento WebhookCallCreatedDomainEvent enfileira o comando CallWebhookCommand

- 5️⃣ E por fim, o comando CallWebhookCommand é executado, realizando a chamada HTTP POST spicy-alligator-90.webhook.cool com os dados da atividade criada.

O tracing nos mostra tudo que acontece nesse fluxo, desde o macro até o micro. Dá pra ver quais queries e requisições foram feitas, junto com quanto tempo cada operação levou.

Ainda é possível adicionar tags aos traces, como o id do evento/comando que está sendo processado.

---

Você já utiliza alguma ferramenta de tracing/observabilidade no seu dia a dia? Compartilhe nos comentários, valeu!

docker run -d -p 4317:4317 -p 16686:16686 jaegertracing/all-in-one:latest

http://localhost:16686
