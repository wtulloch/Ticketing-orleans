FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["ticketing-server/TicketingApi/TicketingApi.csproj", "ticketing-server/TicketingApi/"]
COPY ["Utils/Utils.csproj", "Utils/"]
COPY ["ticketing-server/Grains.Interfaces/Grains.Interfaces.csproj", "ticketing-server/Grains.Interfaces/"]
COPY ["Ticketing.Models/Ticketing.Models.csproj", "Ticketing.Models/"]
RUN dotnet restore "ticketing-server/TicketingApi/TicketingApi.csproj"
COPY . .
WORKDIR "/src/ticketing-server/TicketingApi"
RUN dotnet build "TicketingApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TicketingApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicketingApi.dll"]