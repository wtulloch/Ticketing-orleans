FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 8080
FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Silo/Silo.csproj", "Silo/"]
RUN dotnet restore "Silo/Silo.csproj"
COPY . .
WORKDIR "/src/Silo"
RUN dotnet build "Silo.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Silo.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Silo.dll"]