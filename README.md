# SYKI - Sistema de Gerenciamento Acadêmico

[![Tests](https://github.com/ZaqueuCavalcante/syki/actions/workflows/railway.yml/badge.svg)](https://github.com/ZaqueuCavalcante/syki/actions/workflows/railway.yml)

## O que é este projeto?

É um sistema open-source para gerenciamento educacional, que pode ser usado por gestores, professores e alunos.

Por ser multi-tenant, suporta a operação de diversas instituições de ensino ao mesmo tempo.

## Quais as funcionalidades?

Inicialmente, o sistema só possui o módulo Acadêmico.

A ideia é adicionar mais módulos no futuro, como os de Biblioteca, Financeiro, Almoxarifado...

Também pretendo criar integrações com outros sistemas, como o Moodle e o Google Classroom.

O módulo Acadêmico possui diversas funcionalidades para agilizar sua gestão:

- ✅ Organize os campus, cursos e disciplinas como os fundamentos da sua estrutura acadêmica
- ✅ Crie novas grades curriculares a cada semestre ou reutilize grades em novas ofertas de curso
- ✅ Dê autonomia para seus professores realizarem chamadas e avaliações de maneira simples e efetiva
- ✅ Acompanhe o andamento de cada turma em detalhes, tanto das aulas quanto dos alunos
- ✅ Seus alunos terão acesso direto a agenda, notas, frequência e matrícula
- ✅ Envie notificações para todos os usuários dentro do próprio sistema

## Qual a tech stack?

- Backend em C#/.NET
- Frontend em C#/Blazor Webassembly
- Banco PostgreSQL
- Docker para buildar back e front
- Deploy no Railway

<img src="./Docs/images/docker-compose.png" width="600" height="300" style="border-radius: 6px">

## Como rodar?

- Clone o projeto pra sua máquina

- Para subir banco + back + front, rode o comando: `docker-compose up`

- Para rebuildar caso tenha alguma alteração no código: `docker-compose build --no-cache`

- O back vai subir na porta 5001 e o front na 5002

## Overview

Se cadastre em https://app.syki.com.br e teste o sistema em produção.

<img src="./Docs/images/syki_overview.gif" style="border-radius: 6px">
