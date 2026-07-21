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


- Migração de dados de um sistema X pro Estud
- Endpoints + Interface de Admin (sobe sob demanda)




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






Sidebar do aluno

- Curso (vai trazer dados sobre o curso atual dele, a grade de disciplinas, quais ele ja pagou...)

- Notas (traz as notas do aluno das disciplinas do curso, a sua nota media, graficos de notas)

- Frequência (traz as frequencias do aluno das disciplinas do curso, a sua frequencia media, graficos de frequencias)



# Steps

- Agendas
    - Professor
    - Aluno
    - Turma
    - Sala

- Levar em conta os dias letivos pro cálculo da carga horária da turma
    - Existe a carga horária objetivo e a carga horária atualmente sendo feita
    - O cálculo da CH prevista é feita com base nos dias letivos do período
    - A cada aula as horas cumpridas aumentam

- Frequência feita pelo professor a cada aula
    - Remover metodos "private async Task" dos testes

- Atividades
    - CreateClassActivity
    - Notificações
    - Entregas
    - Notas


