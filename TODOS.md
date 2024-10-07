# TODOS

- Entender o sistema
    - Front
    - Back
    - Banco
    - Daemon

- Unit
    - dotnet test --filter "FullyQualifiedName~UnitTests" --logger:"console;verbosity=detailed"

- Integration
    - dotnet test --filter "FullyQualifiedName~IntegrationTests" --logger:"console;verbosity=detailed"

- E2E
    - Should_login_into_app
    - $env:PWDEBUG=1
    - dotnet test --no-build --filter "FullyQualifiedName~E2ETests" --logger:"console;verbosity=detailed"

- Mutation
    - cd tests
    - dotnet stryker -o

- Coverage Report
    - dotnet test --collect:"XPlat Code Coverage"
    - reportgenerator -reports:"C:\Users\Zaqueu\syki\Tests\TestResults\*\coverage.cobertura.xml" -targetdir:"./Tests/Reports" -reporttypes:Html
