# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/*.csproj .
RUN dotnet restore
COPY src .
RUN dotnet publish -c Release -o /app

# Estágio de produção
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS production
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "EscalaApi.dll"]

# Estágio de desenvolvimento
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS development
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development \
    ASPNETCORE_URLS=http://+:8080 \
    DOTNET_USE_POLLING_FILE_WATCHER=1 \
    DOTNET_WATCH_SUPPRESS_LAUNCH_BROWSER=1 \
    DOTNET_WATCH_SUPPRESS_MSBUILD_INCREMENTALISM=1 \
    DOTNET_WATCH_RESTART_ON_RUDE_EDIT=1
ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile"]