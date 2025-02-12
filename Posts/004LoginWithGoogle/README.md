# Login With Google

- OAuth2 serve para autorizar em outro sistema (acesso limitado)
    - Estou no draw.io e quero salvar meus diagramas no Google Drive

- OpenID Connect serve para se autenticar no sistema A, usando uma conta no sistema B
    - Extensão do OAuth2, traz dados (IdToken, /userinfo) sobre o usuário além do token de acesso
    - Quero logar no Syki usando minha conta do Google/LinkedIn/GitHub...

CRIAR UMA API EM .NET PARA SIMULAR O DRAW.IO
ASSIM CONSIGO DEBUGAR TODOS OS PASSOS E VER TODAS AS INFORMAÇÕES

- Trocar draw.io pelos contatos que tenho no gmail?
    - Pensar em outras aplicações

LER RFCS ORIGINAIS

## OAuth2

- Protocolo de Autorização

- Problema:
    - Estou no draw.io e quero salvar meus diagramas no Google Drive
    - Obviamente não quero informar pro draw.io meu email e senha do Google
    - Como resolver isso? Como dar acesso limitado pro draw.io para que ele possa APENAS salvar e buscar dados no meu Google Drive
    - Como posso revogar o acesso do draw.io?
    - O draw.io pode acessar pastas ou documentos privados?
    - Ele pode deletar todos os meus dados?
    - Permissão GRANULAR!

- Setup inicial
    - Cadastrar minha aplicacao no Google
        - ClientId
        - ClientSecret
        - Callback URI
        - Scopes

- Terminologia
    - Resource Owner
        - Eu, o dono das pastas/arquivos que estão no Drive
    - Client
        - Draw.io, a aplicação que está pedindo acesso ao meus recursos
    - Authorization Server
        - Servidor do Google que pergunta se quero deixar o draw.io acessar meus dados
        - Ele conhece meu email e minha senha (hash)
        - Ele conhece o draw.io (id + secret)
        - Ele conhece o Google Drive
        - Ele pode emitir tokens de acesso, usados para ler/salvar dados no Drive
    - Resource Server
        - Google Drive, onde estão meus recursos (pastas e arquivos)
    - Authorization Grant
        - Response Type
        - Response Mode
        - Code que prova que eu permiti o acesso do draw.io ao meu Drive
        - O que o draw.io espera receber no callback quando o usuário permitir seu acesso ao Drive
        - Usado no backend pelo draw.io para conseguir um JWT, que dá acesso ao Drive do usuário
    - Redirect URI
        - Callback URI (fica no draw.io)
        - URI que o draw.io usa para pegar o token gerado no Authorization Server quando o usuário permite
    - Access Token
        - JWT usado pelo draw.io para realizar a comunicação backend com o Drive (ler e salvar arquivos)
    - Scope
        - Permissões que o token possui
        - Ex: criar arquivos numa pasta / ler arquivos de uma pasta / deletar pasta
        - Elas sao mostradas na tela de consentimento, quando o auth server perguntar se quero dar permissao
    - State
        - Prova que eu iniciei e terminei o fluxo?
    - Nonce

    - Front Channel
        - Comunicacao via frontend (acontece no navegador) MENOS SEGURO
        - Onde ocorre todo o fluxo ate a obtencao do Authorization Grant (code)
    - Back Channel
        - Comunicacao via backend (server -> server) HTTPS / ALTAMENTE SEGURO





- PKCE (Proof Key for Code Exchange)

- Scopes
    - id, name, email...

- Flows
    - Authorization Code
    - 



## OpenID Connect

- Protocolo de Autenticação

- Extensão do OAuth2



## Cookies

- Set-Cookie Header


## Autenticacao

- Via ApiKey, JWT, Cookie...
- Logar com Google, GitHub, Facebook...
- Apresento credenciais e recebo um token/cookie/key de acesso

- Possui varios Schemas (Cookie, Bearer, OAuth)

## Referências

- OAuth 2.0 and OpenID Connect (in plain English) (https://youtu.be/996OiexHze0)
- O que você deveria saber sobre Oauth 2.0 e OpenID! (https://youtu.be/68azMcqPpyo)
- An Illustrated Guide to OAuth and OpenID Connect (https://youtu.be/t18YB3xDfXI)


