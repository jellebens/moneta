#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Moneta.Frontend.API/Moneta.Frontend.API.csproj", "Moneta.Frontend.API/"]
RUN dotnet restore "Moneta.Frontend.API/Moneta.Frontend.API.csproj"
COPY . .
WORKDIR "/src/Moneta.Frontend.API"
RUN dotnet build "Moneta.Frontend.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Moneta.Frontend.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Moneta.Frontend.API.dll"]