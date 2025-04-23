# SYKI - Sistema de Gerenciamento Acadêmico

[![Tests](https://github.com/ZaqueuCavalcante/syki/actions/workflows/railway.yml/badge.svg)](https://github.com/ZaqueuCavalcante/syki/actions/workflows/railway.yml)

Syki um sistema open-source para gerenciamento educacional, que pode ser usado por gestores, professores e alunos.

Se cadastre em https://app.syki.com.br e teste o sistema em produção!

<img src="./Docs/images/syki_overview.gif" style="border-radius: 6px">

## Sumário

- Funcionalidades



- Tecnologias



- Testes
    - Unit
    - Integration
    - Mutation
    - Report Coverage
- CI/CD
    - GitHub Actions
    - Deploy Automático
- Infra
    - Railway
    - Terraform
    - Azure
    - Docker
- Segurança
    - Autenticação
    - Autorização
    - MFA
- Usabilidade
    - Light / Dark Modes
    - Responsividade no mobile
    - App Android / IOS
- Processamento Assíncrono
    - Eventos
    - Comandos
    - Lotes
    - Workflows
    - Falhas e Retentativas
- Observabilidade
    - Logs
    - Metrics
    - Traces
    - OpenTelemetry
    - DataDog
- Auditoria
    - Quem fez o quê quando e onde
- Configuração
    - Feature Flags
    - Secrets
- Rate Limiting
    - Mover pro NGNIX
- Real Time
    - Notificações
    - Usuários Ativos
- Documentação
    - Código
    - API (Scalar)
- Cache
    - Distributed Cache
    - Redis
- Arquitetura
    - Vertical Slices
    - Result Pattern
- Banco de Dados
    - Estrutura
    - Tabelas
    - Relacionamentos
    - Indexes
    - Diagrama
- Desenvolvimento
    - Rodando em localhost
    - Rodando os testes
- Contribuições
    - Código de Conduta
    - Abrir issues
    - PRs
    - Cobertura de Código
    - Doações

## 1 - Funcionalidades

O Syki possui diversas funcionalidades, que podem ser categorizadas com base em cada perfil de usuário do sistema:

- Acadêmico: usuário em cargo de gestão dentro da instituição de ensino
- Professor: usuário que leciona nas turmas da instituição
- Aluno: usuário matriculado em algum curso da instituição
- Adm: usuário administrador do sistema globalmente

Existem também as funcionalidades "Cross", que podem ser acessadas por qualquer usuário, como as de "Login" e "Esqueci Minha Senha".

### 1.1 - Cross

#### Cadastro de Usuário
#### Esqueci Minha Senha
#### Login Padrão
#### Setup MFA
#### Login MFA
#### Marcar Notificações como Lidas



### 1.2 - Acadêmico

#### Insights


#### Cadastro de Campus

É possível criar um novo campus ou editar um já existente.

#### Cadastro de Cursos
#### Cadastro de Disciplinas
#### Cadastro de Grades Curriculares
#### Ofertar Curso
#### Cadastro de Professores
#### Abertura de Turmas
#### Cadastro de Alunos
#### Abertura de Períodos Acadêmicos
#### Abertura de Períodos de Matrícula
#### Envio de Notificações



### 1.3 - Professor

#### Insights
#### Agenda Acadêmica
#### Atividades
#### Frequências



### 1.4 - Aluno

#### Insights
#### Agenda Acadêmica
#### Atividades
#### Frequências



### 1.5 - Adm

#### Insights
#### Usuários
#### Instituições
#### Feature Flags
#### Eventos
#### Comandos
#### Lotes







## Tecnologias

- Backend em C#/.NET
- Frontend em C#/Blazor Webassembly
- Daemon em C#/.NET
- Banco PostgreSQL
- Docker para buildar back e front
- Deploy no Railway
- Azure
- GitHub Actions
- Seq
- DataDog

<img src="./Docs/images/docker-compose.png" width="600" height="300" style="border-radius: 6px">



## Como rodar?

- Clone o projeto pra sua máquina
- Para subir banco + back + daemon + front, rode o comando: `docker-compose up`
- Para rebuildar caso tenha alguma alteração no código: `docker-compose build --no-cache`
- O back vai subir na porta 5001, o front na 5002 e o daemon na 5003
