# 🪝Webhooks na prática

Adicionei suporte a Webhooks no projeto open-source que estou desenvolvendo!

O ([Syki](https://github.com/ZaqueuCavalcante/syki)) é um sistema de gerenciamento de instituições de ensino que pode ser usado por gestores, professores e alunos.

Ele é capaz de emitir eventos quando certas ações são executadas pelos usuários, como por exemplo:
    - Um novo aluno é cadastrado no sistema
    - Uma nova atividade é publicada pelo professor de uma turma

Digamos que seja preciso integrar o Syki à outro serviço XYZ, que vai executar um determinado processamento toda vez que um desses eventos ocorrer.

Talvez a maneira mais simples de realizar essa integração seja através de pooling: a aplicação XYZ fica, periodicamente, chamando a Api do Syki para buscar novos eventos. Isso é simples de implementar, mas também é custoso e ineficiente, pois a maioria das chamadas não vai encontrar dados novos para serem processados, sobrecarregando a Api do Syki desnecessariamente.

Um outro jeito de abordar esse problema é através do uso de 𝘄𝗲𝗯𝗵𝗼𝗼𝗸𝘀: o serviço XYZ cadastra uma url (+ ApiKey) no Syki e escolhe quais eventos quer receber através dela. Dessa forma, toda vez que um dos eventos escolhidos ocorrer, o Syki monta um payload e chama a aplicação XYZ com os dados, em uma integração rápida e eficiente.

Obviamente essa chamada para o endpoint na aplicação XYZ pode falhar, por isso implementei também uma política de retry exponencial: caso a primeira chamada falhe, o Syki vai tentar novamente após 1 min. Caso falhe, tenta novamente após 5 min. Caso falhe novamente, tenta pela última vez após 30 min.

Ainda é possível reprocessar uma chamada manualmente via tela, para o caso onde todas as retentativas automáticas falharam ou mesmo em caso de reconciliação de dados, por exemplo.

O GIF abaixo mostra essa integração acontecendo quando um novo aluno é cadastrado no sistema.

#webhook #dotnet #api #tests #postgres #opensource
