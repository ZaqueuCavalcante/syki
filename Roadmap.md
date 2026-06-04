# Roadmap

## Skills que o projeto demonstra bem

**Arquitetura de Software**
O mais forte do projeto.
Vertical slice, Result pattern, command queue distribuído, middleware pipeline, multi-tenant com tenant context no DbContext.
Quem sabe o que está olhando reconhece decisões não-óbvias aqui.

**Backend Engineering**
ASP.NET Core em profundidade: autenticação multi-scheme, RBAC com permissions granulares, rate limiting com particionamento user/IP, SSO dinâmico com OIDC.
Não é tutorial — é produto.

**API Design**
Consistência de contratos, documentação com exemplos reais de request/response/erro, versionamento implícito por feature.
Scalar UI com tudo documentado é um diferencial visual forte.

**Modelagem de Banco**
Multi-tenant com isolamento por InstitutionId, compound indexes estratégicos, audit trail, snake_case, Dapper + EF Core side-by-side.
Mostra que você pensa em query performance, não só em ORM.

**Testes**
O ponto mais raro de ver em portfólios. Integration tests com banco real, mutation testing com Stryker, 95%+ coverage, CI/CD com relatório publicado.
Isso impressiona muito porque quase ninguém faz.

**Processamento Assíncrono**
Command queue com retry/backoff, SKIP LOCKED para concorrência distribuída, OpenTelemetry por span.
Mostra que você entende sistemas além do request-response.

---

## Skills que aparecem mas não convencem ainda

**Infraestrutura**
Docker Compose existe, Railway e GitHub Actions também.
Mas não tem IaC (Terraform/Pulumi), sem autoscaling, sem ambiente de staging separado.
Mostra conhecimento básico de infra, não engenharia de plataforma.

**Caching**
HybridCache configurado, mas sem Redis real, sem invalidação explícita, sem cache em endpoints que claramente precisam.
Aparece no código mas não demonstra domínio do assunto.

**Observabilidade**
OpenTelemetry + Serilog + Sentry estão lá.
Mas sem dashboards visíveis (Grafana, Seq), sem alertas configurados, ninguém consegue ver o sistema funcionando.
Fica invisível no portfólio.

---

## Skills que o projeto não demonstra

**UI/UX e Design System**
O frontend existe com Nuxt UI, mas parece funcional, não refinado.
Não tem design system próprio, não tem componentes reutilizáveis bem documentados, não tem preocupação visível com acessibilidade ou responsividade.

**Engenharia de Produto**
O projeto tem features acadêmicas, mas não tem nenhuma evidência de decisão de produto: sem roadmap de produto, sem métricas de uso, sem feature flags, sem A/B testing.
Mostra que você sabe construir, não necessariamente o quê construir.

**Integração entre Sistemas**
Tem SSO (OIDC) que é uma integração real. Mas não tem integração com APIs externas de pagamento, comunicação (email/SMS real), ou ERP.
Os webhooks que mostrariam isso estão no Legacy.

---

## O que as empresas vão concluir

**Empresas de produto (startups, scale-ups)**
Vão ver um dev backend sólido com bom senso de arquitetura, que testa de verdade e pensa em segurança. Ponto forte.

**Consultorias e empresas enterprise**
Vão notar o SSO, RBAC, multi-tenancy e audit trail. São exatamente os requisitos de sistemas enterprise. Ponto forte.

**Empresas que valorizam frontend**
O projeto não vai te vender como fullstack convincente. O frontend existe mas não está no mesmo nível do backend.

**Empresas que olham infra/plataforma**
Vão sentir falta de IaC, observabilidade com dashboard, e estratégia de deploy além do básico.

---

## Próximos passos (ordem de impacto no portfólio)

1. **Blob Storage** — implementar `AzureBlobStorageService` e `S3StorageService`. Interface e pacote já existem.
2. **WebSockets/SSE** — progresso de CommandBatch em tempo real via SignalR Hub. Conecta ao sistema de commands existente.
3. **Webhooks** — mover de `/Legacy/` para o fluxo ativo, plugar no sistema de Commands.
4. **Redis como distributed cache** — configurar HybridCache com Redis como L2, invalidação explícita em writes.
5. **Data Structures + Algorithms** — prerequisite chain com topological sort, grade scheduler com conflict detection.
6. **Observabilidade visível** — subir Grafana/Seq no Docker Compose e documentar os dashboards.
