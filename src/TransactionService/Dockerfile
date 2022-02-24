#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:6.0.1 AS base
WORKDIR /app
EXPOSE 8080
ARG VERSION="dev"
ENV VERSION=$VERSION

FROM mcr.microsoft.com/dotnet/sdk:6.0.101 AS publish
WORKDIR /src
COPY . .
RUN dotnet restore TransactionService/TransactionService.csproj
RUN dotnet publish TransactionService/TransactionService.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransactionService.dll"]