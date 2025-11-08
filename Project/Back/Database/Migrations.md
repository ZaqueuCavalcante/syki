# Migrations

O Admin precisa acessar dois bancos de dados para funcionar corretamente:

- Intelligence (BackDbContext)
- Exato Web (WebDbContext)

O banco de Intelligence possui vários schemas, como public, faturamento, ibge e exato (criado para suportar as novas funcionalidades do Admin).

Cada schema está mapeado em um arquivo diferente, mas todos fazem parte do mesmo DbContext no final (utilizando partial class):

- BackDbContext: DbContext real, que você utiliza para acesso ao banco do Intelligence.
- PublicDbContext: onde ficam mapeadas as tabelas do schema public.
- FaturamentoDbContext: onde ficam mapeadas as tabelas do schema faturamento.
- ExatoDbContext: onde ficam mapeadas as tabelas do schema exato.

Pro banco do Exato Web existe apenas um schema, mapeado num único DbContext, o WebDbContext.

Não crie migrations contendo alterações de schema e mapeamento de objetos que já existem ao mesmo tempo, prefira separar em migrations específicas.

## Comandos

### Gerar uma nova migration

```
dotnet ef migrations add ChangeFranquiaMinimaToDecimal -c BackDbContext
```

### Gerar todo o SQL mapeado em um DbContext

```
dotnet ef dbcontext script -o db_context_script.sql -c BackDbContext
```

### Gerar todo o SQL mapeado nas migrations

```
dotnet ef migrations script -o migrations_script.sql -c BackDbContext
```

### Gerar apenas o SQL mapeado nas migrations seguintes a informada

```
dotnet ef migrations script 20251023224622_FixTimestampz -o migrations_script.sql -c BackDbContext
```

### Gerar apenas o SQL mapeado nas migrations entre as informadas

```
dotnet ef migrations script 20250925013848_IntelligenceBootstrap 20250925014337_ExatoBootstrap -o migrations_script.sql -c BackDbContext
```
