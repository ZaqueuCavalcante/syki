# Componentes

- Campi = CampusA, CampusB, CampusC

- Período = Período01, Período02

- Cursos = CursoA, CursoB, CursoC, CursoD, CursoE

- Disciplinas = DisciplinaA, DisciplinaB, DisciplinaC, DisciplinaD, DisciplinaE, DisciplinaF

    🔗 Pode estar vinculada com um ou mais cursos

- Grades = GradeA, GradeB

    🔗 Possui um curso
    🔗 Possui uma ou mais disciplinas do curso acima

- Ofertas = OfertaA, OfertaB, OfertaC, OfertaD

    🔗 Possui um campus
    🔗 Possui um período
    🔗 Possui um curso
    🔗 Possui uma grade

- Professores = ProfessorA, ProfessorB, ProfessorC

    🔗 Trabalha em um ou mais campus
    🔗 Leciona uma ou mais disciplinas
    🔗 Pode ter horários de preferência

- Alunos = AlunoA, AlunoB, AlunoC, AlunoD, AlunoE, AlunoF, AlunoG, AlunoH

    🔗 Está matriculado em uma oferta

- Turmas = TurmaA, TurmaB, TurmaC

    🔗 Possui um campus
    🔗 É de uma disciplina
    🔗 É lecionada por um professor

- Salas = SalaA, SalaB, SalaC, SalaD

    🔗 Está localizada em um campus
    🔗 Possui uma ou mais turmas
    🔗 Cada turma da sala possui um ou mais horários

- Horários = Horário01, Horário02, Horário03, Horário04

    🔗 Pode apontar para um professor
    🔗 Pode apontar para uma turma
    🔗 Pode apontar para uma sala

# Ensalamento

- Manual

    🔗 Realizar vínculos
    🔍 Validar consistência dos dados
    ⏪ Capacidade de desfazer um vínculo

- Automático

    🖱️ Gerar resultado com um click

# Resultado

- Visão do acadêmico

    🎓 Distribuição de uso das salas (tempo e quantidade de alunos)
        - Global (todos os dias, todos os turnos)
        - Diária (pra determinado dia, todos os turnos)
        - Por turno (todos os dias)
        - Diária por turno

    📅 Horário semanal de cada sala
        - Detalhes sobre a distribuição de uso

- Visão do professor

    📅 Horário semanal
    ➡️ Para cada dia:
        - Turma(s)
        - Horário(s)

- Visão do aluno

    📅 Horário semanal
    ➡️ Para cada dia:
        - Turma(s)
        - Horário(s)
