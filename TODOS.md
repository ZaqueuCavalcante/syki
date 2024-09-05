# TODOS

- [] Bug do gutto (aluno faz matricula depois de uma chamada feita)
- [] Se n veio ngm pra aula, deve ser possivel salvar logo de cara tudo vazio?

----------------------------------------------------------------------------------------------

- [] Analisar padrao para as margins/paddings em todas as telas (deixar no layout?)
- [] Diminuir margins em relacao as bordas da tela (mobile)
- [] Dialogs com botoes margem direita e esquerda maiores (campi, curso, disciplina, oferta, turma, aluno)
- [] Substituir todos os dialogs pra usar componente e n C#
- [] Dialog de Nova Grade no celular ta cagalhado
- [] Dialog de Nova Oferta no celular ta cagalhado
- [] Dialog de Nova Turma no celular ta cagalhado

- [] Adicionar comportamento padrao nos botoes de salvar para ficar desativado + loading

- [] Setup de MFA n chama a API no celular ao digitar o 6 numeros (deixar colocar letra tbm)
- [] Remover 2FA da navbar
- [] Tela de profile de usuario com nome, email, reset de senha e 2FA setup
- [] Limite de faltas e nota minima (reprovacao)
- [] Cadastrar datas
    - Periodo de matricula
    - Inicio e fim do periodo academico
    - Feriados nacionais e regionais (n tem aula)
    - Eventos especiais (palestra, visita, simposio?)
    - Semana de provas (n tem aula)
    - Ferias

----------------------------------------------------------------------------------------------

- [] Dados incorretos na tela de index do professor
- [] Adicionar skeletton na tela de detalhes de turma pra ver como fica o loading
- [] Tela de detalhes de turma tela ta toda bugada no mobile (botoes todos/nenhum)
- [] Tamanho horizontal do header na pagina de Agenda (celular)
- [] Criar pagina de notificacoes, n exibir mais no dialog
- [] Botao de notificacoes fica exibindo o tip mesmo depois de fechar o dialog
- [] Botao de notificacoes fica zoado quando tem notificacao (numerozinho) / numero no meio do icon
- [] Adicionar feature simples de diario de classe
    - Listagem com aula + titulo + data-hora + status + icon
    - Detalhes num dialog

----------------------------------------------------------------------------------------------

- [] Adicionar frequencia como insight
- [] Calcular media do aluno + coeficiente de rendimento (on the fly)
- [] No celular, exibir card de insight com icone -> nome -> value (coluna)
- [] Matricula com pre-requisitos
- [] Matricula habilita botao de editar mesmo quando n tem periodo academico in range
- [] Caso ja tenha se matriculado, exibir componente mas com opcao de editar
- [] Caso ja tenha se matriculado e o periodo academico ja tenha passado, exibir escolha como readonly
- [] Abaixo da agenda, ter listagem dividida por mes
    - Eventos especiais
    - Semana de provas
    - Feriados
    - Periodo de matricula
    - Ferias
    - Porcentagem de quanto tempo ja passou
    - Dias totais, dias ja estudados, dias que faltam


## Dispensa e reprovacao de disciplina
- O q acontece aqui?

## Feature para iniciacao cientifica / pesquisa / extensao
- Analisar o que colocar aqui

## Feature para acompanhamento de monitoria
- Aluno quando monitor de uma disciplina/turma possui acesso as funcionalidades de monitor
- Professor consegue ver e interagir com os dados de monitoria (atas, assuntos, frequencias)
- Alunos dentro de uma turma conseguem acessar abas da monitoria

## Feature para montar quadro de horarios
- Ao iniciar o periodo, tenho X alunos que precisam se matricular
- Idealmente, cada oferta de curso vai ter as disciplinas a serem abertas no periodo
- Pode acontecer de alunos de diferentes cursos ou ofertas se matricularem na mesma turma
- Pode acontecer de nao abrir a turma ideal, mas mudar pra alguma outra disciplina
- Levar em conta as salas + horarios que estarao ocupadas
- Levar em conta o numero de vagas de uma turma
- Levar em conta carga-horaria caso a disciplina seja expremida ou alargada
- Um aluno n pode estar em duas turmas no mesmo dia e horario
- Um professor n pode estar em duas turmas no mesmo dia e horario
- Deixar cadastrar de boa, mas mostrar os conflitos pendentes (travar passo seguinte)
- Permitir de maneira inteligente a edicao e salvamento dos novos dias e horarios
- Cards q ficam um em cima do outro em vermelho claro (arrastar e editar on fly)
- Permitir que professores possam ver preview do horario no sistema


## Front

- xs sm md lg xl
- Font Ultra #594AE2

## Events

- [] Adicionar eventos ao sistema + avoid polling do banco
- [] Usar interceptors para salvar os eventos na mesma transacao do EF
- [] Criar mapping para bindig fortemente tipado de eventos e tasks
- [] O processamento dos eventos deve ser feito de maneira unica e sequencial
- [] Caso um falhe, marcar como falha e seguir com os demais
- [] Criar fila unica pra executar as tasks (depois pensar em ter uma pros emails e outros pras demais coisas...)

https://www.graymatterdeveloper.com/2019/12/02/listening-events-postgresql/index.html

# Students Features

## Dados/Perfil
    - Nome, nascimento, matricula, situação
    - Filiação (pai e mãe)
    - Contato (celular, email)
    - Endereço (CEP, cidade, estado)
    - Acompanhamento do curso
        - Quanto ja conclui / quanto falta
        - Monitorias? Aluno pode receber acesso a "features de monitor"?
        - Horas necessarias fora as disciplinas
        - Iniciacao cientifica

## Grade Curricular
    - Dados consolidados/porcentagens:
        - Situação
        - Média global
        - Coeficiente de rendimento
        - Disciplinas ja cursadas/cursando/pendentes
    - Listagem separada por periodo:
        - Nome + situação
        - Optativas? Eletiva?

## Matrícula Acadêmica
    - Apresentar sugestão de escolhas
    - N pode estourar as vagas

## Notas
    - Push notification (mobile) ao atualizar notas?
    - SignalR Hub?

## Financeiro
    - Parcelas
    - Boleto/Pix
    - Bolsas/Descontos
    - Atrasos/Juros

## Planos de Aula
    - Todos os professores apresentam na aula 0
    - Deixar como fixo?
    - Ir marcando a cada aula dada

## Requerimentos
    - Dispensa de disciplinas
    - Declaração de Matrícula
    - Transferência Externa

## Calendário
    - Eventos especiais
    - Semana de provas
    - Feriados
    - Periodo de matricula
    - Ferias
    - Porcentagem de quanto tempo ja passou
    - Dias totais, dias ja estudados, dias que faltam

## Atividades
    - Listagem de pendentes + data limite para entrega
