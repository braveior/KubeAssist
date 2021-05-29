#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Braveior.KubeAssist/Braveior.KubeAssist.csproj", "Braveior.KubeAssist/"]
COPY ["KubernetesClient/KubernetesClient.csproj", "KubernetesClient/"]
RUN dotnet restore "Braveior.KubeAssist/Braveior.KubeAssist.csproj"
COPY . .
WORKDIR "/src/Braveior.KubeAssist"
RUN dotnet build "Braveior.KubeAssist.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Braveior.KubeAssist.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Braveior.KubeAssist.dll"]