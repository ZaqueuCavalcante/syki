# TODOS

- System Docs
- Login with Google (social e OneTap)
- SSO Multi-Tenant
- Add RBAC dynamic roles system (feature based)
- Database schema viz with Vue
- Projeto deve mostrar que eu sei construir um sistema full-stack
- Testes com ordem bem definida (Authentication, Authorization, Validation errors, Happy path)
- Sistema de tracking de eventos relevantes
- Suporte para Ensino Fundamental e Médio (pais dos alunos)
- https://github.com/ZaqueuCavalcante/syki/issues/81

- https://vueflow.dev/
- https://mermaid.js.org/


crie um endpoint novo para Adicionar professores a uma turma (no máximo 2)
ou seja, o endpoint recebe uma lista de int com ate dois ids de professores
valida que os professores existem pra institution atual
valida que os professores possuem vinculo com a disciplina da turma
e criar/edita os registros ClassTeacher no banco

crie testes de integracao pra o novo endpoint tbm

ele pode ficar na pasta Features/Classes, um POST em /classes/{classId}/teachers
{
    "teachers": [14, 32]
}



- Criar turma (frontend + testes)
- Adicionar professores a uma turma (no máximo 2)
- Definir horários da turma (aparecem nas agendas dos alunos e professores)
- Matricular alunos na turma
- Em que momento criar as aulas da turma?
    - Ao iniciar a turma?
    - Sob-demanda na hora da chamada?
- Vincular opcionalmente salas na turma (validar horários livres da sala)
- Após iniciada a turma, os professores podem
    - Fazer chamadas
    - Criar atividades

- Cada atividade
    - Pode ter várias entregas
    - Pode ser individual ou em grupo

- Cada entrega vai ter uma nota
    - Individual do aluno
    - Pode ter uma nota em grupo que por baixo vira a nota do aluno









- Organizar a documentação por grupo de tópicos/conceitos
    - Vão ter coisas que são apenas conceitos
    - Outras serão guias de como usar/configurar o sistema

- Focar em Turma
    - Aulas
    - Agenda
    - Atividades, entregas e notas
    - Frequências

- Os dados acima servem de input pra diversas queries, análises, insights e dashboards


