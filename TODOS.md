# TODOS

- [V] Quando o academico criar turma, ja redirecionar pra pagina de detalhes
- [V] Tabs de (Aulas | Alunos) do detalhe de turma do academico deve abrir em coluna
- [V] Detalhes da turma do academico devem ficar em uma unica coluna quando abrir no celular
- [] Analisar padrao para as margins/paddings em todas as telas (deixar no layout?)
- [] Diminuir margins em relacao as bordas da tela
- [] Mudar icone de setup de MFA para algo de seguranca
- [] Substituir todos os dialogs pra usar componente e n C#
- [] Dialog de Nova Grade no celular ta cagalhado
- [] Dialog de Nova Oferta no celular ta cagalhado
- [] Dialog de Nova Turma no celular ta cagalhado

----------------------------------------------------------------------------------------------

- [] Dados corretos na tela de index do professor
- [] Adicionar skeletton na tela de detalhes de turma pra ver como fica o loading
- [] Essa tela ta toda bugada no mobile
- [] Adicionar opcao de selecionar todos e selecionar nenhum na aba de frequencia
- [] Tamanho do header na pagina de Agenda (celular)
- [] Criar pagina de notificacoes ou drawer, n exibir mais no dialog
- [] Botao de notificacoes fica exibindo o tip mesmo depois de fechar o dialog
- [] Botao de notificacoes fica zoado quando tem notificacao (numerozinho)
- [] Adicionar feature simples de diario de classe (selecionar aula + caixa de texto simples multiline)

----------------------------------------------------------------------------------------------

- [] Calcular media do aluno + coeficiente de rendimento (on the fly)
- [] No celular, exibir card de insight com icone -> nome -> value (coluna)
- [] Matricula habilita botao de editar mesmo quando n tem periodo academico in range
- [] Caso ja tenha se matriculado, exibir componente mas com opcao de editar
- [] Caso ja tenha se matriculado e o periodo academico ja tenha passado, exibir escolha como readonly (sem Editar, Cancelar nem Salvar)








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
    - Escolher disciplinas
    - Pode editar durante o período de matrícula
    - Apresentar sugestão de escolhas
    - N pode estourar as vagas

## Quadro de Horários
    - Tela fixa com as disciplinas do período
    - Dias e horários
    - Mostrar nome do professor? Primeiro nome?
    - Link pra uma página específica com coisas apenas da disciplina?

## Notas
    - Disciplina - Status - N1/N2/N3 - Final
    - Push notification (mobile) ao atualizar notas?
    - SignalR Hub?

## Financeiro
    - Parcelas
    - Boleto/Pix
    - Bolsas/Descontos
    - Atrasos/Juros



## Faltas
    - Disciplinas + quantas faltei
    - Limite de faltas
    - Justificar falta online?

## Planos de Aula
    - Todos os professores apresentam na aula 0
    - Deixar como fixo?
    - Ir marcando a cada aula dada

## Requerimentos
    - Dispensa de disciplinas
    - Declaração de Matrícula
    - Transferência Externa

## Calendário
    - Quando tem aula
    - Qual o horario das aulas
    - Eventos especiais
    - Semana de provas
    - Feriados
    - Datas limite para entrega de atividades
    - Periodo de matricula
    - Ferias
    - Porcentagem de quanto tempo ja passou
    - Dias totais, dias ja estudados, dias que faltam
    - Falta quanto ate a primeira prova, segunda e final
