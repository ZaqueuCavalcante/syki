# SYKI - Sistema de Gerenciamento Acadêmico

[![Tests](https://github.com/ZaqueuCavalcante/syki/actions/workflows/tests.yml/badge.svg)](https://github.com/ZaqueuCavalcante/syki/actions/workflows/tests.yml)

## O que é este projeto?

É um sistema para gerenciamento educacional, usado por gestores, professores e alunos.

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

- Backend feito em C#/.NET
- Frontend feito em C#/Blazor Webassembly
- Banco PostgreSQL
- Docker para buildar back e front
- Deploy no Railway

## Como rodar?

- 0 - Clone o projeto pra sua máquina usando o git

- 1 - Instale o SDK do DotNet 8.0.200

- 2 - Instale o PostgreSQL ou rode ele via Docker

- 3 - Entre na pasta ./Back e rode "dotnet run" para subir a API

- 4 - Entre na pasta ./Front e rode "dotnet run" para subir o Frontend

- 5 - Caso dê erro de CORs, acesse o endereço https da API e do Frontend no navegador e aceite o certificado de dev como válido

## Overview

Se cadastre em https://syki.zaqbit.com e teste o sistema em produção.

![Overview](/Docs/images/syki_overview.gif "Overview")
