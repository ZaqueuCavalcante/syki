# SYKI - Sistema de Gerenciamento Acadêmico

[![Tests](https://github.com/ZaqueuCavalcante/syki/actions/workflows/railway.yml/badge.svg)](https://github.com/ZaqueuCavalcante/syki/actions/workflows/railway.yml)

**Syki** é um sistema open-source para gerenciamento educacional, que pode ser usado por gestores, professores e alunos.

Se cadastre em https://app.syki.com.br e teste o sistema em produção!

<img src="./Docs/images/syki_overview.gif" style="border-radius: 6px">

## Sumário

1. [Funcionalidades](#features)
2. [Tecnologias](#)
3. [Testes](#)
4. [CI/CD](#)
5. [Infra](#)
6. [Usabilidade](#)
7. [Processamento Assíncrono](#)
8. [Auditoria](#)
9. [Observabilidade](#)
10. [Rate Limiting](#)
11. [Documentação](#)
12. [Real Time](#)
13. [Cache](#)
14. [Arquitetura](#)
15. [Banco de Dados](#)
16. [Desenvolvimento](#)
17. [Contribuições](#contributions)

## 1 - Funcionalidades <a name="features"></a>

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












## 2 - Tecnologias

A stack predominante é a da Microsoft, utilizo C# tanto no backend quanto no frontend.

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

## 3 - Testes

O projeto possui mais de 500 testes automatizados, divididos entre Unidade e Integração.

Os testes de unidade validam regras de negócio apenas utilizando as entidades em memória, sem dependências com o banco de dados.

Já os testes de integração são mais robustos, pois validam que os fluxos de negócio envolvendo Back + Daemon + Postgres estão funcionando corretamente.

## 4 - CI/CD

O Syki conta com um pipeline de CI/CD, que roda no GitHub Actions toda vez que um novo commit é feito na branch master.

Esse pipeline é responsável por buildar toda a solução, rodar os testes automatizados e executar eventuais migrações contra o banco de dados.

Quando o pipeline executa com sucesso, uma integração com a plataforma Railway é disparada para que o deploy de todos os serviços seja realizado. O Railway utiliza os Dockerfiles de cada componente para gerar as imagens e subir os contâiners a partir delas. Todas as configurações necessárias para que as aplicações rodem são armazenadas como variáveis de ambiente direto no Railway.

## 5 - Infra

Pretendo migrar toda a infra pra Azure no mês de Maio.

Vou utilizar o Terraform para provisionar os recursos na nuvem de maneira organizada e replicável para os ambientes de Staging e Produção.

## 6 - Usabilidade

O frontend do projeto conta com modos claro e escuro.

Todas as telas são responsivas, funcionando bem tanto no desktop quanto no mobile.

Pretendo criar um app usando Flutter (Android/IOS) para que o aluno possa acessar o Syki.

## 7 - Processamento Assíncrono

O Syki possui diversos fluxos de negócio naturalmente assíncronos, como o envio de notificações pros usuários via email.

Ele possui um sistema robusto de eventos, comandos e lotes para a realização dessas tarefas.

Ainda é possível acompanhar em tempo real todas as execuções do sistema, possibilitando a rápida detecção de problemas e o reprocessamento de comandos com erro.

No futuro ele vai contar também com reprocessamento automático de falhas.

## 8 - Auditoria

Muitas das ações dos usuários no sistema possuem auditoria, ou seja, são salvas no banco de dados para que seja possível saber quem, fez o quê, quando e onde dentro da aplicação.

## 9 - Observabilidade

O padrão OpenTelemetry é utilizado nas aplicações para a coleta de logs, métricas e traces.

No futuro todos esses dados serão enviados para o DataDog.

## 10 - Rate Limiting

O backend utiliza o middleware de rate limiting nativo do próprio ASP.NET para bloquear o uso abusivo da API.

No futuro esse controle deve ser realizado por algum outro componente na Azure.

## 11 - Documentação

Grande parte do código possui documentação via XML, que é utilizada tanto pela IDE quanto pelo Swagger para gerar o documento de especificação OpenAPI.

Utilizo o Scalar para ler esse documento e gerar a documentação completa da API.

## 12 - Real Time

A biblioteca SignalR foi utilizada para trazer funcionalidades em tempo real para o sistema.

Hoje é possível ser notificado em tempo real quando uma nova atividade é postada, por exemplo.

O Adm também consegue saber quais usuários estão ativos no momento, além ter acesso a quantas conexões com o servidor cada um está estabelecendo.

## 13 - Cache

Utilizo a biblioteca Hybrid Cache para cachear alguns dados básicos da instituição, como seus cursos e grades curriculares.

No futuro pode ser necessária a adoção de cache distribuído, usando o Redis por exemplo.

## 14 - Arquitetura

Todo o desenvolvimento é orientado por simplicidade.

Utilizo *Vertical Slices* e *Result Pattern* em praticamente todas as funcionalidades do sistema.

## 15 - Banco de Dados

Toda a estrutura do banco está mepeada no EF Core.

Podemos ver toda sua evolução no GIF abaixo, indo de 12 tabelas no início do projeto até as 40 atuais.

## 16 - Desenvolvimento

Para rodar o sistema na sua máquina, siga os passos abaixo:
- Clone o projeto pra sua máquina
- Para subir banco + back + daemon + front, rode o comando: `docker-compose up`
- Para rebuildar caso tenha alguma alteração no código: `docker-compose build --no-cache`
- O back vai subir na porta 5001, o front na 5002 e o daemon na 5003

Para rodar os testes automatizados:

- All: `dotnet test --logger:"console;verbosity=detailed"`
- Unit: `dotnet test --filter "FullyQualifiedName~UnitTests"`
- Integration: `dotnet test --filter "FullyQualifiedName!~UnitTests"`

## 17 - Contribuições <a name="contributions"></a>

- Código de Conduta
- Abrir issues
- PRs
- Cobertura de Código
- Doações
