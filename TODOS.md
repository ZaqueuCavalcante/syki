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


vamos criar uma nova feature de 2FA Enforcement no sistema

hoje o 2FA pode ser configurado pelo proprio usuario, mas nao existe nada que obrigue ele a fazer isso

a ideia do 2FA Enforcement eh que uma instituicao possa forcar que determinados usuarios seus sejam obrigados a logar usando 2FA

vamos fazer isso apenas pro login com email+senha (login com SSO e com Google Social Login ficam de fora por enquanto)

precisamos que o Manager consiga configurar isso (endpoints, telas, modais)

e precisamos que o usuario que tiver com o 2FA pela instituicao seja forcado a fazer o setup ao tentar logar
ou seja, o sistema deve mostrar uma tela de setup de 2FA quando o usuario tentar logar com email+senha E tiver 2FA Enforcement E ainda n configurou
perceba que apos informar email+senha o usuario ja logou, mas esse login deve ter um JWT com permissoes minimas para que ele consiga apenas fazer o setup do 2FA
apos o setup ele ja vai entrar realmente logado, com um JWT contendo todas as sua permissoes reais

crie um plano pra isso e salve em Plans/2FAEnforcement.md



