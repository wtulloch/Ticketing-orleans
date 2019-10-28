FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["ticketing-server/Silo/Silo.csproj", "ticketing-server/Silo/"]
COPY ["Utils/Utils.csproj", "Utils/"]
COPY ["ticketing-server/Grains.Interfaces/Grains.Interfaces.csproj", "ticketing-server/Grains.Interfaces/"]
COPY ["Ticketing.Models/Ticketing.Models.csproj", "Ticketing.Models/"]
COPY ["ticketing-server/Grains/Grains.csproj", "ticketing-server/Grains/"]
COPY ["ticketing-server/Repositories/Repositories.csproj", "ticketing-server/Repositories/"]
RUN dotnet restore "ticketing-server/Silo/Silo.csproj"
COPY . .
WORKDIR "/src/ticketing-server/Silo"
RUN dotnet build "Silo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Silo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Silo.dll"]