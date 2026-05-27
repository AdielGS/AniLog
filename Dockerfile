# Estágio 1: Compilação do projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copia os arquivos de projeto e restaura as dependências NuGet
COPY ["AniLog/AniLog.csproj", "AniLog/"]
COPY ["AniLog.sln", "./"]
RUN dotnet restore "AniLog/AniLog.csproj"

# Copia todo o resto do código e compila em modo Release
COPY . ./
RUN dotnet publish "AniLog/AniLog.csproj" -c Release -o out

# Estágio 2: Criação da imagem final leve de execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Configura a porta que o Render usa para expor sites Docker na internet
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "AniLog.dll"]
