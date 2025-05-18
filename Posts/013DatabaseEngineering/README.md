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
    - O que acontece quando tenho mais de uma transação ao mesmo tempo?
    - Uma transação pode ver o que a outra está fazendo?
    - Read Phenomena
        - Dirty Reads
        - Non-Repeatable Reads
        - Phantom Reads
        - Lost Updates
    - Isolation Levels
        - Read Uncommitted (no isolation)
        - Read Committed (transactions only sees committed changes)
        - Repeatable Read (row lock inside transaction)
        - Snapshot (same data since start)
        - Serializable (one after one transactions)
    - Locks
        - Pessimistic
        - Optimistic

- Consistency
    - 

- Durability





