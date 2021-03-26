#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 8080
ARG VERSION="dev"
ENV VERSION=$VERSION

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS publish
WORKDIR /src
COPY . .
RUN dotnet restore AccountService/AccountService.csproj
RUN dotnet publish AccountService/AccountService.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccountService.dll"]