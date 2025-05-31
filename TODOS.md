# CREATE ACCOUNT AND LOGIN WITH EMAIL

## Cadastro

- Usuario informa seu email
- Enviamos um link pro email informado, contendo um token valido por 1 min
- Caso clicado dentro de 1 min, ja cria o usuario vinculando email e loga o usuario direto na conta
- Gerar user name como tudo que vem antes do @ do email

## Login

- Usuario informa seu email
- Enviamos um link pro email informado, contendo um token valido por 1 min
- Caso clicado dentro de 1 min, loga o usuario direto na conta

## Perfil e senha

- Ja logado, o usuario pode mudar seu user name
- Eh preciso ter senha? Caso ele queira acessar de outro computador sem precisar logar no seu email

---------------------------------------------------------------------------------------------------

# WebHooks

- Adicionar no README

- Porque usar webhooks
    - Uso eficiente de recursos (sem polling)
    - Real-time updates

- Desvantagens
    - Erros na hora da notificacao
    - Possivel perca de updates
    - Necessidade de mecanismos de retry e de consciliacao de mensagens

- A URL informada pelo cliente deve ser indepotente
    - Facilita nos casos de retry e consciliacao


- Academic pode escolher evento e vincular uma URL pra receber o POST com os dados

- Precisa de algum metodo de autenticacao
    - API Key

- Um mesmo evento pode ter mais de uma URL inscrita

- Eventos possiveis
    - StudentCreatedDomainEvent
    - ClassActivityCreatedDomainEvent

- Retry caso a chamada falhe
    - Configurar quantas chamadas devem ser feitas
    - Qual o intervalo entre as chamadas

- Listar todas as chamadas
    - Sucessos
    - Erros
    - Faltam retry
    - Range de datas

- Dados sobre as chamadas
    - Quando foi feita
    - Qual o payload
    - Quanto tempo levou
    - Quantas tentativas foram feitas
    - Quando o evento acorreu (delta entre isso e a chamada)


