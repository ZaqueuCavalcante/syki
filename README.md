# SYKI - Sistema de Gerenciamento Acadêmico

https://stackify.com/opentelemetry-dotnet/

[![Tests](https://github.com/ZaqueuCavalcante/syki/actions/workflows/tests.yml/badge.svg)](https://github.com/ZaqueuCavalcante/syki/actions/workflows/tests.yml)

## O que é este projeto?

É um sistema para gerenciamento educacional, que pode ser usado por gestores, professores e alunos.

Por ser multi-tenant, suporta a operação de diversas instituições de ensino ao mesmo tempo, seguindo o modelo de SAAS.

## Quais as funcionalidades?

Inicialmente, o sistema só possui o módulo Acadêmico.

A ideia é adicionar mais módulos no futuro, como os de Biblioteca, Financeiro, Almoxarifado...

O módulo acadêmico conta com:

- Registro de usuários + login
- Criação de instituição + seed de dados
- Configuração de MFA + login com 2FA
- Página insights com dados consolidados
- Criação e edição de campus
- Criação de cursos, disciplinas e grades curriculares
- Possibilidade de ofertar um curso em um campus
- Cadastro de professores e alunos
- Abertura de turmas
- Matrícula acadêmica
- Envio de notificações para professores e alunos

## Qual a tech stack?

- Backend em C#/.NET
- Frontend em C#/Blazor Webassembly
- Testes automatizados com NUnit
- Banco PostgreSQL
- Docker para buildar back e front
- Deploy no Railway

<img src="./Docs/images/docker-compose.png" width="600" height="300" style="border-radius: 6px">

## Como rodar?

- Clone o projeto pra sua máquina

- Para subir banco + back + front, rode o comando: `docker-compose up`

- O back vai subir na porta 5001 e o front na 6001

## Overview

Se cadastre em https://syki.zaqbit.com e teste o sistema em produção.

<img src="./Docs/images/syki_overview.gif" style="border-radius: 6px">
