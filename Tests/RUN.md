# RUN

## Code Coverage

dotnet test --collect:"XPlat Code Coverage"

reportgenerator -reports:"C:\Users\Zaqueu\syki\Tests\TestResults\*\coverage.cobertura.xml" -targetdir:"./Tests/Reports" -reporttypes:Html




# Mutation

cd tests
dotnet stryker -o


## 


dotnet test --filter TestCategory=Auth
dotnet test --filter TestCategory=Integration
dotnet test --filter "FullyQualifiedName~UnitTests"

dotnet test --no-build --filter "FullyQualifiedName~E2ETests" --logger:"console;verbosity=detailed"

pwsh Tests/bin/Debug/net8.0/playwright.ps1 codegen http://localhost:6001

dotnet test --logger:"console;verbosity=detailed"
dotnet test --logger:"console;verbosity=detailed" --filter "FullyQualifiedName~IntegrationTests"

dotnet test --collect:"XPlat Code Coverage"
dotnet test --logger:"console;verbosity=detailed" --collect:"XPlat Code Coverage"

reportgenerator -reports:"C:\Users\Zaqueu\syki\Tests\TestResults\*\coverage.cobertura.xml" -targetdir:"./Tests/Reports" -reporttypes:Html





