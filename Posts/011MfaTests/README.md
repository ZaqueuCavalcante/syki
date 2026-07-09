# ✅ Como você testaria isso? 🧪

Estou desenvolvendo um sistema open source, voltado para o gerenciamento acadêmico de instituições de ensino. Ele suporta MFA (Multi-Factor Authentication) e quero criar testes automatizados para essa funcionalidade.

O fluxo normal acontece quando o usuário:
 - 1) Cria uma conta no sistema
 - 2) Realiza login usando email + senha
 - 3) Faz o setup do MFA (lendo sua chave e gerando o TOTP a partir dela)
 - 4) Realiza logout e tenta logar novamente, informando email + senha
 - 5) Informa corretamente o TOTP atual
 - 6) Recebe o seu JWT, finalmente logando no sistema

Para evitar que alguém consiga logar direto, somente informando o TOTP, a API gera um cookie (Identity.TwoFactorUserId) contendo o UserId e manda na resposta do passo 4 apenas.

Por último, se você informar email + senha corretos, mas TOTP errado, não deve conseguir logar.

💡O jeito mais efetivo que encontrei pra realizar esses testes: subir uma única vez o Postgres + API (com WebApplicationFactory) e rodar todos os testes batendo diretamente na API, usando o HttpClient.

Dessa forma consigo:
 - Rodar todos os testes em paralelo
 - Ter um ambiente de testes o mais próximo possível do de produção
 - Testar facilmente a autenticação via cookie do passo 5 (o próprio HttpClient já pega o cookie recebido e envia na próxima request)

Abaixo mostro como ficou o teste do fluxo normal.

<p align="center">
  <img src="https://github.com/ZaqueuCavalcante/estud/blob/master/Posts/011MfaTests/test.gif?raw=true" style="display: block; margin: 0 auto" />
</p>

> Você pode ver o código aqui (https://github.com/ZaqueuCavalcante/estud) e a aplicação rodando aqui (https://app.estud.com.br). Venho postando no LinkedIn todas as atualizações de desenvolvimento do projeto (https://www.linkedin.com/in/zaqueu-cavalcante).

> O que achou dessa nova funcionalidade? Faria diferente? Acha que faltou algo ou possui ideias para melhorar o projeto? Me deixe saber nos comentários abaixo, valeu!
