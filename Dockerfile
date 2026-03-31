# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . ./
WORKDIR /app/TicketSystem
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/TicketSystem/out ./

ENV ASPNETCORE_URLS=http://+:$PORT

CMD ["dotnet", "TicketSystem.dll"]