# Database Engineering

Learn ACID, Indexing, Partitioning, Sharding, Concurrency control, Replication, DB Engines, Best Practices and More!

## ACID + Transactions

- Transaction
    - Um monte e comandos SQL, tratados como uma coisa só (unit of work)
    - BEGIN -> ... -> COMMIT
    - ERROR -> ROLLBACK
    - SEMPRE tem uma transaction, seja automatica ou definida pelo usuario

- Atomicity
    - Tudo funciona ou nada é commitado
    - Falha -> rollback

- Isolation
    - O que acontece quando tenho mais de uma transacao ao mesmo tempo?
    - Uma transaction pode ver o que a outra esta fazendo?
    - Isolation Levels
        - Dirty Reads
        - Non-Repeatable Reads
        - Phantom Reads
        - Lost Updates
        - 

- Consistency

- Durability





