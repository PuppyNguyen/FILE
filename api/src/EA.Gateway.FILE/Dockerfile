#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/EA.Gateway.FILE/EA.Gateway.FILE.csproj", "src/EA.Gateway.FILE/"]
COPY ["src/EA.NetDevPack/EA.NetDevPack.csproj", "src/EA.NetDevPack/"]
RUN dotnet restore "src/EA.Gateway.FILE/EA.Gateway.FILE.csproj"
COPY . .
WORKDIR "/src/src/EA.Gateway.FILE"
RUN dotnet build "EA.Gateway.FILE.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EA.Gateway.FILE.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EA.Gateway.FILE.dll"]