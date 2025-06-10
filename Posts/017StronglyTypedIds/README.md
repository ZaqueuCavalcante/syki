# Strongly Typed Ids

Dica sobre tipagem em C#

O sistema possui as seguintes entidades
    - Evento de Domínio
    - Comando
    - Lote de Comandos

DIAGRAMA COM TODAS AS RELAÇÕES ENTRE ESSAS 2 CLASSES

PRECISA FICAR CLARO QUE NÃO TER TIPAGEM FORTE COMPLICA AS COISAS


Um comando pode ter os seguintes ids:
    - Id do evento que gerou o comando
    - Id do comando que gerou o comando (utilizado quando um comando gera outro em seu handler)
    - Id do comando com erro que gerou o comando atual (utilizado quando o comando original está com erro e é reprocessado)
    - Id do lote que contém o comando

Todos esses ids são UUIDs, logo eh preciso muita atenção do desenvolvedor para não passar o id de um evento no lugar do de um comando/lote por exemplo.

Pensando nisso, podemos utilizar ids fortemente tipados para evitar esses erros. Dessa forma, todos os lugares que referenciem o id de um comando precisam possuir a tipagem correta (CommandId) ao invés da tipagem mais genérica (Guid).
