# Implementação DevOps para o Projeto SYKI

## Visão Geral

Esta análise apresenta as decisões técnicas tomadas para a implementação da infraestrutura DevOps do projeto SYKI, detalhando a arquitetura escolhida, justificativas técnicas para cada componente e considerações sobre ferramentas adotadas ou descartadas.

## Arquitetura Implementada

### IaC

Terraform foi selecionado como a principal ferramenta de IaC pelos seguintes motivos:

- Declaratividade: Permite definir o estado desejado da infraestrutura, abstraindo a complexidade dos passos intermediários.

- Integração com múltiplos provedores: Embora atualmente o projeto utilize apenas DigitalOcean, a estrutura permanece extensível para múltiplos provedores.

- Modularidade: Organização em arquivos separados (main.tf, kubernetes.tf, variables.tf) para melhor manutenção.

### Plataforma de Cloud

DigitalOcean foi escolhida como provedora de cloud devido a:

- Simplicidade: Interface mais direta e menos complexa, além de crédito disponível de U$ 200.00.

- Serviços gerenciados adequados: DOKS (Kubernetes) com custo-benefício.

### Orquestração de Containers

Kubernetes (DOKS) foi implementado como plataforma de orquestração devido a:

- Portabilidade: Padronização da implantação independentemente do ambiente subjacente.

- Escalabilidade: Capacidade de ajustar recursos conforme necessário, mesmo começando com um único nó.

- Automação operacional: Autoreparação, rollouts/rollbacks automatizados e balanceamento de carga.

**Obs:** O cluster foi configurado com um único nó s-1vcpu-2gb para controle de custos no ambiente de desenvolvimento.

### Gerenciamento de Aplicações

Helm foi adotado como gerenciador de pacotes Kubernetes para:

- Repositórios compartilhados: Acesso a charts pré-configurados para Prometheus, Grafana e Loki, acelerando a implementação.

### Observabilidade

Um stack completo de observabilidade foi implementado:

- Prometheus: Coleta e armazenamento de métricas baseado em pull, com descoberta automática de serviços.

- Grafana: Visualização de métricas e logs através de dashboards customizáveis.

- Loki: Agregação e consulta de logs, complementando as métricas para troubleshooting.

Esta tríade foi escolhida por:

    Integração nativa: Funcionamento harmonioso como um ecossistema coeso.

    Padrão da indústria: Ampla adoção e comunidade ativa.

    Compatibilidade com Kubernetes: Instrumentação automatizada de componentes do cluster.

## Ferramentas Consideradas e Descartadas

### Ansible

Decisão: Excluído da implementação final.

Justificativa técnica:

- Paradigma de configuração: O modelo baseado em estado do Terraform e a abordagem declarativa do Kubernetes tornaram desnecessária a configuração imperativa do Ansible, já que não são utilizadas VMs.

### CI/CD

Decisão: Não implementado na fase atual, mas estruturado para adição futura.

Justificativa técnica:

- Priorização: Foco inicial na infraestrutura estável antes da automação de deployment.

- Flexibilidade: A arquitetura Kubernetes permite fácil integração posterior com qualquer pipeline CI/CD

A infraestrutura está preparada para integração com CI/CD quando o projeto avançar para essa fase de maturidade.
Considerações de Segurança

Foram implementadas práticas de segurança básicas:

- Namespaces separados: Segregação entre aplicação e monitoramento.

- Secrets protegidos: Senhas do Postgres e Grafana tratadas como informações sensíveis.

- Recursos limitados: Configuração de limites de recursos para prevenir sobrecarga do cluster.

## Conclusão e Próximos Passos

A implementação atual representa uma base sólida de infraestrutura DevOps para o projeto SYKI, com ênfase em:

- Automação: Toda a infraestrutura pode ser recriada com um único comando terraform apply.

- Observabilidade: Stack completo de monitoramento e logging.

- Portabilidade: Arquitetura baseada em padrões que pode ser replicada em outros ambientes.

Recomendações para evolução:

- Implementação de CI/CD: Automação de deployment, possivelmente com GitHub Actions.

- Security scanning: Adição de análise de vulnerabilidades em containers e código.
