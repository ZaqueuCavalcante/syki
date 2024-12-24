# Testes de Integração

Eu sempre prefiro ter mais testes de integração que de unidade.
O motivo é simples: acredito que quanto mais parecido com o cenário de produção for o teste, mais valor ele entrega.

Você mandaria pra produção um sistema que possui apenas testes de unidade? Talvez sim né, mas antes disso iria testar manualmente se tudo funciona em um ambiente de Staging.

E no caso do sistema ter apenas testes de integração? Sim né, e talvez ainda precise realizar testes em Staging, só que esses testes manuais...

Testes de integração:
- Quebram quando fluxos não triviais do código passam a não funcionar depois de uma alteração


- São 593 testes:
    - 289 de unidade, que rodam em 0.8s
    - 304 de integração, que rodam em 11.2s
- O setup dos testes de integração é feito apenas uma vez, ou seja, a API + Daemon + Postgres são os mesmos para todos os testes
- Não fico limpando o banco antes de cada teste, isso permite rodá-los em paralelo e deixa o cenário mais próximo do que ocorre em produção


- A API possui 70 endpoints, todos foram chamados pelo menos uma vez
- Ao total foram feitos 3.310 requests (TOP 10 a seguir)

    | Endpoint                          | Requests |
    |-----------------------------------|----------|
    | POST /login                       | 387      |
    | POST /users                       | 313      |
    | PUT  /users                       | 291      |
    | POST /academic/courses            | 255      |
    | POST /academic/academic-periods   | 252      |
    | POST /academic/course-curriculums | 228      |
    | POST /academic/course-offerings   | 218      |
    | POST /academic/classes            | 202      |
    | POST /reset-password-token        | 138      |
    | POST /reset-password              | 137      |



- O banco possui 34 tabelas
- Foram executados mais de 44.000 comandos SQL (TOP 3 a seguir)

    | Comando | Quantidade |
    |---------|------------|
    | INSERT  | 22.638     |
    | SELECT  | 18.017     |
    | UPDATE  | 1.664      |



- Foram emitidos e processados 1.055 eventos de domínio

    | Evento                                | Quantidade |
    |---------------------------------------|------------|
    | PendingUserRegisterCreatedDomainEvent | 294        |
    | ResetPasswordTokenCreatedDomainEvent  | 274        |
    | InstitutionCreatedDomainEvent         | 271        |
    | TeacherCreatedDomainEvent             | 100        |
    | StudentCreatedDomainEvent             | 77         |
    | ExamGradeNoteAddedDomainEvent         | 40         |


- Como cada evento gera apenas uma tarefa em background, também foram processadas 1.055 tarefas

    | Evento                                 | Quantidade |
    |----------------------------------------|------------|
    | SendUserRegisterEmailConfirmationTask  | 294        |
    | SendResetPasswordEmailTask             | 274        |
    | SeedInstitutionDataTask                | 271        |
    | LinkOldNotificationsTask               | 177        |
    | CreateNewExamGradeNoteNotificationTask | 40         |

