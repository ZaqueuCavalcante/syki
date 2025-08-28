# Vertical Slices Arch

Arquitetura simples e efetiva

A ideia principal da Vertical Slice Architecture (VSA) é organizar seu código por funcionalidade e não por camadas.

Como exemplo, temos a funcionalidade de criar um curso, mostrada na imagem abaixo.

Essa controller possui:
    - Autorização (apenas usuários com role=Academic podem acessar)
    - Rota (POST /academic/courses)
    - Documentação + exemplos de requests, responses e erros
    - Chamada do CreateCourseService que executa lógica de negócio

<p align="center">
  <img src="https://github.com/ZaqueuCavalcante/syki/blob/master/Posts/021VerticalSlicesArch/00_endpoint.png?raw=true" style="display: block; margin: 0 auto" />
</p>

O CreateCourseService possui:
    - Validação do input usando FluentValidation
    - Uso de Result Pattern para retornar sucesso ou erro
    - SykiDbContext para operações no banco de dados
    - Mapper para converter a entidade de domínio em DTO de resposta

<p align="center">
  <img src="https://github.com/ZaqueuCavalcante/syki/blob/master/Posts/021VerticalSlicesArch/01_service.png?raw=true" style="display: block; margin: 0 auto" />
</p>

<p align="center">
  <img src="https://github.com/ZaqueuCavalcante/syki/blob/master/Posts/021VerticalSlicesArch/02_mapper.png?raw=true" style="display: block; margin: 0 auto" />
</p>

Os objetos de input e output do endpoint possuem um método que retorna exemplos com valores, usados na documentação.

<p align="center">
  <img src="https://github.com/ZaqueuCavalcante/syki/blob/master/Posts/021VerticalSlicesArch/03_examples.png?raw=true" style="display: block; margin: 0 auto" />
</p>

<p align="center">
  <img src="https://github.com/ZaqueuCavalcante/syki/blob/master/Posts/021VerticalSlicesArch/04_docs.png?raw=true" style="display: block; margin: 0 auto" />
</p>

Por fim, é extremamente fácil entender e encontrar as coisas no projeto, pois todos os arquivos relacionados com uma determinada funcionalidade possuem o mesmo prefixo, inclusive os de testes automatizados.

<p align="center">
  <img src="https://github.com/ZaqueuCavalcante/syki/blob/master/Posts/021VerticalSlicesArch/05_files.png?raw=true" style="display: block; margin: 0 auto" />
</p>
