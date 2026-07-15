# Plano — Novo UserType: Parent (Responsável)

Mães, pais e responsáveis de alunos passam a ter acesso ao sistema para acompanhar a vida escolar dos filhos (notas, frequência, trabalhos, agenda) e complementar o cadastro deles com informações de saúde e contatos de emergência.

## Estado atual do sistema (base para o plano)

- `UserType` (`Back/Domain/Enums/UserType.cs`): `Manager = 0`, `Teacher = 1`, `Student = 2`.
- Roles padrão em `Back/Auth/Roles/EstudDefaultRoles.cs` (Diretor, Professor, Aluno), cada uma com `BaseType`.
- Policies por feature em `Back/Auth/Policies/Policies.*.cs`, via `AddEstudPolicy(nome, UserType, permissões...)`.
- Os dados que os pais vão consumir **já existem**:
  - Notas: `StudentClassNote`, `ClassActivityWork.Note`, `EstudStudent.YieldCoefficient`
  - Frequência: `ClassLessonAttendance`
  - Trabalhos/atividades: `ClassActivity` / `ClassActivityWork`
  - Agenda: feature `GetStudentAgenda`
  - Notificações: `Back/Features/Notifications`
- Criação de usuário vinculado a domínio segue o padrão de `CreateStudentService` (cria `EstudUser` + entidade de domínio + `EstudUserRole`).

---

## Fase 0 — Fundação: o novo tipo de usuário

Pré-requisito de tudo. Sem isso nenhuma feature existe.

### 0.1 Enum e role padrão

- Adicionar `Parent = 3` em `UserType` com `[Description("Responsável")]`.
- Adicionar role `Parent` em `EstudDefaultRoles`:
  - `Name = "Responsável"`, `NormalizedName = "RESPONSAVEL"`, `BaseType = UserType.Parent`, `Permissions = []` (igual a Aluno/Professor: o acesso vem das policies por `UserType`).
- Seed/migração para criar a role nas instituições existentes (mesmo mecanismo usado pelas roles atuais).

### 0.2 Entidades de domínio

Novo diretório `Back/Domain/Parents/`:

- **`EstudParent`** — espelha `EstudStudent`: `Id`, `InstitutionId`, `UserId`, `Name`, navegação `User`.
- **`ParentStudent`** — vínculo N:N responsável ↔ aluno:
  - `ParentId`, `StudentId`, `Relationship` (enum `ParentRelationship`: `Mother`, `Father`, `Guardian`...), `Status` (enum: `Pending`, `Active`, `Revoked`), timestamps.
  - Um aluno pode ter vários responsáveis; um responsável pode ter vários filhos na mesma instituição.
  - `RevokedByStudent` (bool): aluno maior de idade pode revogar o acesso do responsável (feature futura no portal do aluno); vínculo revogado pelo aluno bloqueia todas as queries de dados dele.

DbConfigs em `Back/Database/Parents/` seguindo o padrão snake_case + schema `estud`.

### 0.3 Criação e vínculo do responsável

Nova permissão `EstudPermissions.ManageParents` (adicionada à role Diretor em `EstudDefaultRoles`), para a secretaria poder delegar só a gestão de responsáveis.

Feature `Back/Features/Parents/CreateParent/` (policy `Manager` + `ManageParents`):

- A secretaria cadastra o responsável informando nome, e-mail e o(s) aluno(s) vinculado(s) + parentesco.
- Fluxo igual ao `CreateStudentService`: cria `EstudUser`, `EstudParent`, `EstudUserRole` e os `ParentStudent`.
- Command assíncrono para enviar e-mail de boas-vindas / definição de senha (reaproveitar o fluxo de reset password).

> Decisão de segurança: **o vínculo pai↔filho é sempre criado pela instituição** (ou futuramente confirmado por ela). Pais não se auto-vinculam a alunos — evita acesso indevido a dados de menores.

### 0.4 Autorização e contexto

- Novo arquivo `Back/Auth/Policies/Policies.Parents.cs` com as policies das features abaixo.
- Todas as queries de dados do filho validam o vínculo: `ctx.ParentStudents.Any(x => x.ParentId == parentId && x.StudentId == studentId && x.Status == Active)` — nunca confiar no `studentId` vindo da rota sem essa checagem.
- Expor o `ParentId` no `ctx.RequestUser` (mesmo mecanismo usado para aluno/professor).

### 0.5 Frontend — casca do portal do responsável

- Login existente já funciona (usuário é um `EstudUser` normal).
- Menu/layout condicionado ao `UserType.Parent`.
- **Seletor de filho**: componente global (header) para responsáveis com mais de um filho; todas as telas seguintes operam sobre o filho selecionado.
- Página inicial do responsável (`/children` ou similar) listando os filhos vinculados.

**Entrega da Fase 0**: responsável consegue logar, ver a lista de filhos e navegar — ainda sem dados acadêmicos.

---

## Features — da maior para a menor prioridade

Ordenadas por valor entregue ao usuário final. Cada uma segue o padrão vertical slice (Controller + Service + DTOs + Mapper) em `Back/Features/Parents/`.

### P1 — Visão geral do filho (boletim: notas + frequência)

**Por quê primeiro**: é a pergunta nº 1 de qualquer responsável — "como meu filho está indo?". Sozinha já justifica o portal.

- `GetParentStudentOverview` — resumo do filho: turmas ativas, média/notas por disciplina, % de frequência, coeficiente de rendimento.
- `GetParentStudentClasses` / `GetParentStudentClass` — detalhe por turma: notas (`StudentClassNote`, `ClassActivityWork.Note`) e frequência aula a aula (`ClassLessonAttendance`).
- Frontend: dashboard do filho com cards de disciplina (nota atual + frequência), com destaque visual para risco (frequência < 75%, nota abaixo da média).
- Tudo somente leitura, reaproveitando os dados já gravados pelos professores.

### P2 — Agenda e calendário do filho

**Por quê**: valor recorrente diário — "que aula/prova/entrega tem hoje?". Dados já existem (`GetStudentAgenda` + `Calendar`).

- `GetParentStudentAgenda` — mesma projeção do `GetStudentAgenda`, com autorização de responsável.
- Incluir eventos do calendário institucional (feriados, semana de provas).
- Frontend: tela de agenda idêntica à do aluno, em modo leitura.

### P3 — Trabalhos e atividades do filho

**Por quê**: fecha o ciclo do acompanhamento acadêmico — o responsável vê o que foi pedido, o que foi entregue e a nota.

- `GetParentStudentClassActivities` / `GetParentStudentClassActivity` — lista e detalhe das atividades (`ClassActivity`) com status de entrega e nota do `ClassActivityWork` do filho.
- Frontend: lista com filtros (pendente / entregue / avaliado) e alerta de atividade com prazo próximo não entregue.

### P4 — Ficha de saúde do filho (escrita pelos pais)

**Por quê primeiro entre as features de escrita**: é o dado que só o responsável tem e que a escola mais precisa — segurança física do aluno. Valor bidirecional (pais alimentam, escola consome).

- Nova entidade `StudentHealthRecord` (`Back/Domain/Students/`), 1:1 com `EstudStudent`:
  - `BloodType` (enum), `Allergies`, `DietaryRestrictions`, `ChronicDiseases`, `MedicationsInUse`, `AdditionalNotes`, `UpdatedByUserId`, `UpdatedAt`.
- Features:
  - `GetParentStudentHealthRecord` / `UpsertStudentHealthRecord` — responsável lê e edita (policy `Parent` + vínculo ativo).
  - `GetStudentHealthRecord` / `UpsertStudentHealthRecordByManager` (lado `Manager`, permissão `ManageStudents`) — secretaria/coordenação consulta e também edita a ficha.
- Auditoria: alterações passam pelo `AuditTrail` existente, com `UpdatedByUserId` registrando quem alterou — responsável ou secretaria (dado sensível — LGPD: dado de saúde de menor, acesso restrito e auditado).
- Frontend: formulário de ficha de saúde na página do filho + visualização/edição na tela de detalhes do aluno (lado gestor).

### P5 — Contatos de emergência

**Por quê**: complemento natural da ficha de saúde; simples e de alto valor operacional para a escola.

- Nova entidade `StudentEmergencyContact` (N por aluno): `Name`, `Relationship`, `PhoneNumber`, `Priority`.
- Features: `GetStudentEmergencyContacts` / `CreateStudentEmergencyContact` / `UpdateStudentEmergencyContact` / `DeleteStudentEmergencyContact` (responsável) + leitura no lado gestor.
- Frontend: lista ordenável de contatos na página do filho.

### P6 — Notificações para responsáveis

**Por quê**: transforma o portal de "consulta passiva" em canal ativo — o responsável fica sabendo sem precisar entrar.

- Estender `CreateNotification` para segmentar `UserType.Parent`.
- Notificações automáticas via `Command`s existentes nos pontos de gravação:
  - nota lançada, falta registrada, atividade criada / prazo próximo, frequência do filho abaixo de limiar.
- Frontend: inbox já existente passa a atender o responsável; e-mail para eventos críticos.

### P7 — Boletim/declarações em PDF

- `GetParentStudentReportCard` — boletim do período em PDF para download/impressão.
- Menor prioridade: o dado já está visível em P1; aqui é conveniência de formato.

### P8 — Autosserviço de vínculo (convite pela secretaria, aceite pelo responsável)

- Fluxo de convite: secretaria dispara convite por e-mail; responsável se registra sozinho e o vínculo nasce `Pending` até confirmação.
- Reduz trabalho manual da secretaria em escala; irrelevante enquanto a base é pequena — por isso por último.

---

## Ordem de entrega sugerida

| Entrega | Conteúdo | Valor percebido |
|---|---|---|
| 1 | Fase 0 + P1 | Responsável loga e vê notas/frequência do filho |
| 2 | P2 + P3 | Acompanhamento diário completo (agenda + trabalhos) |
| 3 | P4 + P5 | Escola ganha ficha de saúde e contatos de emergência |
| 4 | P6 | Canal ativo de comunicação |
| 5 | P7 + P8 | Conveniências e escala |

## Decisões tomadas

1. **Permissão**: nova `EstudPermissions.ManageParents` (não reaproveitar `ManageStudents`), para a secretaria poder delegar só a gestão de responsáveis.
2. **Aluno maior de idade**: campo `RevokedByStudent` no vínculo `ParentStudent` permite ao aluno maior revogar o acesso do responsável.
3. **Multi-instituição**: manter o modelo single-institution por usuário na v1 (um cadastro por instituição; mesmo e-mail não pode repetir). Revisitar depois se houver demanda.
4. **Ficha de saúde**: editável por responsáveis **e** secretaria, com auditoria de quem alterou (`UpdatedByUserId` + `AuditTrail`).

## Testes

Cada feature nova segue o padrão de `Tests/Features/` (partial `IntegrationTests`, regions Authentication / Authorization / Validation errors / Happy path). Casos de autorização críticos:

- Responsável tentando acessar dados de aluno **não vinculado** → 400/403.
- Vínculo `Revoked`/`Pending` → sem acesso.
- Aluno/professor tentando acessar endpoints de responsável → 403.
