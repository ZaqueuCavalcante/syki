# TODOS

- [] Adicionar mais alunos no seed (15 ADS e 10 Direito)
- [] Analisar padrao para as margins/paddings em todas as telas (deixar no layout?)
- [] Diminuir margins em relacao as bordas da tela (mobile)
- [] Dialogs com botoes margem direita e esquerda maiores (campi, curso, disciplina, oferta, turma, aluno)
- [] Substituir todos os dialogs pra usar componente e n C#
- [] Dialog de Nova Grade no celular ta cagalhado
- [] Dialog de Nova Oferta no celular ta cagalhado
- [] Dialog de Nova Turma no celular ta cagalhado
- [] Adicionar frequencia por aula e global nos detalhes de turma [Tab Aulas]
- [] Adicionar frequencia por aluno e global nos detalhes de turma [Tab Alunos]
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
