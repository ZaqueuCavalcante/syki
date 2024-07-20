# TODOS

- Events
- Inbox/Outbox Patterns
- RabbitMQ
- Hangfire (PostgreSQL)


## NOTAS

- O Academico precisa iniciar a turma
- Apos iniciar, o professor responsavel pode atribuir as notas de cada prova a cada aluno da turma
- O aluno pode ver suas notas em tempo real







❗Exceptions VS Result Pattern 🤔

Refatorei o tratamento de erros no meu projeto, antes utilizava Exceptions, agora estou usando o Result Pattern.

Existem diversas formas de implementar isso, no caso utilizei a biblioteca OneOf, que é bem simples e atende bem meu cenário atual.

Testei a performance do endpoint de cadastro de usuário, que retorna um BadRequest ao informar um email já utilizado:
    - Utilizei o K6 pros testes, com 10 VUs batendo no endpoint durante 30s
    - Exceptions: 92.654 reqs/s
    - Result Pattern: 99.414 reqs/s (aumento de 7,3% em relação ao uso de exceptions)

Nos testes de integração não teve diferença, rodam na mesma velocidade que antes.

O código fica mais verboso, pois agora preciso explicitar o retorno de cada método.

Fica mais claro de entender os caminhos possíveis de execução do código, isso facilita a elaboração dos casos de testes e reduz as chances de algum passar despercebido.


