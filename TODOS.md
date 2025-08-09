# Alocação de Salas e Professores

- Classroom Assignment Problem
- School Timetabling Problem

- Utilizar Metaheurísticas

1 - Pegar cursos/grades/alunos
    - Definir quantos alunos cada disciplina precisa ter

2 - Pegar professores com suas restricoes de carga-horaria e preferencias de horarios

3 - Pegar salas, considerando grupos e recursos

4 - Juntar tudo num esquema consolidado




- Aulas de uma mesma disciplina não poderão ser alocadas em dias consecutivos
- Posso ter no máximo X aulas de cada disciplina consecutivas
- Disciplinas do periodo atual de um curso nao podem ter horarios conflitantes
    - Um aluno blocado precisa poder cursar todas as disciplinas do periodo durante a semana


- Alunos com necessidades especiais devem estar em salas com acessibilidade
- Os alunos devem utilizar a mesma sala durante a semana, de preferencia
- Se possível cada uma das salas deve ser deixada vazia em pelo menos um horário ao longo do dia de forma a favorecer a limpeza

- Levar em conta
    - Intervalos (meia hora a cada 3 aulas...)
    - Feriados
    - Carga horaria total no periodo deve ser maior ou igual a da disciplina



- Criar turma vinculada apenas com Disciplina
    - Sem Professor
    - Sem Sala
    - Sem Horarios
    - Opcionalmente vinculada a um Campus (aula online é cross-campus)

- Ensino Fundamental e Médio
    - Ideia de reutilizar/clonar turma do ano anterior no ano atual

- Professores devem possuir uma lista de disciplinas que podem lecionar
    - Nao deve ser possivel atribuir uma turma de Calculo à um professor de Biologia...

- Professores podem morar longe e possuirem preferencias de horarios
    - Todas as aulas da semana segunda e terca, por exemplo
    - Levar em conta na resolucao

- Reserva de salas
    - Recorrente
    - Dia e horario especifico


Todo início de semestre, temos que:
    - Para cada campus, definir quais disciplinas devem ser ofertadas
    - Cada curso terá disciplinas "com oferta garantida" no semestre (instituições menores podem não seguir esta regra)
    - A oferta de cada disciplina deve levar em conta os alunos habilitados a cursá-la (pré-requisitos)
    - Uma turma pode acolher alunos de diferentes ofertas de curso, mas que pagam a mesma disciplina
    - Turma e sala possuem uma capacidade máxima de vagas/alunos
    - Os professores devem ser alocados com base em valores de carga-horária mínima e máxima
    - Um professor não pode estar em duas turmas ao mesmo tempo
    - Um aluno não pode estar em duas turmas ao mesmo tempo
    - Uma mesma turma não pode estar em duas salas ao mesmo tempo
    - Uma mesma turma pode ser vinculada a mais de uma sala
    - Uma mesma sala pode ser vinculada a mais de uma turma, em dias e horários diferentes
    - Cada disciplina possui uma carga-horária que precisa ser satisfeita dentro do semestre
    - Cada semestre possui uma data de início e uma de fim
    - É preciso levar em conta feriados e outros fatores que podem atrapalhar o cumprimento da carga-horária
    - Algumas turmas podem precisar necessariamente de um recurso (projetor, laboratório) ofertado por algumas poucas salas 

## Minimizar o deslocamento de alunos e professores entre salas num mesmo dia

- Precisamos coletar informações sobre a localização relativa entre as salas
- Salas vizinhas, no mesmo andar, mesmo prédio, prédios diferentes...

## Constraint Programming

- Hard Constraints
    - Uma sala só pode ter uma turma por vez
    - Uma turma precisa de pelo menos uma sala
    - A capacidade da sala deve ser maior ou igual ao número de alunos da turma

- Soft Constraints
    - Preferência por salas com projetor
    - Agrupar aulas do mesmo curso na mesma região do campus

## NP-Hard Problem

- Inviável de resolver por força bruta



# Scalar API Docs

- Request examples
    - Body
    - Route
    - Query

- Response examples
    - Many

- Error examples
    - Many

- Filters
    - Global configs




# Start Simple

- Dados um Campus e um Período
    - Listar todas as ofertas de curso ativas
    - Com base na grade de cada oferta (o no primeiro período de cada uma), determinar quais disciplinas devem ser abertas no período atual
    - Para cada oferta, determinar quais alunos estão aptos a cursar cada disciplina (aprovados/reprovados)
    - Montar uma lista com items assim:
        - DisciplineId, CourseOfferingId, StudentId
    - Agrupando:
        - DisciplineId, CourseOfferingId -> 15 alunos
        - DisciplineId -> 87 alunos





