#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Braveior.KubeAssist/Server/Braveior.KubeAssist.Server.csproj", "Braveior.KubeAssist/Server/"]
COPY ["Braveior.KubeAssist/Client/Braveior.KubeAssist.Client.csproj", "Braveior.KubeAssist/Client/"]
RUN dotnet restore "Braveior.KubeAssist/Server/Braveior.KubeAssist.Server.csproj"
COPY . .
WORKDIR "/src/Braveior.KubeAssist/Server"
RUN dotnet build "Braveior.KubeAssist.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Braveior.KubeAssist.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Braveior.KubeAssist.Server.dll"]