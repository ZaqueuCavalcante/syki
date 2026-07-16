# Plano — Multi-instituição: usuário global, perfis por instituição

Hoje um usuário pertence a exatamente uma instituição. Este plano evolui o Estud para um modelo de **conta global** (uma pessoa = um `EstudUser`) com **perfis por instituição** (Professor, Aluno, Responsável, Gestor), cobrindo:

- Professor que leciona em duas ou mais instituições **ao mesmo tempo**.
- Professor/aluno que **muda** de instituição e continua usando a mesma conta.
- Aluno que percorre a jornada fundamental → médio → faculdade, todas no Estud, mantendo identidade e histórico.
- Responsável com filhos em escolas diferentes (supera a decisão nº 3 do `Parents.md`, que fixou single-institution na v1).

## Estado atual (acoplamentos a desfazer)

| Ponto | Hoje | Problema |
|---|---|---|
| `EstudUser` (`Back/Domain/Identity/EstudUser.cs`) | Tem `InstitutionId` direto | Usuário preso a 1 instituição |
| E-mail | Único global (`ctx.Users.AnyAsync(u => u.Email == email)`) | Professor já cadastrado na instituição A não pode ser criado na B (`EmailAlreadyUsed`) |
| JWT (`SignInService`) | Claim `inst` vem de `user.InstitutionId` | Sessão não tem noção de "instituição ativa" escolhida |
| `RequestUser` / `EstudDbContext` | `InstitutionId` populado do token por request | OK — continua funcionando se o token carregar a instituição ativa |
| `EstudUserRole` | **Já tem `InstitutionId`** | Nenhum — o vínculo usuário↔role já é por instituição, base perfeita para o novo modelo |
| `EstudStudent` / `EstudTeacher` | Têm `UserId` + `InstitutionId` próprios | Nenhum — já são, na prática, "perfis por instituição"; hoje só existe 1 por usuário |

A boa notícia: o domínio já está quase pronto. `EstudStudent`/`EstudTeacher` já são entidades separadas do usuário, escopadas por instituição. O trabalho grosso é na **identidade e na sessão**, não no domínio acadêmico.

## Conceito

```
EstudUser (global)                      ← identidade: e-mail, senha, 2FA, social logins, nome, foto
 ├── EstudUserRole (inst A, Professor)  ← papel na instituição
 ├── EstudUserRole (inst B, Professor)
 ├── EstudTeacher  (inst A)             ← perfil: disciplinas, campi
 ├── EstudTeacher  (inst B)
 └── EstudStudent  (inst C)             ← perfil: matrícula, status, coeficiente, turmas, notas
```

- **Global (da pessoa)**: credenciais, 2FA, social logins, nome, foto, data de nascimento, telefone.
- **Por instituição (do vínculo)**: role, matrícula, status, coeficiente, turmas, notas, frequência, disciplinas/campi do professor, ficha de saúde e contatos de emergência do aluno (cada instituição gerencia os seus — isolamento LGPD entre instituições).
- Uma pessoa pode ter **papéis diferentes em instituições diferentes** (professor na A, aluno de pós na B) — o modelo suporta naturalmente, já que role e perfil são por instituição.

---

## Fase 1 — Desacoplar identidade da instituição ✅ (código aplicado; falta gerar migração e rodar testes)

### 1.1 `EstudUser` global

- Remover `InstitutionId`/`Institution` de `EstudUser`.
- Fonte da verdade dos vínculos passa a ser `EstudUserRole` (já tem `InstitutionId` + `RoleId`).
- Migração de dados: nada a migrar em `EstudUserRole` (já carrega a instituição); apenas dropar a coluna de `users` após validar que todo usuário tem ao menos um `EstudUserRole`.
- Varredura dos usos de `user.InstitutionId` no backend (criações de entidades, queries, webhooks) trocando pela instituição do contexto (`ctx.RequestUser.InstitutionId`) ou do vínculo em questão.

### 1.2 Convenção de unicidade

- E-mail continua único global — agora corretamente, pois identifica a **pessoa**, não o vínculo.
- Nova unicidade nos vínculos: `(UserId, InstitutionId)` único em `EstudUserRole` (uma pessoa tem um papel por instituição na v1 — simplifica sessão e UI; papéis múltiplos na mesma instituição ficam fora do escopo).
- `(UserId, InstitutionId)` único também em `EstudTeacher` e `EstudStudent`.

**Entrega da Fase 1**: modelo de dados pronto, comportamento externo idêntico ao atual (todo mundo continua com 1 instituição).

---

## Fase 2 — Sessão com instituição ativa

### 2.1 Login em duas etapas (quando necessário)

- `SignInService` deixa de ler `user.InstitutionId` e passa a listar os vínculos ativos (`EstudUserRole`).
- **1 vínculo** → comportamento atual: token direto com claim `inst` desse vínculo.
- **N vínculos** → login retorna `RequiresInstitutionSelection` + lista (id, nome, logo, papel na instituição); novo endpoint `SelectInstitution` emite o token definitivo com a `inst` escolhida.
  - Mesmo padrão do fluxo 2FA existente (`LoginRequiresTwoFactor` + scheme intermediário): autenticação parcial num cookie/scheme temporário até a escolha.
- Vale para todos os fluxos de entrada: e-mail/senha, magic link, social login, SSO. Todos convergem no `SignInService`, então a mudança é num ponto só.

### 2.2 Troca de instituição sem novo login

- `SwitchInstitution` — usuário autenticado troca a instituição ativa; valida o vínculo e emite novo token/cookie.
- Frontend: seletor de instituição no header (só aparece para quem tem >1 vínculo), persistindo a última escolha (claim ou preferência) para o próximo login.

### 2.3 O que não muda

- `RequestUser`, policies, `EstudDbContext` escopado por `InstitutionId` — tudo continua igual, pois o token segue carregando **uma** instituição ativa. Nenhuma feature de domínio precisa mudar.

**Entrega da Fase 2**: uma conta consegue transitar entre instituições — mas ainda não existe jeito de **criar** o segundo vínculo.

---

## Fase 3 — Criar o segundo vínculo (a feature que destrava o valor)

### 3.1 Cadastro com e-mail já existente vira convite

Hoje `CreateTeacherService`/`CreateStudentService` retornam `EmailAlreadyUsed`. Novo comportamento:

- E-mail inexistente → fluxo atual (cria user global + perfil + role).
- E-mail já existente → **cria convite** (`InstitutionInvite`: `InstitutionId`, `Email`, `UserType`, `Status`, expiração) em vez de erro.
  - Command assíncrono envia e-mail; usuário logado também vê o convite pendente no app (badge/inbox).
  - `AcceptInstitutionInvite` / `DeclineInstitutionInvite` — ao aceitar, cria `EstudUserRole` + `EstudTeacher`/`EstudStudent` na nova instituição.

> Decisão de segurança: vincular conta existente a uma nova instituição **sempre exige aceite do dono da conta**. A instituição B não pode anexar uma pessoa à revelia — nem ver que o e-mail já tem conta no Estud além do estado "convite pendente".

### 3.2 Desligamento sem perda de histórico

- Desativar vínculo (professor sai da instituição A) = `EstudUserRole`/perfil ficam inativos; **nenhum dado é apagado** — notas lançadas, turmas, histórico permanecem íntegros na instituição.
- Usuário com vínculo inativo não loga naquela instituição, mas a conta global segue viva para as demais.

**Entrega da Fase 3**: professor lecionando em 2+ instituições simultâneas; aluno trocando de escola sem criar conta nova. **É aqui que o valor prometido chega ao usuário.**

---

## Fase 4 — Experiência multi-instituição no frontend

- Tela pós-login de escolha de instituição (cards com logo/nome/papel).
- Seletor no header + indicação clara da instituição ativa em todas as telas.
- Tela "Minhas instituições" na conta do usuário: vínculos ativos/inativos, convites pendentes, aceitar/recusar.
- Perfil do usuário separado em "dados pessoais" (globais, editáveis pelo dono) vs "dados do vínculo" (por instituição, geridos pela secretaria).

---

## Fase 5 — Extensões de valor (backlog, em ordem)

1. **Responsáveis multi-instituição** — `ParentStudent` cruzando instituições: o responsável tem um vínculo por instituição (como professor), e o seletor de filho do portal (ver `Parents.md`) resolve a instituição implícita de cada filho.
2. **Histórico acadêmico global do aluno** — timeline read-only cruzando instituições ("Minha jornada"): períodos, cursos concluídos, certificados. Cada instituição controla o que expõe. É a feature-vitrine do modelo global, mas depende de adoção real multi-instituição para ter valor.
3. **Transferência assistida** — instituição destino solicita dados da origem (com consentimento do aluno/responsável): documentos, ficha de saúde, histórico. Reduz recadastro manual.

---

## Riscos e salvaguardas

- **Isolamento entre instituições (LGPD)**: instituição A nunca vê dados da B. O escopo por `ctx.RequestUser.InstitutionId` já garante isso nas queries; os pontos novos de atenção são os fluxos globais (convites, "minhas instituições", histórico global) — cobrir com testes dedicados.
- **Convite malicioso**: instituição não pode descobrir onde a pessoa estuda/trabalha via convite; resposta de criação é idêntica para "usuário novo criado" e "convite enviado".
- **Tokens antigos durante a migração**: token sem estrutura nova de claim deve ser rejeitado/renovado com gracefulness (janela de expiração já é curta).
- **`AuditTrail`**: registros de auditoria já carregam instituição via contexto — validar que trocas de instituição na mesma sessão auditam com a `inst` correta.

## Testes

Seguem o padrão de `Tests/Features/` (partial `IntegrationTests`, regions). Cenários críticos:

- Login com 1 vínculo → token direto (regressão do fluxo atual).
- Login com 2 vínculos → `RequiresInstitutionSelection`; selecionar instituição sem vínculo → erro.
- `SwitchInstitution` para instituição sem vínculo ou com vínculo inativo → erro.
- Professor com perfil nas instituições A e B: dados criados na A invisíveis na B (queries, webhooks, notificações, auditoria).
- Cadastro com e-mail existente → convite criado, resposta indistinguível de criação nova; aceite cria perfil; recusa/expiração não cria nada.
- Vínculo desativado: login naquela instituição bloqueado, histórico preservado, demais instituições intactas.

## Sequência de entrega

| Entrega | Conteúdo | Valor percebido |
|---|---|---|
| 1 | Fase 1 | Invisível (fundação) — deploy isolado para reduzir risco |
| 2 | Fase 2 | Sessão multi-instituição funcionando (ainda sem 2º vínculo real) |
| 3 | Fase 3 | **Professor em 2 escolas / aluno transferido — o valor do plano** |
| 4 | Fase 4 | UX completa |
| 5 | Fase 5 | Responsáveis multi-inst, histórico global, transferência |
