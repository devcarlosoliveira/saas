# Define a versão do .NET a ser usada. Usar uma variável facilita a atualização.
ARG DOTNET_VERSION=10.0

# --- Estágio Base ---
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS base
WORKDIR /app
EXPOSE 8080

# Configura a porta interna do ASP.NET Core
ENV ASPNETCORE_HTTP_PORTS=8080

# Cria a pasta de dados ANTES de trocar de usuário 
RUN mkdir -p /app/Data \ 
    && chmod -R 777 /app/Data

# Cria usuário não-root (imagem padrão = Debian Slim → usar useradd)
RUN useradd --system --no-create-home appuser
USER appuser

# --- Estágio de Build ---
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src

# Copia arquivos de projeto
COPY ["Saas.Web/Saas.Web.csproj", "Saas.Web/"]
COPY ["Saas.slnx", "."]

# Restaura dependências
RUN dotnet restore "Saas.Web/Saas.Web.csproj"

# Copia todo o código
COPY . .

# Publica em Release
RUN dotnet publish "Saas.Web/Saas.Web.csproj" -c Release -o /app/publish --no-restore

# --- Estágio Final ---
FROM base AS final
WORKDIR /app

# Copia artefatos publicados
COPY --from=build /app/publish .

# Define o entrypoint
ENTRYPOINT ["dotnet", "Saas.Web.dll"]
