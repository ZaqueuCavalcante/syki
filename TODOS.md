# TODOS

- System Docs
- Login with Google
- SSO Multi-Tenant
- Add RBAC dynamic roles system (feature based)
- Database schema viz with Vue
- Projeto deve mostrar que eu sei contruir um sistema full-stack
- Testes com ordem bem definida (Authentication, Authorization, Validation errors, Business logic errors, Happy path)
- Sistema de tranking de eventos relevantes
- Domínio estud.com.br na Hostinger
- Suporte para Ensino Fundamental e Médio (pais dos alunos)
- https://github.com/ZaqueuCavalcante/syki/issues/81



analise todas as policies do backend (procura pelo metodo AddSykiPolicy)
elas protegem os endpoints, cada policy define regras de acesso com base no tipo e nas permissoes do usuario logado

quero levar essa exata mesma ideia pro frontend
no front deve existir alguma estrutura que pega os dados do usuario logado (UserType e lista Permissions, obtidos via endpoint no objeto GetUserAccountOut)
e aplicar determinada logica pra definir se um component, pagina ou qualquer outro recurso do frontend deve ser acessivel/visivel pro usuario




o objeto GetUserAccountOut possui todos os dados do usuario logado
pra agora vamos focar apenas no UserType e na lista Permissions





