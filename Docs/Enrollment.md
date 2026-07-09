# Ciclo de Vida da Turma (Enrollment)

Este documento define o ciclo de vida de uma **Turma** (`Class`) no Estud, da criação
ao encerramento, incluindo matrícula de alunos e resolução (aprovado × reprovado).

## Visão geral

A espinha dorsal do ciclo já está codificada no enum `ClassStatus`
(`Back/Shared/Enums/ClassStatus.cs`). O status por aluno usa `StudentDisciplineStatus`
(`Back/Shared/Enums/StudentDisciplineStatus.cs`).

```
                      ┌─────────────────┐
   CreateClass  ──▶   │ OnPreEnrollment │   gestor cria a turma
                      └────────┬────────┘
                               │  gestor libera (exige Período de Matrícula vigente)
                      ┌────────▼────────┐
                      │  OnEnrollment   │◀── alunos se matriculam  (self-service)
                      │                 │◀── gestor aloca alunos    (mesmo vínculo)
                      └────────┬────────┘
                               │  (data-fim do período ⇒ AwaitingStart, virtual)
                               │  gestor inicia (exige período ENCERRADO)
                      ┌────────▼────────┐
                      │    Started      │   professor: aulas, chamadas, notas
                      └────────┬────────┘
                               │  ResolveStudents(): calcula nota média + frequência
                               │  → grava Aprovado / ReprovadoPorNota / ReprovadoPorFalta
                               │
                               │  Finalize()  ── GUARDA: nenhum aluno em 'Matriculado'
                      ┌────────▼────────┐
                      │   Finalized     │   turma encerrada
                      └─────────────────┘
```

## Estados da turma (`ClassStatus`)

| Status | Valor | Significado |
|---|---|---|
| `OnPreEnrollment` | 0 | Default ao criar a turma. Aguardando liberação para matrícula. |
| `OnEnrollment` | 1 | Matrícula aberta. Alunos entram na turma (self-service ou alocação do gestor). |
| `AwaitingStart` | 2 | **Virtual** — computado em memória pela data-fim do período. No banco continua `OnEnrollment`. |
| `Started` | 3 | Turma iniciada. Professor lança aulas, chamadas e notas. Sem retrocesso. |
| `Finalized` | 4 | Turma encerrada. Todos os alunos resolvidos. |

## Estados do aluno na turma (`StudentDisciplineStatus`)

Gravados no vínculo Turma↔Aluno.

| Status | Significado |
|---|---|
| `Pendente` | Estado inicial neutro. |
| `Matriculado` | Aluno matriculado, ainda não resolvido. |
| `Aprovado` | Passou (nota e frequência ≥ limites). |
| `Dispensado` | Dispensado da disciplina. |
| `ReprovadoPorNota` | Nota média < `NoteLimit` da instituição. |
| `ReprovadoPorFalta` | Frequência < `FrequencyLimit` da instituição. |

## Transições e guardas

| Transição | Quem dispara | Guarda |
|---|---|---|
| `→ OnPreEnrollment` | Gestor (`CreateClass`) | — (default no construtor de `Class`) |
| `OnPreEnrollment → OnEnrollment` | Gestor | Existe Período de Matrícula vigente |
| Matrícula de aluno | Aluno **ou** Gestor | Turma em `OnEnrollment`; dentro do período (self-service) |
| `OnEnrollment → Started` | Gestor (`StartClass`) | Período de Matrícula já encerrou (`today > period.EndAt`); sem retrocesso |
| Resolução dos alunos | Automático (`ResolveClassStudents`) | Turma `Started`; calcula nota + frequência de cada aluno |
| `Started → Finalized` | Gestor (`FinalizeClass`) | **Nenhum aluno em `Matriculado`** (todos já resolvidos) |

`AwaitingStart` nunca é persistido — é sempre computado em memória a partir da data-fim
do Período de Matrícula.

## Os dois modos de matrícula

Aluno se auto-matricular e gestor alocar o aluno são **duas portas de entrada para o
mesmo vínculo** Turma↔Aluno com `StudentDisciplineStatus.Matriculado`. A diferença é só
autorização/endpoint — o estado resultante é idêntico. Não requer status novo, apenas
duas features escrevendo no mesmo join:

- `EnrollStudentInClass` — aluno, self-service, só dentro do período.
- `AssignStudentsToClass` — gestor, aloca alunos manualmente.

## Resolução dos alunos e encerramento

Decisão adotada: **resolução automática + trava de encerramento**.

- A resolução é **calculada automaticamente** (`ResolveClassStudents`): para cada aluno,
  computa nota média + frequência e compara com `NoteLimit` / `FrequencyLimit` da
  instituição, gravando `Aprovado` / `ReprovadoPorNota` / `ReprovadoPorFalta`.
- Não há status intermediário na turma.
- `FinalizeClass` só é permitido se **nenhum aluno** ainda estiver em `Matriculado`
  (ou seja, todos já foram resolvidos). Isso garante integridade sem estado extra.

## O que precisa ser construído/ajustado no `Back/` atual

1. **Reintroduzir o join com payload** — hoje `Class` tem `List<EstudStudent> Students`
   (many-to-many puro, `Back/Domain/Classes/Class.cs`), sem onde gravar o status por
   aluno. Criar entidade de ligação `ClassStudent { ClassId, StudentId,
   StudentDisciplineStatus }` (default `Matriculado`). *(necessidade técnica)*

2. **Features de matrícula (duas portas, mesmo vínculo):**
   - `EnrollStudentInClass` — aluno, self-service, só dentro do período.
   - `AssignStudentsToClass` — gestor, aloca alunos manualmente.

3. **`StartClass`** — porta o legado (`Legacy/.../StartClasses`); guarda de período encerrado.

4. **`ResolveClassStudents`** — calcula nota + frequência vs. limites da instituição e
   grava o `StudentDisciplineStatus`. Pode ser acionado explicitamente ou embutido no
   `Finalize`; mesmo embutido, a guarda garante integridade.

5. **`FinalizeClass`** — porta o legado (`Legacy/.../FinalizeClasses`); com a guarda
   "nenhum aluno em `Matriculado`".

## Pendências ortogonais ao ciclo

- **Cálculo da nota**: hoje é média simples (`GetAverageNote()`). O legado tinha
  `// TODO: Calculate notes using the class activities`. Definir se a nota vem de
  atividades ponderadas ou média simples — não afeta o ciclo de vida.
