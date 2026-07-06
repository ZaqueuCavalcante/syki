# Endpoints

## home

| Método | Rota |
|---|---|
| GET | `/home/stats` |

## identity

| Método | Rota |
|---|---|
| GET | `/identity/permissions` |
| GET | `/identity/roles/{id}` |
| GET | `/identity/social-login/challenge/{provider}` |
| GET | `/identity/social-login/check-availability` |
| GET | `/identity/sso/configuration` |
| POST | `/identity/social-login/google-one-tap` |
| POST | `/identity/sso/check-availability` |
| POST | `/identity/sso/configurations` |
| PUT | `/identity/roles` |
| PUT | `/identity/sso/configurations/{id:guid}` |

## notifications

| Método | Rota |
|---|---|
| GET | `/notifications` |
| GET | `/notifications/unread-count` |
| PUT | `/notifications/mark-as-viewed` |

## students

| Método | Rota |
|---|---|
| GET | `/students` |
| GET | `/students/{studentId:int}` |
| POST | `/students/{studentId:int}/course-offerings` |

## teachers

| Método | Rota |
|---|---|
| GET | `/teachers` |
| GET | `/teachers/{id}` |
| GET | `/teachers/{id}/potential-campi` |
| GET | `/teachers/{id}/potential-disciplines` |
| PUT | `/teachers/{id}` |
| PUT | `/teachers/{id}/assign-campi` |
| PUT | `/teachers/{id}/assign-disciplines` |

## webhooks

| Método | Rota |
|---|---|
| GET | `/webhooks/subscriptions` |
| GET | `/webhooks/subscriptions/{id}` |
| POST | `/webhooks/subscriptions` |
| PUT | `/webhooks/subscriptions` |
