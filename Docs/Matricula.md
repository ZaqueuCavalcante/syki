## Matricula

- Para liberar o periodo de matricula pros alunos, o academico deve:
    - Criar um PeriodoDeMatricula pro periodo atual
    - Cadastrar todas as turmas

- Para que um aluno possa realizar sua matricula naquele periodo:
    - Deve haver um PeriodoDeMatricula vinculado ao periodo e o request deve ser feito dentro dos limites de Start e End
    - Ele deve poder selecionar todas as disciplinas que fazem sentido pra sua situacao academica atual:
        - Disciplinas que estao na grade da sua oferta de curso
        - Disciplinas nas quais ele ainda nao foi aprovado
        - Disciplinas que nao tem pre-requisitos
        - Disciplinas que tem pre-requisitos mas o aluno ja cumpriu
        - Turma ainda tem vagas abertas





