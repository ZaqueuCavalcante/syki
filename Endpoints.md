# Endpoints

## classes

| Método | Rota |
|---|---|
| GET | `/classes` |
| POST | `/classes` |

## course-curriculums

| Método | Rota |
|---|---|
| GET | `/course-curriculums` |
| GET | `/course-curriculums/{id}` |
| POST | `/course-curriculums` |
| PUT | `/course-curriculums` |

## course-offerings

| Método | Rota |
|---|---|
| GET | `/course-offerings` |
| POST | `/course-offerings` |

## courses

| Método | Rota |
|---|---|
| GET | `/courses` |
| GET | `/courses/{id}` |
| GET | `/courses/{id}/disciplines` |
| GET | `/courses/{id}/potential-disciplines` |
| POST | `/courses` |
| POST | `/courses/disciplines` |
| PUT | `/courses` |
| DELETE | `/courses/disciplines` |

## disciplines

| Método | Rota |
|---|---|
| GET | `/disciplines` |
| GET | `/disciplines/{id}` |
| GET | `/disciplines/{id}/potential-courses` |
| POST | `/disciplines` |
| POST | `/disciplines/courses` |
| PUT | `/disciplines` |
| DELETE | `/disciplines/courses` |

## home

| Método | Rota |
|---|---|
| GET | `/home/stats` |

## identity

| Método | Rota |
|---|---|
| GET | `/identity/2fa-key` |
| GET | `/identity/permissions` |
| GET | `/identity/roles` |
| GET | `/identity/roles/{id}` |
| GET | `/identity/social-login/challenge/{provider}` |
| GET | `/identity/social-login/check-availability` |
| GET | `/identity/sso/configuration` |
| POST | `/identity/2fa-login` |
| POST | `/identity/2fa-setup` |
| POST | `/identity/email-password-login` |
| POST | `/identity/logout` |
| POST | `/identity/magic-link-login` |
| POST | `/identity/reset-password` |
| POST | `/identity/reset-password-token` |
| POST | `/identity/roles` |
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

## periods

| Método | Rota |
|---|---|
| GET | `/periods/academic` |
| POST | `/periods/academic` |

## students

| Método | Rota |
|---|---|
| GET | `/students` |
| GET | `/students/{studentId:int}` |
| POST | `/students` |
| POST | `/students/{studentId:int}/course-offerings` |

## teachers

| Método | Rota |
|---|---|
| GET | `/teachers` |
| GET | `/teachers/{id}` |
| GET | `/teachers/{id}/potential-campi` |
| GET | `/teachers/{id}/potential-disciplines` |
| POST | `/teachers` |
| PUT | `/teachers/{id}` |
| PUT | `/teachers/{id}/assign-campi` |
| PUT | `/teachers/{id}/assign-disciplines` |

## users

| Método | Rota |
|---|---|
| GET | `/users/account` |
| POST | `/users/register` |
| PUT | `/users/account` |

## version

| Método | Rota |
|---|---|
| GET | `/version` |

## webhooks

| Método | Rota |
|---|---|
| GET | `/webhooks/subscriptions` |
| GET | `/webhooks/subscriptions/{id}` |
| POST | `/webhooks/subscriptions` |
| PUT | `/webhooks/subscriptions` |
