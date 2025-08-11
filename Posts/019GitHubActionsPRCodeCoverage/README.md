# GitHub Actions + Code Coverage

Adicionei métricas de cobertura de código ao projeto open-source que estou desenvolvendo.

Agora, toda vez que um PR é aberto no Syki, um workflow roda no GitHub Actions para buildar e testar o código enviado.

Ao final, 3 comentários são automaticamente adicionados ao PR:

- Sumário com o total de testes que passaram/falharam
- Tabela com os índices de cobertura (line/branch) de cada projeto
- Link para um relatório completo de cobertura do código submetido no PR

Utilizei o ReportGenerator (Daniel Palme) para gerar o relatório de cobertura e o GitHub Pages para hospedá-lo.






## Referências


https://seankilleen.com/2024/03/beautiful-net-test-reports-using-github-actions/


