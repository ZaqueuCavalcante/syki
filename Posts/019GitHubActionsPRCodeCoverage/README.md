# CI/CD + GitHub Actions + Code Coverage

Adicionei métricas de cobertura de código ao projeto open-source que estou desenvolvendo.

Agora, toda vez que um PR é aberto no Syki, um workflow roda no GitHub Actions para buildar e testar o código enviado.

Ao final, 3 comentários são automaticamente adicionados ao PR:

- Sumário com o total de testes que passaram/falharam
- Tabela com os índices de cobertura (line/branch) de cada projeto
- Link para um relatório completo de cobertura do código submetido no PR

Os testes que falham são agrupados em uma página específica, cujo link é inserido junto com o sumário no primeiro comentário.

Utilizei o ReportGenerator para gerar o relatório de cobertura e o GitHub Pages para hospedá-lo. Com ele é possível visualizar a cobertura do código a nível de classe/método/linha.

Quando o PR é mergeado, um outro workflow roda para buildar, testar, 






## Referências

- https://seankilleen.com/2024/03/beautiful-net-test-reports-using-github-actions
- https://github.com/danielpalme/ReportGenerator




