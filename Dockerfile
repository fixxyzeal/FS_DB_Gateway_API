#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["FS_DB_GatewayAPI/FS_DB_GatewayAPI.csproj", "FS_DB_GatewayAPI/"]
RUN dotnet restore "FS_DB_GatewayAPI/FS_DB_GatewayAPI.csproj"
COPY . .
WORKDIR "/src/FS_DB_GatewayAPI"
RUN dotnet build "FS_DB_GatewayAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FS_DB_GatewayAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FS_DB_GatewayAPI.dll"]