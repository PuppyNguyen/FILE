#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/EA.Api.FILE/EA.Api.FILE.csproj", "src/EA.Api.FILE/"]
COPY ["src/EA.Application.FILE/EA.Application.FILE.csproj", "src/EA.Application.FILE/"]
COPY ["src/EA.Domain.FILE/EA.Domain.FILE.csproj", "src/EA.Domain.FILE/"]
COPY ["src/EA.NetDevPack/EA.NetDevPack.csproj", "src/EA.NetDevPack/"]
COPY ["src/EA.Infra.FILE/EA.Infra.FILE.csproj", "src/EA.Infra.FILE/"]
RUN dotnet restore "src/EA.Api.FILE/EA.Api.FILE.csproj"
COPY . .
WORKDIR "/src/src/EA.Api.FILE"
RUN dotnet build "EA.Api.FILE.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EA.Api.FILE.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EA.Api.FILE.dll"]