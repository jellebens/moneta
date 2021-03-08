

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS publish
WORKDIR /src

COPY . .
RUN dotnet restore Moneta.Frontend.Web/Moneta.Frontend.Web.csproj
RUN dotnet publish Moneta.Frontend.Web/Moneta.Frontend.Web.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Moneta.Frontend.Web.dll"]