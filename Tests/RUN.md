# RUN

dotnet test --filter TestCategory=Integration
dotnet test --filter "FullyQualifiedName~UnitTests"
dotnet test --filter "FullyQualifiedName!~UnitTests"

dotnet test --logger:"console;verbosity=detailed"
dotnet test --filter "FullyQualifiedName~IntegrationTests"
dotnet test --logger:"console;verbosity=detailed" --filter "FullyQualifiedName~IntegrationTests"

dotnet test --collect:"XPlat Code Coverage"
dotnet test --logger:"console;verbosity=detailed" --collect:"XPlat Code Coverage"

reportgenerator -reports:"C:\Users\Zaqueu\syki\Tests\TestResults\*\coverage.cobertura.xml" -targetdir:"./Tests/Reports" -reporttypes:Html

## Code Coverage

dotnet test --collect:"XPlat Code Coverage"

reportgenerator -reports:"C:\Users\Zaqueu\syki\Tests\TestResults\*\coverage.cobertura.xml" -targetdir:"./Tests/Reports" -reporttypes:Html

# Mutation

cd tests
dotnet stryker -o
