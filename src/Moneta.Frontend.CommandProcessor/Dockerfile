FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Moneta.Frontend.CommandProcessor/Moneta.Frontend.CommandProcessor.csproj", "Moneta.Frontend.CommandProcessor/"]
RUN dotnet restore "Moneta.Frontend.CommandProcessor/Moneta.Frontend.CommandProcessor.csproj"
COPY . .
WORKDIR "/src/Moneta.Frontend.CommandProcessor"
RUN dotnet build "Moneta.Frontend.CommandProcessor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Moneta.Frontend.CommandProcessor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Moneta.Frontend.CommandProcessor.dll"]