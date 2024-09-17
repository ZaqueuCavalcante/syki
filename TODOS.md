# TODOS

- [V] Ao criar uma Turma, ela vai comecar com o status de Pre-Matricula

- [V] Deve ser possivel liberar para matricula varias turmas ao mesmo tempo
    - Todas precisam ter o status de Pre-Matricula e ser do mesmo periodo academico
    - O periodo de matricula vinculado deve conter a data do request (date >= start && date <= end)

- [ ] Uma vez liberada a matricula, o status deve ser Em matricula (ate dia final do periodo de matricula)
    - Se o periodo de matricula acabar e a turma ainda estiver Em-Matricula, deve exibir o status de Aguardando Inicio
    - Nesse momento, caso o periodo de matricula seja estendido, o sistema deve continuar funcional

- [ ] Deve ser possivel iniciar varias turmas ao mesmo tempo
    - Todas precisam ter o status de Aguardando Inicio e ser do mesmo periodo

- [ ] O aluno so pode ver a turma quando ela tiver o status de Em Matricula


- [] Fluxo de Iniciar Turma
- [] Arrumar tela de matricula do aluno
- [] Corrigir dados de index de Aluno e Professor
- [] Mudar seed para ter alunos ja matriculados e outros ainda em periodo de matricula
- [] Revisar todos os testes
- [] Documentacao do codigo e da API
- [] Deploy na Azure com Terraform
- [] Limite de faltas e nota minima (reprovacao)




- [] Feature flags
    - API retorna header com as features habilitadas [541, 684, 1844]
    - Front salva lista no LocalStorage
    - Usar o AuthorizeView para determinar se pode ver ou nao coisa relacionadas a features
    - Isso n deve ser usado para coisas relacionadas a autenticacao / autorizacao
    - Em todos os requests o que veio no header eh salvo no LocalStorage (sobrescrever msm)
    - Ao mudar os headers na API, o front deve se adaptar automaticamente
    - Isso pode ser usado para forcar o reload depois de um deploy do front, basta retornar o hash do ultimo commit do codigo








- [] Bug do gutto (aluno faz matricula depois de uma chamada feita)

----------------------------------------------------------------------------------------------

- [] Tela de profile de usuario com nome, email, reset de senha e 2FA setup
- [] Cadastrar datas
    - Periodo de matricula
    - Inicio e fim do periodo academico
    - Feriados nacionais e regionais (n tem aula)
    - Eventos especiais (palestra, visita, simposio?)
    - Semana de provas (n tem aula)
    - Ferias

----------------------------------------------------------------------------------------------

- [] Adicionar feature simples de diario de classe
    - Listagem com aula + titulo + data-hora + status + icon
    - Detalhes num dialog
    - Deve aparecer pro aluno

----------------------------------------------------------------------------------------------

- [] Adicionar frequencia como insight
- [] Calcular media do aluno + coeficiente de rendimento (on the fly)
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
