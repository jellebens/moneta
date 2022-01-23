#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["InstrumentService/InstrumentService.csproj", "InstrumentService/"]
RUN dotnet restore "InstrumentService/InstrumentService.csproj"
COPY . .
WORKDIR "/src/InstrumentService"
RUN dotnet build "InstrumentService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InstrumentService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InstrumentService.dll"]