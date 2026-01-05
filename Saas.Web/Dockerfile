# Define a versão do .NET a ser usada. Usar uma variável facilita a atualização.
ARG DOTNET_VERSION=10.0

# --- Estágio Base ---
# Imagem de runtime do ASP.NET Core. Contém apenas o necessário para rodar a aplicação.
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS base
WORKDIR /app
EXPOSE 8080

# Configura as portas que o ASP.NET Core irá escutar dentro do contêiner.
ENV ASPNETCORE_HTTP_PORTS=8080

# Cria um usuário não-root para executar a aplicação por segurança.
RUN adduser --system --group --no-create-home appuser
USER appuser

# --- Estágio de Build ---
# Imagem do SDK do .NET. Contém as ferramentas para compilar a aplicação.
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src

# Copia os arquivos de projeto (.csproj) e o arquivo de solução (.slnx)
# O `COPY` separado para cada arquivo otimiza o cache do Docker.
COPY ["Saas.Web/Saas.Web.csproj", "Saas.Web/"]
COPY ["Saas.slnx", "."]

# Restaura as dependências do projeto. Este passo só será re-executado se os arquivos .csproj mudarem.
RUN dotnet restore "Saas.Web/Saas.Web.csproj"

# Copia o restante do código-fonte.
COPY . .

# Compila e publica a aplicação em modo Release.
RUN dotnet publish "Saas.Web/Saas.Web.csproj" -c Release -o /app/publish --no-restore

# --- Estágio Final ---
# Imagem final, baseada na imagem de runtime.
FROM base AS final
WORKDIR /app

# Copia os artefatos publicados do estágio de build.
COPY --from=build /app/publish .

# Define o ponto de entrada para a aplicação.
ENTRYPOINT ["dotnet", "Saas.Web.dll"]