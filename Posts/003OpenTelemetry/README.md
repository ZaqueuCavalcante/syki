# OpenTelemetry

- ILogger (logs)
- Activity (traces)
- Meter (metrics)


- OpenTelemetry
- Seq, Jaeger, Grafana, Sentry
- DataDog


Posts:
- LinkedIn short
- LinkedIn article
- TabNews article

Tools:
- Otel
- SigNoz
- Serilog

## 0 - Objetivo

Tenho um sistema com Back + Daemon + Front + Banco (+ integrações + app mobile?).
Quero monitorar tudo que acontece nele:
- Performance e comportamento
- Requests com erro (taxa de erros)
- Endpoints com baixa performance (código / banco)
- Hot paths
- Anomalias no uso do sistema
- Usos de recursos (CPU/Memoria)
- Requests/segundo
- Fluxo completo, desde o front -> back -> banco -> daemon (eventos + tasks) -> moodle

Configurar alertas para quando determinada coisa acontecer:
- Endpoints lentos
- Erros (400, 500) acontecendo com frequencia
- Container com alto consumo de CPU/memoria

Logs + metrics + traces + events

### O que é Observabilidade?

Observabilidade é a capacidade de compreender o estado interno de um sistema complexo, com base em dados e métricas externas.
Mais importante que mandar algo para produção é MANTER algo em produção!

- Monitorar e medir o comportamento do sistema
- Identificar problemas e oportunidades de melhoria
- Solucionar problemas de forma proativa e preventiva
- Mapear sistemas interconectados e suas interações
- Observar a integridade do sistema em tempo real



O que está acontecendo no sistema? A observabilidade permite que você acompanhe e registre informações sobre o desempenho e o comportamento do sistema em tempo real.

Por que algo deu errado? Quando ocorrem problemas ou falhas no sistema, a observabilidade fornece insights detalhados para que você possa identificar a causa raiz e solucionar o problema mais rapidamente.

Como o sistema está funcionando? Ela oferece métricas e indicadores de desempenho que ajudam a avaliar o estado do sistema e a tomar decisões informadas sobre otimizações e escalabilidade.


https://opentelemetry.io/docs/concepts/observability-primer/#what-is-observability

https://www.elastic.co/pt/what-is/observability



https://signoz.io/


Observabilidade

- OpenTelemetry + Jaeger
- Contar quantos requests foram feitos durante a execucao dos testes de integracao
- Endpoints + quantas chamadas tiveram + qual o retorno (sucesso vs erro)



