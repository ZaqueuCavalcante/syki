# Features

## Notas

- Uma faculdade pode definir qual a nota media para ser aprovado
    - Essa nota sera usada globalmente para todos os cursos e disciplinas
    - Salvar na propria faculdade, pensar numa tabela especifica mais tarde
    - Valor default = 7

- As notas vao servir para:
    - Determinar se um aluno foi aprovado ou nao numa disciplina que cursou
    - Determinar a media da turma numa disciplina
    - Determinar a media do curso naquela disciplina

- Avaliacoes:
    - Prova 1 + Prova 2
    - Prova Final

## Grades

- [X] Nao deve criar uma grade sem vinculo com curso
- [X] Nao deve criar uma grade com um curso que n existe
- [X] Nao deve criar uma grade com curso de outra faculdade
- [X] Nao deve criar uma grade vinculando disciplinas que nao sao do curso escolhido
- [X] Nao deve criar uma grade vinculando disciplinas de outra faculdade
- [X] A carga horaria da disciplina dentro de uma grade pode ser diferente da CH default do curso

- [X] Deve criar uma grade mesmo sem disciplinas vinculadas, apenas com curso
- [X] Caso venham disciplinas repetidas (ids), retornar erro
- [X] Caso venham disciplinas q n existem, retornar erro

- [X] Ao buscar todas as grades:
    - Deve trazer apenas daquela faculdade
    - Deve trazer ordenado por nome