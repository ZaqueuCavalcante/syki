# Timetabling — Algoritmo mais simples

Creating Timetables Automatically with FET Software Tutorial = https://www.youtube.com/watch?v=NXZO_SF7Xi4

O algoritmo de timetabling mais simples é o **guloso sequencial** (*greedy sequential / first-fit*): encaixa o próximo evento no primeiro horário livre. Não precisa de solver nem biblioteca.

## Ideia central

Eventos (aula = disciplina + turma + professor) são alocados em slots (dia × horário × sala), respeitando restrições de conflito.

```
para cada evento na lista:
    para cada slot possível (dia, hora, sala):
        se o slot não gera conflito:
            aloca o evento nesse slot
            break
    se nenhum slot serviu:
        marca como "não alocado"
```

## Conflitos (hard constraints)

- Mesmo **professor** em dois lugares no mesmo horário
- Mesma **turma/aluno** em duas aulas no mesmo horário
- Mesma **sala** ocupada por duas aulas no mesmo horário

Mantém-se 3 mapas de ocupação (professor→horários, turma→horários, sala→horários); checagem O(1) com `HashSet`.

## Melhoria barata: ordenar antes (graph coloring guloso)

Ordenar os eventos do mais restrito para o menos restrito antes de alocar (*most-constrained-first* / DSATUR simplificado) reduz muito os "não alocados" sem mudar a estrutura.

## Limitações

- Não é ótimo e pode falhar mesmo existindo solução (guloso, sem backtracking).
- Não otimiza soft constraints (janelas, agrupamento de aulas, preferências).

## Escala de complexidade

| Nível | Técnica | Quando usar |
|---|---|---|
| Mais simples | Greedy first-fit | Protótipo, poucos conflitos |
| Simples+ | Greedy + ordenação (graph coloring) | Maioria dos casos reais pequenos/médios |
| Médio | Backtracking / CSP | Quando greedy deixa buracos |
| Avançado | Metaheurísticas (Simulated Annealing, GA, Tabu) ou solver (OR-Tools CP-SAT) | Otimizar soft constraints |

---

# Modelo de Input/Output no Estud (validado contra as entidades reais)

## Escopo de execução

O timetabling roda por **`(InstitutionId, CampusId, AcademicPeriodId)`**. Só faz sentido dentro de um campus específico: oferta de curso, salas, professores e coortes de um campus X não têm relação com entidades de um campus Y, mesmo na mesma instituição. Reforçado pelo schema: `CourseOffering.CampusId`, `TeachersCampi`, e a sala (quando existir) também pertence a um campus.

## Input

| Item | Entidade real | Observação |
|---|---|---|
| Cursos | `Course` | — |
| Disciplinas | `Discipline` | — |
| Professores | `EstudTeacher` + `TeachersDisciplines` + `TeachersCampi` | Vínculos professor↔disciplina e professor↔campus são **restrições duras** |
| Ofertas de curso | `CourseOffering` | `(Institution, Campus, Course, Curriculum, AcademicPeriod, Session/Turno)` |
| Grades curriculares | `CourseCurriculum` + `CourseCurriculumDiscipline` | Liga disciplina → semestre (`Period`), créditos e carga horária |
| Salas de aula | `Classroom` (`Back/Domain/Classrooms/Classroom.cs`) | Já existe: `CampusId`, `Name`, `Capacity`. `Schedule.ClassroomId` referencia essa entidade |
| Cargas horárias / créditos | `CourseCurriculumDiscipline` (`Credits`, `Workload`) | Definem quantos slots cada disciplina precisa |

## Coorte no lugar de "Alunos"

Alunos **não** são input do timetabling inicial. Matrícula é separada da criação de turma. Quem exige "essas disciplinas precisam caber juntas na grade" é o **coorte**, não o aluno individual:

```
coorte = (CourseOffering) × (Period do CourseCurriculumDiscipline)
```

Todas as disciplinas de um mesmo `Period` de uma mesma oferta são cursadas pela mesma leva → **não podem colidir entre si**. Esse é o "conflito de aluno" que importa, derivado da grade.

Alunos só viram input de verdade com matrícula livre/eletivas atravessando ofertas (garantir grade sem choque por aluno) — problema mais difícil e posterior.

## Output

`List<Class>`, cada uma com:

- 1 disciplina, 1 coorte, vagas
- N `Schedule`, cada um com `(Day, Start, End, ClassroomId, TeacherId)` resolvidos

### Professor e sala são por-`Schedule`, não por-`Class`

O requisito "uma mesma turma pode ter salas diferentes durante a semana" **já é suportado pelo schema**:

- `Class` **não** tem `TeacherId` único — tem `List<EstudTeacher> Teachers`, vínculo many-to-many via `ClassTeacher`, com **máximo de 2 professores por turma** (`AssignTeachersToClassService.MaxTeachers`). `CampusId` é nullable (turma online).
- `Schedule` tem `ClassroomId` **e** `TeacherId` próprios, por linha (por dia/horário).

Logo, a alocação de sala/professor produzida pelo timetabling é gravada **em cada `Schedule`**, não na `Class`. Quando a turma tem 2 professores, o solver também precisa decidir **qual dos dois cobre cada horário** — mesma regra que `UpdateClassSchedulesService` já aplica hoje na edição manual (0 professores → horário sem professor; 1 → preenchido automaticamente; 2 → obrigatório escolher por `Schedule`).

## Peça já pronta

`Schedule.Conflict(other)` (em `Back/Domain/Classes/Schedule.cs`) já implementa a checagem de sobreposição de horários — é exatamente o predicado de conflito das 3 restrições duras (professor, sala, coorte).
