# syntax=docker/dockerfile:1.7

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["StageSim.Domain/StageSim.Domain.csproj", "StageSim.Domain/"]
COPY ["StageSim.Infrastructure/StageSim.Infrastructure.csproj", "StageSim.Infrastructure/"]
COPY ["StageSim.API/StageSim.API.csproj", "StageSim.API/"]

RUN --mount=type=cache,id=nuget-packages,target=/root/.nuget/packages \
    --mount=type=cache,id=nuget-http,target=/root/.local/share/NuGet/v3-cache \
    dotnet restore "StageSim.API/StageSim.API.csproj"

COPY . .

WORKDIR "/src/StageSim.API"
RUN dotnet publish "StageSim.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "StageSim.API.dll"]
