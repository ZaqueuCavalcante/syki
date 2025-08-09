# SYKI - Sistema de Gerenciamento Acadêmico

[![Tests](https://github.com/ZaqueuCavalcante/syki/actions/workflows/railway.yml/badge.svg)](https://github.com/ZaqueuCavalcante/syki/actions/workflows/railway.yml)

[![Coverage Report](https://img.shields.io/badge/Coverage-Report-blue)](https://ZaqueuCavalcante.github.io/syki)

![Line Coverage](https://ZaqueuCavalcante.github.io/syki/line-coverage.svg)

![Branch Coverage](https://ZaqueuCavalcante.github.io/syki/branch-coverage.svg)

**Syki** é um sistema open-source para gerenciamento educacional, que pode ser usado por gestores, professores e alunos.

Se cadastre em https://app.syki.com.br e teste o sistema em produção!

<img src="./Docs/Readme/0_Overview.gif" style="border-radius: 6px">

## Sumário

1. [Funcionalidades](#features)
2. [Tecnologias](#tech)
3. [Testes](#tests)
4. [CI/CD](#ci-cd)
5. [Infra](#infra)
6. [Usabilidade](#usability)
7. [Processamento Assíncrono](#async-processing)
8. [Auditoria](#audit)
9. [Observabilidade](#observability)
10. [Rate Limiting](#rate-limiting)
11. [Documentação](#docs)
12. [Real-Time](#real-time)
13. [Cache](#cache)
14. [Arquitetura](#arch)
15. [Banco de Dados](#db)
16. [Webhooks](#webhooks)
17. [Desenvolvimento](#dev)
18. [Contribuições](#contributions)

## 1 - Funcionalidades <a name="features"></a>

O **Syki** possui diversas funcionalidades, que podem ser categorizadas com base em cada perfil de usuário do sistema:

- **Acadêmico**: usuário em cargo de gestão dentro da instituição de ensino
- **Professor**: usuário que leciona nas turmas da instituição
- **Aluno**: usuário matriculado em algum curso da instituição
- **Adm**: usuário administrador do sistema globalmente

Existem também as funcionalidades "**Cross**", que podem ser acessadas por qualquer usuário, como as de "Login" e "Esqueci Minha Senha".

### 1.1 - Cross

<p align="center">
  <img src="./Docs/Readme/1.1_Cross.png" style="display: block; margin: 0 auto" />
</p>

#### 1.1.1 - Cadastro de Usuário

- Cadastrar-se no sistema informando seu email.
- Definir senha utilizando o link de confirmação de email.

<p align="center">
  <img src="./Docs/Readme/1.1.1_UserRegister.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.1.2 - Esqueci Minha Senha

- Redefinir sua senha utilizando o link de redefinição de senha.

<p align="center">
  <img src="./Docs/Readme/1.1.2_ResetPassword.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.1.3 - Login Padrão

- Logar no sistema informando email e senha.

<p align="center">
  <img src="./Docs/Readme/1.1.3_Login.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.1.4 - Setup MFA

- Utilizar um app de autenticação (ex: Google Authenticator) para configurar o segundo fator de autenticação.

<p align="center">
  <img src="./Docs/Readme/1.1.4_MfaSetup.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.1.5 - Login MFA

- Logar no sistema apenas ao informar corretamente o TOTP gerado no app (Time-Based One-Time Password).

<p align="center">
  <img src="./Docs/Readme/1.1.5_LoginMfa.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.1.6 - Receber Notificações

- Listar notificações relacionadas com sua instituição, curso ou atividades das turmas.
- Marcar notificações já visualizadas como lidas.

<p align="center">
  <img src="./Docs/Readme/1.1.6_Notifications.gif" style="display: block; margin: 0 auto" />
</p>

### 1.2 - Acadêmico

#### 1.2.1 - Insights

- Acessar diversos dados consolidados sobre a instituição, atualizados em tempo real.

<p align="center">
  <img src="./Docs/Readme/1.2.1_Insights.png" style="display: block; margin: 0 auto" />
</p>

#### 1.2.2 - Campus

- Criar um novo campus.
- Editar um campus já existente.
- Listar todos os campus da instituição.

<p align="center">
  <img src="./Docs/Readme/1.2.2_Campus.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.2.3 - Cadastro de Cursos

- Criar um novo curso.
- Listar todos os cursos da instituição.

<p align="center">
  <img src="./Docs/Readme/1.2.3_Courses.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.2.4 - Cadastro de Disciplinas

- Criar uma nova disciplina.
- Listar todas as disciplinas da instituição.

<p align="center">
  <img src="./Docs/Readme/1.2.4_Disciplines.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.2.5 - Cadastro de Grades Curriculares

- Criar uma nova grade curricular, vinculando curso e disciplinas.
- Listar todas as grades curriculares da instituição.

<p align="center">
  <img src="./Docs/Readme/1.2.5_CourseCurriculum.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.2.6 - Ofertar Curso

- Ofertar um curso, vinculando campus, período acadêmico, turno, curso e grade curricular.
- Listar todas as ofertas de curso da instituição.

<p align="center">
  <img src="./Docs/Readme/1.2.6_CourseOffering.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.2.7 - Cadastro de Professores

- Criar um novo professor.
- Listar todos os professores da instituição.

<p align="center">
  <img src="./Docs/Readme/1.2.7_Teachers.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.2.8 - Gerenciamento de Turmas

- Abrir um nova turma, vinculando disciplina, professor, período acadêmico e horários.
- Liberar uma turma para matrícula, dentro do período de matrícula vigente.
- Iniciar uma turma após validar todas as matrículas.
- Finalizar uma turma ao término do período acadêmico.
- Listar todas as turmas da instituição.

<p align="center">
  <img src="./Docs/Readme/1.2.8_Classes.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.2.9 - Acompanhamento de Turma

- Analisar as notas dos alunos nas atividades, bem como suas frequências em cada aula.

<p align="center">
  <img src="./Docs/Readme/1.2.9_Class.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.2.10 - Cadastro de Alunos

- Criar um novo aluno, vinculando com determinada oferta de curso.
- Listar todos os alunos da instituição.

<p align="center">
  <img src="./Docs/Readme/1.2.10_Students.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.2.11 - Abertura de Períodos Acadêmicos

- Abrir um novo período acadêmico.
- Listar todos os períodos acadêmicos da instituição.

<p align="center">
  <img src="./Docs/Readme/1.2.11_AcademicPeriods.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.2.12 - Abertura de Períodos de Matrícula

- Abrir um novo período de matrícula.
- Editar as datas de início e fim de um período de matrícula.
- Listar todos os períodos de matrícula da instituição.

<p align="center">
  <img src="./Docs/Readme/1.2.12_EnrollmentPeriods.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.2.13 - Envio de Notificações

- Enviar notificações para alunos e professores.
- Listar todas as notificações da instituição.

<p align="center">
  <img src="./Docs/Readme/1.2.13_Notifications.gif" style="display: block; margin: 0 auto" />
</p>

### 1.3 - Professor

#### 1.3.1 - Insights

- Acessar diversos dados consolidados sobre suas turmas, atualizados em tempo real.

<p align="center">
  <img src="./Docs/Readme/1.3.1_Insights.png" style="display: block; margin: 0 auto" />
</p>

#### 1.3.2 - Agenda Acadêmica

- Acompanhar qual o horário semanal das suas aulas.

<p align="center">
  <img src="./Docs/Readme/1.3.2_Agenda.png" style="display: block; margin: 0 auto" />
</p>

#### 1.3.3 - Turmas

- Acessar facilmente todas as turmas do período atual.

<p align="center">
  <img src="./Docs/Readme/1.3.3_Classes.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.3.4 - Atividades

- Criar novas atividades para a turma.
- Listar todas as atividades da turma.
- Avaliar com pontuação cada entrega de atividade feita pelos alunos.

<p align="center">
  <img src="./Docs/Readme/1.3.4_Activities.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.3.5 - Frequências

- Realizar chamadas em cada aula.
- Listar todas as aulas e suas frequências.

<p align="center">
  <img src="./Docs/Readme/1.3.5_Frequencies.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.3.6 - Alunos

- Acompanhar notas e frequências dos alunos da turma.

<p align="center">
  <img src="./Docs/Readme/1.3.6_Students.gif" style="display: block; margin: 0 auto" />
</p>

### 1.4 - Aluno

#### 1.4.1 - Insights

- Acessar diversos dados consolidados sobre seu curso, atualizados em tempo real.

<p align="center">
  <img src="./Docs/Readme/1.4.1_Insights.png" style="display: block; margin: 0 auto" />
</p>

#### 1.4.2 - Agenda Acadêmica

- Acompanhar qual o horário semanal das suas aulas.

<p align="center">
  <img src="./Docs/Readme/1.4.2_Agenda.png" style="display: block; margin: 0 auto" />
</p>

#### 1.4.3 - Disciplinas

- Visualizar facilmente todas as disciplinas do seu curso, agrupadas por período acadêmico.

<p align="center">
  <img src="./Docs/Readme/1.4.3_Disciplines.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.4.4 - Notas

- Acompanhar sua nota média no curso, bem como sua nota em cada disciplina.

<p align="center">
  <img src="./Docs/Readme/1.4.4_Notes.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.4.5 - Frequência

- Ver sua frequência total no curso, bem como em cada turma.

<p align="center">
  <img src="./Docs/Readme/1.4.5_Frequency.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.4.6 - Turmas

- Acessar facilmente todas as turmas do período atual.

<p align="center">
  <img src="./Docs/Readme/1.4.6_Classes.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.4.7 - Atividades

- Listar todas as atividades da turma, bem como seu desempenho em cada uma.
- Entrar nos detalhes da atividade e realizar sua entrega via link em anexo.

<p align="center">
  <img src="./Docs/Readme/1.4.7_Activities.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.4.8 - Frequências

- Acompanhar quantidade de presenças, faltas, aulas realizadas e pendentes.
- Visualizar gráfico com sua frequência em todas as aulas realizadas até o momento.

<p align="center">
  <img src="./Docs/Readme/1.4.8_Frequencies.gif" style="display: block; margin: 0 auto" />
</p>

### 1.5 - Adm

#### 1.5.1 - Insights

- Ver dados consolidados sobre todo o sistema, atualizados em tempo real.

<p align="center">
  <img src="./Docs/Readme/1.5.1_Insights.png" style="display: block; margin: 0 auto" />
</p>

#### 1.5.2 - Usuários

- Acompanhar todos os usuários do sistema.
- Filtrar quais estão online no momento atual.

<p align="center">
  <img src="./Docs/Readme/1.5.2_Users.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.5.3 - Instituições

- Listar todas as instituições de ensino cadastradas no sistema.

<p align="center">
  <img src="./Docs/Readme/1.5.3_Institutions.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.5.4 - Feature Flags

- Ativar/desativar features flags.

<p align="center">
  <img src="./Docs/Readme/1.5.4_FeatureFlags.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.5.5 - Eventos

- Dashboard com eventos pendentes, processando, erros e sucessos.
- Últimos eventos ocorridos e distribuição por quantidade.
- Listagem e filtros de todos os eventos gerados por todas as instituições.

<p align="center">
  <img src="./Docs/Readme/1.5.5_Events.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.5.6 - Detalhes de um Evento

- Dados gerais e origem do evento.
- Lista dos comandos gerados pelo evento em questão.

<p align="center">
  <img src="./Docs/Readme/1.5.6_EventDetails.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.5.7 - Comandos

- Dashboard com comandos pendentes, processando, erros e sucessos.
- Listagem e filtros de todos os comandos gerados por todas as instituições.

<p align="center">
  <img src="./Docs/Readme/1.5.7_Commands.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.5.8 - Detalhes de um Comando

- Dados gerais e origem do comando.
- Lista do subcomandos gerados pelo comando em questão.
- Reprocessamento de comando com erro.
- Vinculo do comando caso ele pertença à um lote.

<p align="center">
  <img src="./Docs/Readme/1.5.8_CommandDetails.gif" style="display: block; margin: 0 auto" />
</p>

#### 1.5.9 - Lotes

- Dashboard com lotes pendentes, processando, erros e sucessos.
- Listagem e filtros de todos os lotes gerados por todas as instituições.

<p align="center">
  <img src="./Docs/Readme/1.5.9_Batches.png" style="display: block; margin: 0 auto" />
</p>

#### 1.5.10 - Detalhes de um Lote

- Dados gerais e origem do lote.
- Próximo comando a ser executado caso o lote termine com sucesso.

<p align="center">
  <img src="./Docs/Readme/1.5.10_BatchDetails.gif" style="display: block; margin: 0 auto" />
</p>

## 2 - Tecnologias <a name="tech"></a>

A stack predominante é a da Microsoft, utilizo C# tanto no backend quanto no frontend:

- API em ASP.NET
- Front em Blazor Webassembly
- Daemon em ASP.NET
- Banco PostgreSQL
- Build com Docker
- Deploy no Railway
- GitHub Actions
- Seq para logs

<p align="center">
  <img src="./Docs/Readme/2_TechStack.png" style="display: block; margin: 0 auto" />
</p>

## 3 - Testes <a name="tests"></a>

O projeto possui mais de 500 testes automatizados, divididos entre Unidade e Integração.

Os testes de unidade validam regras de negócio apenas utilizando as entidades em memória, sem dependências com o banco de dados.

Já os testes de integração são mais robustos, pois validam que os fluxos de negócio envolvendo Back + Daemon + Postgres estão funcionando corretamente.

<p align="center">
  <img src="./Docs/Readme/3_Tests.gif" style="display: block; margin: 0 auto" />
</p>

## 4 - CI/CD <a name="ci-cd"></a>

O Syki conta com um pipeline de CI/CD, que roda no GitHub Actions toda vez que um novo commit é feito na branch master.

Esse pipeline é responsável por buildar toda a solução, rodar os testes automatizados e executar eventuais migrações contra o banco de dados.

<p align="center">
  <img src="./Docs/Readme/4_Pipeline.png" style="display: block; margin: 0 auto" />
</p>

Quando o pipeline executa com sucesso, uma integração com a plataforma Railway é disparada para que o deploy de todos os serviços seja realizado. O Railway utiliza os Dockerfiles de cada componente para gerar as imagens e subir os contâiners a partir delas. Todas as configurações necessárias para que as aplicações rodem são armazenadas como variáveis de ambiente direto no Railway.

<p align="center">
  <img src="./Docs/Readme/4_Railway.png" style="display: block; margin: 0 auto" />
</p>

## 5 - Infra <a name="infra"></a>

Atualmente todos os serviços estão rodando no [Railway](https://railway.com?referralCode=zk):

<p align="center">
  <img src="./Docs/Readme/5_Infra.png" style="display: block; margin: 0 auto" />
</p>

Pretendo migrar toda a infra pra Azure logo logo. Vou utilizar o Terraform para provisionar os recursos na nuvem de maneira organizada e replicável para os ambientes de Staging e Produção.

## 6 - Usabilidade <a name="usability"></a>

O frontend do projeto conta com modos claro e escuro.

<p align="center">
  <img src="./Docs/Readme/6_DarkLightModes.gif" style="display: block; margin: 0 auto" />
</p>

Todas as telas são responsivas, funcionando bem tanto no desktop quanto no mobile.

Pretendo criar um app usando Flutter (Android/IOS) para que o aluno possa acessar o Syki.

<p align="center">
  <img src="./Docs/Readme/6_Mobile.gif" style="display: block; margin: 0 auto" />
</p>

## 7 - Processamento Assíncrono <a name="async-processing"></a>

O Syki possui diversos fluxos de negócio naturalmente assíncronos, como o envio de notificações pros usuários via email.

Ele possui um sistema robusto de eventos, comandos e lotes para a realização dessas tarefas.

Ainda é possível acompanhar em tempo real todas as execuções do sistema, possibilitando a rápida detecção de problemas e o reprocessamento de comandos com erro.

No futuro ele vai contar também com reprocessamento automático de falhas.

<p align="center">
  <img src="./Docs/Readme/7_AsyncProcessing.gif" style="display: block; margin: 0 auto" />
</p>

## 8 - Auditoria <a name="audit"></a>

Muitas das ações dos usuários no sistema possuem auditoria, ou seja, são salvas no banco de dados para que seja possível saber quem, fez o quê, quando e onde dentro da aplicação.

## 9 - Observabilidade <a name="observability"></a>

O padrão OpenTelemetry é utilizado nas aplicações para a coleta de logs, métricas e traces.

No futuro todos esses dados serão enviados para o DataDog.

## 10 - Rate Limiting <a name="rate-limiting"></a>

O backend utiliza o middleware de rate limiting nativo do próprio ASP.NET para bloquear o uso abusivo da API.

## 11 - Documentação <a name="docs"></a>

Grande parte do código possui documentação via XML, que é utilizada tanto pela IDE quanto pelo Swagger para gerar o documento de especificação OpenAPI.

Utilizo o Scalar para ler esse documento e gerar a documentação completa da API.

<p align="center">
  <img src="./Docs/Readme/11_Scalar.gif" style="display: block; margin: 0 auto" />
</p>

## 12 - Real-Time <a name="real-time"></a>

A biblioteca SignalR foi utilizada para trazer funcionalidades em tempo real para o sistema.

Hoje é possível ser notificado em tempo real quando uma nova atividade é postada, por exemplo.

O Adm também consegue saber quais usuários estão ativos no momento, além ter acesso a quantas conexões com o servidor cada um está estabelecendo.

## 13 - Cache <a name="cache"></a>

Utilizo a biblioteca Hybrid Cache para cachear alguns dados básicos da instituição, como seus cursos e grades curriculares.

No futuro pode ser necessária a adoção de cache distribuído, usando o Redis por exemplo.

## 14 - Arquitetura <a name="arch"></a>

Todo o desenvolvimento é orientado por simplicidade.

Utilizo *Vertical Slices* e *Result Pattern* em praticamente todas as funcionalidades do sistema.

## 15 - Banco de Dados <a name="db"></a>

Toda a estrutura do banco está mepeada no Entity Framework Core.

Acompanhe a seguir toda a evolução das tabelas:

<p align="center">
  <img src="./Docs/Readme/15_DatabaseEvolution.gif" style="display: block; margin: 0 auto" />
</p>

E agora o estado atual do banco (omiti alguns relacionamentos para não poluir o diagrama):

<p align="center">
  <img src="./Docs/Readme/15_DatabaseTables.gif" style="display: block; margin: 0 auto" />
</p>


## 16 - Webhooks <a name="webhooks"></a>

O sistema possui suporte à Webhooks, permitindo a realização de integrações mais rápidas e eficientes.

Ele é capaz de emitir eventos quando certas ações são executadas pelos usuários, como por exemplo:
  - Um novo aluno é cadastrado no sistema
  - Uma nova atividade é publicada pelo professor de uma turma

Digamos que seja preciso integrar o Syki à outro serviço XYZ, que vai executar um determinado processamento toda vez que um desses eventos ocorrer.

Talvez a maneira mais simples de realizar essa integração seja através de pooling: a aplicação XYZ fica, periodicamente, chamando a Api do Syki para buscar novos eventos. Isso é simples de implementar, mas também é custoso e ineficiente, pois a maioria das chamadas não vai encontrar dados novos para serem processados, sobrecarregando a Api do Syki desnecessariamente.

Um outro jeito de abordar esse problema é através do uso de 𝘄𝗲𝗯𝗵𝗼𝗼𝗸𝘀: o serviço XYZ cadastra uma url (+ ApiKey) no Syki e escolhe quais eventos quer receber através dela. Dessa forma, toda vez que um dos eventos escolhidos ocorrer, o Syki monta um payload e chama a aplicação XYZ com os dados, em uma integração rápida e eficiente.

<p align="center">
  <img src="./Docs/Readme/16.1_WebhookSubscription.gif" style="display: block; margin: 0 auto" />
</p>

O GIF abaixo mostra essa integração acontecendo quando um novo aluno é cadastrado no sistema.

<p align="center">
  <img src="./Docs/Readme/16.2_WebhookCall.gif" style="display: block; margin: 0 auto" />
</p>

Obviamente essa chamada para o endpoint na aplicação XYZ pode falhar, por isso implementei também uma política de retry exponencial: caso a primeira chamada falhe, o Syki vai tentar novamente após 1 min. Caso falhe, tenta novamente após 5 min. Caso falhe novamente, tenta pela última vez após 30 min.

Ainda é possível reprocessar uma chamada manualmente via tela, para o caso onde todas as retentativas automáticas falharam ou mesmo em caso de reconciliação de dados, por exemplo.

<p align="center">
  <img src="./Docs/Readme/16.3_WebhookRetry.gif" style="display: block; margin: 0 auto" />
</p>

## 17 - Desenvolvimento <a name="dev"></a>

Para rodar o sistema na sua máquina, siga os passos abaixo:
- Clone o projeto pra sua máquina
- Para subir banco + back + daemon + front, rode o comando: `docker-compose up`
- Para rebuildar caso tenha alguma alteração no código: `docker-compose build --no-cache`
- O back vai subir na porta 5001, o front na 5002 e o daemon na 5003

Para rodar os testes automatizados:

- All: `dotnet test --logger:"console;verbosity=detailed"`
- Unit: `dotnet test --filter "FullyQualifiedName~UnitTests"`
- Integration: `dotnet test --filter "FullyQualifiedName!~UnitTests"`

## 18 - Contribuições <a name="contributions"></a>

Qualquer contribuição é bem-vinda!

Fique à vontade para usar o sistema, abrir issues, enviar pull requests e tirar dúvidas.

---------------------------------------------------------------------------------------------------

> Computadores fazem arte | Artistas fazem dinheiro
