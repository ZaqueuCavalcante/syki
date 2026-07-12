# TODOS

- System Docs
- Login with Google (social e OneTap)
- SSO Multi-Tenant
- Add RBAC dynamic roles system (feature based)
- Database schema viz with Vue
- Projeto deve mostrar que eu sei contruir um sistema full-stack
- Testes com ordem bem definida (Authentication, Authorization, Validation errors, Business logic errors, Happy path)
- Sistema de tracking de eventos relevantes
- Domínio estud.com.br na Hostinger
- Suporte para Ensino Fundamental e Médio (pais dos alunos)
- https://github.com/ZaqueuCavalcante/syki/issues/81

- https://vueflow.dev/
- https://mermaid.js.org/




crie um novo item na sidebar que so deve aparecer pra quem tem UserType=Manager
vai ser o Calendário
crie uma pagina pra isso tbm

no backend cria uma pasta Features/Calendar
cria um endpoint GetInstitutionCalendar, que deve retornar um GetInstitutionCalendarOut
o GetInstitutionCalendarOut vai ter uma lista com GetInstitutionCalendarItemOut
cada GetInstitutionCalendarItemOut tera o DateTime Data e DayType
DayType pode ser Default | Halyday | Recesso | Feriado (coloque nomes em ingles)


o objetivo é que o gestor consiga ver e customizar o calendario academico da instituicao
deve ser possivel adicionar dias de ferias, semanas de provas, eventos, feriados nacionais e regionais...

