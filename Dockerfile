# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Choppz.Servicos.CalculoImposto/Servicos.CalculoImposto.Api/Servicos.CalculoImposto.Api.csproj", "src/Choppz.Servicos.CalculoImposto/Servicos.CalculoImposto.Api/"]
COPY ["src/Choppz.Servicos.CalculoImposto/Servicos.CalculoImposto.Application/Servicos.CalculoImposto.Application.csproj", "src/Choppz.Servicos.CalculoImposto/Servicos.CalculoImposto.Application/"]
COPY ["src/Choppz.Servicos.CalculoImposto/Servicos.CalculoImposto.Core/Servicos.CalculoImposto.Core.csproj", "src/Choppz.Servicos.CalculoImposto/Servicos.CalculoImposto.Core/"]
COPY ["src/Choppz.Servicos.CalculoImposto/Servicos.CalculoImposto.Infra/Servicos.CalculoImposto.Infra.csproj", "src/Choppz.Servicos.CalculoImposto/Servicos.CalculoImposto.Infra/"]
RUN dotnet restore "src/Choppz.Servicos.CalculoImposto/Servicos.CalculoImposto.Api/Servicos.CalculoImposto.Api.csproj"
COPY . .
WORKDIR "/src/src/Choppz.Servicos.CalculoImposto/Servicos.CalculoImposto.Api"
RUN dotnet build "Servicos.CalculoImposto.Api.csproj" -c Release -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Servicos.CalculoImposto.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Servicos.CalculoImposto.Api.dll"]