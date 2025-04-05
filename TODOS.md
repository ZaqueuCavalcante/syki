# TODOS


## Atividades V0

- ✅ O professor deve ter acesso rápido à todas as suas turmas atuais, através da SideBar esquerda

- ✅ Analisar pq a tela de turma do professor eh tão lenta

- Dentro de uma turma, deve ser possível criar uma atividade
    - O Dialog só deve ser fechado ao clicar no X ou no Cancelar
    - A soma total dos pesos das atividades já existentes deve ser levada em conta no peso máximo da nova atividade




- Na listagem de atividades
    - Ordenar por CreatedAt DESC
    - Filtros por Tipo e Nota
    - Adicionar coluna Criação
    - Adicionar botão para abrir Dialog de edição
    - Adicionar coluna de Entregas
        - Progress com 5/26
        - Usar Tooltip informando a %?
        - Abaixo de 70% fica vermelha, igual ou acima fica amarela, 100% fica verde

- Na tela de detalhes da atividade
    - Adicionar um Card com todos os dados da atividade
    - Usar mesmo estilo do Dialog de criação
    - Deve ser possível ocultar/mostrar o Card

- Listagem com todas as entregas
    - Filtro: Todos Pendentes Corrigidos (usar Toggle Group)
    - Colunas: Aluno, Data da entrega, Status (pendente de correção (alerta amarelo), corrigido (check verde)), Nota
    - Clicar no detalhe, deve abrir Drawer lateral com todos os links enviados pelo aluno
    - Professor pode atribuir nota à entrega e alterar a nota quando quiser (mesmo endpoint, criar caso null...)



- Aluno deve poder ver todas as suas turmas na SideBar esquerda

- Ao entrar em uma turma, listar todas as atividades ordenadas por CreatedAt DESC

- Filtro: Todos Pendentes Entregues

- Drawer na direita para adicionar vários links e enviar

- Analisar criação de timeline



























- Aplicar filtros na query-string
    - Quando voltar pra pagina ela mantem os filtros
    - Ao compartilhar um link, nao perdemos o filtro

- Plano de aulas (conteudos)
- Melhorar design da paginação das tabelas

- DbConnectionFactory
    - NpgsqlDataSource
    - DROP new NpgsqlConnection

- HttpClientFactory
- Pool de conexoes com o banco de dados
- RabbitMQ use case (project)
- Redis use case (project)

- Use Breadcrumbs
- Use db migrations

- Aluno pode gerar um historico de disciplinas + notas (PDF)
- Aluno pode gerar comprovante de matricula
- Alertas pro aluno, estilo nubank-app
- Disciplinas com pre-requisitos (bloqueio de matricula)
- Disciplinas extra-curriculares
- Emissao de carteira de estudante (ou integracao com providers)
- Plano de aula / conteudo de cada aula
- Descricao de conteudo de uma disciplina / bibliografia (unica dentro da disciplina da grade)



# Notas

Cada aluno possui 3 notas em cada turma: N1, N2 e N3





# Atividades

- O professor pode ver todas as atividades da turma
    - Filtros
        - Nota (N1 | N2 | N3)
        - Tipo (Trabalho | Apresentação | Prova)
    - Coluna de status (Pendente | Publicada | Finalizada)
    - Coluna com porcentagem de entregas
    - Coluna com porcentagem de notas atribuídas

- Ao clicar em "Nova Atividade", abrir dialog

- Endpoint: dado um ClassId, retornar lista com pesos restantes por nota
    - Ex: [(N1, 50), (N2, 80), (N3, 100)]

- O professor da turma pode criar atividades
    - Uma atividade pode ser do tipo trabalho, apresentação ou prova
    - Cada nota possui título e descrição
    - Cada nota possui data e hora limites para entrega
    - Cada atividade deve ser vinculada à uma nota, possuindo um peso relativo à ela
        - Ex: um trabalho de pesquisa que possui peso 20% na nota N1
        - Buscar lista com pesos restantes por nota na API
    - A soma dos pesos das atividades dentro de uma nota deve sempre ser 100%
        - Ex: a nota N1 é formada por um trabalho (20%) + apresentação (30%) + prova (50%)
    - É possível alterar o peso de uma atividade, desde que a soma dos pesos na nota continue 100%
        - Ex: usando o exemplo anterior, mudamos o peso da prova para 40% e adicionamos um novo trabalho de peso 10%

- A atividade é criada como pendente e a listagem é recarregada

- Na listagem o professor pode clicar em uma atividade e ver seus detalhes (abrir nova página)
    - Exibir todas as informações da atividade
    - Botão para editar (mesmo form do dialog da criação)
    - Listagem com todas as entregas feitas pelos alunos


- Alunos podem ver as atividades de cada turma




- O professor da turma pode atribuir valores às atividades dos alunos
    - Toda entrega que o aluno fizer deve ser imutável (timeline cima->baixo)
    - Quando a atividade do aluno for entregue após o limite, alertar o professor com um chip
    - Cada atividade do aluno possui um valor de 0 a 10


## Entregas

- Links ou arquivos pdf
- Permitir a entrega mesmo depois da data limite
    - Mostrar na timeline e num chip pro professor

## Notificações

- Nova atividade publicada
- Novo comentário
- Citação
- Data limite de entrega (amanhã / hoje)

## Comentários

- Comentários Aluno-Professor / Grupo-Professor
    - Na timeline de entrega de uma atividade, professor e aluno podem fazer comentários

## Grupos

- Após criada a atividade, é possível alterar para que ela seja em grupo
    - O professor pode criar um grupo por vez
    - Cada grupo deve possuir nome e quantidade de alunos
    - Deve ser possível adicionar/remover alunos do grupo
        - Ele pode sortear alunos no grupo (apenas os sem grupo)
    - Ele pode alterar os grupos enquanto a atividade estiver publicada

- Em uma atividade em grupo, todos os alunos devem ver a mesma atividade/grupo
    - Apenas um integrante do grupo deve submeter entregas
    - Todos os integrantes podem ver as entregas e fazer comentários

- Nota do grupo e nota dos alunos
    - Deve ser possível atribuir uma nota ao grupo
    - Todos os alunos do grupo recebem essa nota
    - O professor pode alterar a nota de um aluno para uma diferente da do grupo
