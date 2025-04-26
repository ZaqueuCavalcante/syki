# TODOS

Atividades, notas e frequências

## Aluno
    - Arrumar tela de notas
    - Arrumar tela de frequências
    - Testes automatizados

## Professor
    - Trazer gráficos para análise da turma no geral

## Acadêmico
    - Instituição
    - Campus
    - Curso
    - Turma
    - Aluno

# Atividades

- Professor pode criar atividade
- Uma prova nao possui entrega (link/documento), o professor deve simplesmente atribuir a nota

- O peso da atividade dentro da nota deve ser de 0 a 100
    - 1 só atividade -> peso qualquer
    - 2 atividades -> soma dos pesos sempre <= 100
    - 3 atividades -> soma dos pesos sempre <= 100

- Verificar retorno correto do endpoint que retorna o peso restante das notas
    - Sem atividades -> tudo 100
    - Com uma atividade -> peso menor que 100
    - Com atividades em cada nota -> pesos variam

- Retorno correto dos detalhes da atividade

- Retorno correto da lista de entregas da atividade

- Professor pode atribuir nota (0 1 10) à cada entrega

---------------------------------------------------------------------------------------------------

- Aluno pode ver as atividades de cada turma

- Aluno pode submeter link de entrega pra uma atividade
