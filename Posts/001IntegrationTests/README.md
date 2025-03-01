# Integração > Unidade

Prefiro ter mais testes de integração que de unidade.

O motivo é simples: acredito que quanto mais parecido com o cenário de produção for o teste, mais valor ele entrega.

Testes de integração:
- Alertam quando fluxos não triviais do código passam a não funcionar depois de uma alteração
- Servem como uma documentação viva das funcionalidades do sistema
- Possibilitam a reprodução de bugs difíceis e garantem que eles não voltem após serem corrigidos
- São fundamentais para a manutenção de um ritmo sustentável de desenvolvimento

Vou utilizar o Syki como projeto de exemplo:
- Ele possui 538 testes, sendo 282 de unidade e 256 de integração
- Sua arquitetura é baseada em 3 componentes principais: API, Daemon (processa eventos e tarefas em background) e PostgreSQL
- O setup dos testes de integração é feito apenas uma vez, ou seja, a API + Daemon + PostgreSQL são os mesmos para todos os testes
- Não fico limpando o banco antes de cada teste, isso permite rodá-los em paralelo e deixa o cenário mais próximo do que ocorre em produção

Resultado dos testes:
- A API possui 70 endpoints, todos foram chamados pelo menos uma vez, totalizando 3.310 requests
- O Daemon realizou o processamento de 1.055 eventos de domínio e de 1.055 tarefas em background
- O banco possui 34 tabelas e foram executados mais de 44.000 comandos SQL (INSERT, SELECT, UPDATE...)

Abaixo mostro um dos testes, onde:
- Chamo a API para criar um registro de usuário pendente (+ evento PendingUserRegisterCreatedDomainEvent)
- Aguardo o Daemon processar o evento de domínio gerado (que enfilera a tarefa SendUserRegisterEmailConfirmationCommand)
- Aguardo o Daemon processar a tarefa de enviar o email de confirmação de registro
- Valido que o email foi "enviado" pela minha implementação fake usada nos testes
