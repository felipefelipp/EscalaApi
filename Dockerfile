# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

# Estágio de produção
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS production
WORKDIR /app
COPY --from=build /app .
COPY ./database .
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "EscalaApi.dll"]

# Estágio de desenvolvimento
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS development
WORKDIR /src
COPY . .
RUN dotnet restore
ENV ASPNETCORE_ENVIRONMENT=Development \
    ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile"]