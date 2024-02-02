FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build_stage
WORKDIR /source
COPY . .
RUN dotnet restore "./Back/Back.csproj"
RUN dotnet publish "./Back/Back.csproj" -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build_stage /app ./

EXPOSE 5000

ENTRYPOINT [ "dotnet", "Back.dll" ]
