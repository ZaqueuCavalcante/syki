# Endpoints

## identity

| Método | Rota |
|---|---|
| GET | `/identity/social-login/challenge/{provider}` |
| POST | `/identity/social-login/google-one-tap` |

## students

| Método | Rota |
|---|---|
| POST | `/students/{studentId:int}/course-offerings` |

## teachers

| Método | Rota |
|---|---|
| GET | `/teachers/{id}/potential-campi` |
| GET | `/teachers/{id}/potential-disciplines` |
| PUT | `/teachers/{id}` |
| PUT | `/teachers/{id}/assign-campi` |
| PUT | `/teachers/{id}/assign-disciplines` |





