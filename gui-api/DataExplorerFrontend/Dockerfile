#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["DataExplorerFrontend.csproj", "DataExplorerFrontend/"]
RUN dotnet restore "DataExplorerFrontend/DataExplorerFrontend.csproj"

WORKDIR "/src/DataExplorerFrontend"
COPY . .

RUN dotnet build "DataExplorerFrontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataExplorerFrontend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataExplorerFrontend.dll"]
