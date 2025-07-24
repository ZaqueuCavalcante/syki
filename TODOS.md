# Alocação de Salas e Professores

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


