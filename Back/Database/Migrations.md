# Migrations

## Criar uma nova migration

dotnet ef migrations add Bootstrap

## Generate SQL script for migrations

dotnet ef migrations script -o all_migrations.sql

## Generate SQL from one migration to front

dotnet ef migrations script NAME single_migration.sql
