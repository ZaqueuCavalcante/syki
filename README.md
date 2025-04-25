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

- Cadastrar-se no sistema informando seu email.
- Definir senha utilizando o link de confirmação de email.

#### Esqueci Minha Senha

- Redefinir sua senha utilizando o link de redefinição de senha.

#### Login Padrão

- Logar no sistema informando email+senha.

#### Setup MFA

- Utilizar um app de autenticação (ex: Google Authenticator) para configurar o segundo fator de autenticação.

#### Login MFA

- Logar no sistema apenas ao informar corretamente o TOTP gerado no app (Time-Based One-Time Password).

#### Receber Notificações

- Listar notificações relacionadas com sua instituição, curso ou atividades das turmas.
- Marcar notificações já visualizadas como lidas.

### 1.2 - Acadêmico

#### Insights

- Acessar diversos dados consolidados sobre a instituição, atualizados em tempo real.

#### Campus

- Criar um novo campus.
- Editar um campus já existente.
- Listar todos os campus da instituição.

#### Cadastro de Cursos

- Criar um novo curso.
- Listar todos os cursos da instituição.

#### Cadastro de Disciplinas

- Criar uma nova disciplina.
- Listar todas as disciplinas da instituição.

#### Cadastro de Grades Curriculares

- Criar uma nova grade curricular, vinculando curso e disciplinas.
- Listar todas as grades curriculares da instituição.

#### Ofertar Curso

- Ofertar um curso, vinculando campus, período acadêmico, turno, curso e grade curricular.
- Listar todas as ofertas de curso da instituição.

#### Cadastro de Professores

- Criar um novo professor.
- Listar todos os professores da instituição.

#### Abertura de Turmas

- Abrir um nova turma, vinculando disciplina, professor, período acadêmico e horários.
- Liberar uma turma para matrícula, dentro do período de matrícula vigente.
- Iniciar uma turma após validar todas as matrículas.
- Finalizar uma turma ao término do período acadêmico.
- Listar todas as turmas da instituição.

#### Acompanhamento de Turma

- Analisar as notas dos alunos nas atividades, bem como suas frequências em cada aula.

#### Cadastro de Alunos

- Criar um novo aluno, vinculando com determinada oferta de curso.
- Listar todos os alunos da instituição.

#### Abertura de Períodos Acadêmicos

- Abrir um novo período acadêmico.
- Listar todos os períodos acadêmicos da instituição.

#### Abertura de Períodos de Matrícula

- Abrir um novo período de matrícula.
- Editar as datas de início e fim de um período de matrícula.
- Listar todos os períodos de matrícula da instituição.

#### Envio de Notificações

- Enviar notificações para alunos e professores.
- Listar todas as notificações da instituição.

### 1.3 - Professor

#### Insights

- Acessar diversos dados consolidados sobre suas turmas, atualizados em tempo real.

#### Agenda Acadêmica

- Acompanhar qual o horário semanal das suas aulas.

#### Turmas

- Acessar facilmente todas as turmas do período atual.

#### Atividades

- Criar novas atividades para a turma.
- Listar todas as atividades da turma.
- Avaliar com pontuação cada entrega de atividade feita pelos alunos.

#### Frequências

- Realizar chamadas em cada aula.
- Listar todas as aulas e suas frequências.

#### Alunos

- Acompanhar notas e frequências dos alunos da turma.

### 1.4 - Aluno

#### Insights

- Acessar diversos dados consolidados sobre seu curso, atualizados em tempo real.

#### Agenda Acadêmica

- Acompanhar qual o horário semanal das suas aulas.

#### Disciplinas

- Visualizar facilmente todas as disciplinas do seu curso, agrupadas por período acadêmico.

#### Notas

- Acompanhar sua nota média no curso, bem como sua nota em cada disciplina.

#### Frequência

- Ver sua frequência total no curso, bem como em cada turma.

#### Turmas

- Acessar facilmente todas as turmas do período atual.

#### Atividades

- Listar todas as atividades da turma, bem como seu desempenho em cada uma.
- Entrar nos detalhes da atividade e realizar sua entrega via link em anexo.

#### Frequências

- Acompanhar quantidade de presenças, faltas, aulas realizadas e pendentes.
- Visualizar gráfico com sua frequência em todas as aulas realizadas até o momento.

### 1.5 - Adm

#### Insights

- Ver dados consolidados sobre todo o sistema, atualizados em tempo real.

#### Usuários

- Acompanhar todos os usuários do sistema.
- Filtrar quais estão online no momento atual.

#### Instituições

- Listar todas as instituições de ensino cadastradas no sistema.

#### Feature Flags

- Ativar/desativar features flags.

#### Eventos

- Dashboard com eventos pendentes, processando, erros e sucessos.
- Últimos eventos ocorridos e distribuição por quantidade.
- Listagem e filtros de todos os eventos gerados por todas as instituições.

#### Detalhes de um Evento

- Dados gerais e origem do evento.
- Lista dos comandos gerados pelo evento em questão.

#### Comandos

- Dashboard com comandos pendentes, processando, erros e sucessos.
- Listagem e filtros de todos os comandos gerados por todas as instituições.

#### Detalhes de um Comando

- Dados gerais e origem do comando.
- Lista do subcomandos gerados pelo comando em questão.
- Reprocessamento de comando com erro.
- Vinculo do comando caso ele pertença à um lote.

#### Lotes

- Dashboard com lotes pendentes, processando, erros e sucessos.
- Listagem e filtros de todos os lotes gerados por todas as instituições.

#### Detalhes de um Lote

- Dados gerais e origem do lote.
- Próximo comando a ser executado caso o lote termine com sucesso.












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
