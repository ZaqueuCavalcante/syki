# TRACK

- Necessidade de ordenacao (prioridade):
    - Campanhas
    - Listas de Contatos
    - Doadores

## Code

- Ordenar as campanhas (ativas) via front (Drag and Drop)
- Ordenar as Listas de Contatos dentro de uma campanha via front (Drag and Drop)
- Ordenar doadores:
    - Com base em criterios?
    - Dar opcao de ordenacao manual?

- Ao salvar uma ordenacao:
    - Emitimos evento
    - Comando roda e aplica as alteracoes

## Database

- Campanhas:
    - OrgId, Priority (Unique Index)

- Listas de Contatos:
    - OrgId, CampaignId, Priority (Unique Index)

- Doadores:
    - OrgId, CampaignId, ListId, Priority (Unique Index)


## Endpoints

- Buscar proximo doador pra atender:
    - Request => OrgId, AgentId
    - Faz um SELECT FOR UPDATE SKIP LOCKED + UPDATE inserindo o AgentId
    - Response => Doador prioritario
