# Hard Problems

Como "resolvi" um problema NP-Hard no meu projeto open-source

Este é um ótimo exercício de pensamento analítico: como quebrar um problemão em pequenos problemas mais simples, resolvendo um por vez até chegar na solução final.

O problema consiste em vincular professores, alunos e disciplinas a turmas. Depois cada turma deve ser vinculada a salas de aulas, em determinados horários.

Classroom Assignment Problem +  School Timetabling Problem

## Versão simplificada

- Uma instituição de ensino que possui apenas 1 campus
- Neste único campus, temos apenas 1 curso, 1 sala e 1 professor
- Apenas 3 alunos estão matriculados no curso
- O curso é de ADS, possuindo 5 períodos, cada período com 5 disciplinas

Precisamos montar o quadro de horários do campus para o curso de ADS.

O arranjo aqui é trivial, pois existe apenas uma possibilidade para cada decisão.

Sendo assim, vamos abrir uma turma para cada disciplina, onde todas as turmas serão ministradas pelo único professor do campus.

❌ Erros de validação
- Ao tentar colocar duas disciplinas na segunda-feira com horários conflitantes
- Ao tentar colocar 25 alunos em uma turma com limite de 22 alunos
- Ao tentar colocar uma turma com 20 alunos em uma sala com 15 vagas



# TODOS

- Criar config para determinar
    - Quais dias da semana a instituicao opera (default segunda a sexta)
    - Quais os turnos (default manhã, tarde e noite)
    - Em cada turno, quais os horários de início, intervalo, fim
    - Ter valores default para cada turno
        - Manhã -> 08:00 - 10:00 | 10:00 - 10:15 | 10:15 - 12:00
        - Tarde -> 14:00 - 16:00 | 16:00 - 16:15 | 16:15 - 18:00
        - Noite -> 19:00 - 20:30 | 20:30 - 20:45 | 20:45 - 22:00

- Abrir dialog para selecionar quais os horários
    - Exibir as turmas + horários já vinculados com a sala
    - Caso o professor tenha suas preferências, mostrar um hint
    - O select de horário deve ser exibido em um componente grande com colunas sendo os dias da semana
    - Validar no backend a consistência dos dados
    



