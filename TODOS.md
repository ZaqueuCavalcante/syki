# TODOS

- Feature para enviar email pros alunos toda vez que for postada uma nova atividade
- Plano de aulas (conteudos)
- Melhorar design da AppBar (rounded)
- Melhorar design da paginação das tabelas






- HttpClientFactory
- DbConnectionFactory
- Pool de conexoes com o banco de dados
- RabbitMQ use case (project)
- Redis use case (project)







# Desafio Banco Inter

## Dígito Único

Definimos um dígito único de um inteiro usando as seguintes regras:

- Dado um inteiro x, precisamos encontrar seu dígito único:
    - Se x possui apenas um dígito, seu dígito único é x
    - Caso contrário, seu dígito único é igual ao dígito único da soma dos dígitos de x

Por exemplo, o dígito único de 9875 será calculado como:

DigitoUnico(9875) -> 9+8+7+5 -> 29
DigitoUnico(29) -> 2+9 -> 11
DigitoUnico(11) -> 1+1 -> 2
DigitoUnico(2) -> 2
