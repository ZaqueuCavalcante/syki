# Commands

dotnet test --logger:"console;verbosity=detailed"
dotnet test --logger:"console;verbosity=detailed" --filter "FullyQualifiedName~UnitTests"

# How to create maintainable and testable Blazor components

1 - They should do one thing well.
2 - They don't mix abstraction levels.
3 - They exhibit a high level of cohesion.
4 - They don't depend on infrastructure, they depend on abstractions.

# BUnit

- Components have life-cycle methods that must be invoked in the order Blazor runtime does it.
- Parameters and services must be passed in like the Blazor runtime does it.
- Components have event handlers that must be invoked like the Blazor runtime does it.
- BUnit is a Blazor runtime specifically built for testing.




