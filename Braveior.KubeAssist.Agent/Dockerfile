#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Braveior.KubeAssist.Agent/Braveior.KubeAssist.Agent.csproj", "Braveior.KubeAssist.Agent/"]
RUN dotnet restore "Braveior.KubeAssist.Agent/Braveior.KubeAssist.Agent.csproj"
COPY . .
WORKDIR "/src/Braveior.KubeAssist.Agent"
RUN dotnet build "Braveior.KubeAssist.Agent.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Braveior.KubeAssist.Agent.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Braveior.KubeAssist.Agent.dll"]