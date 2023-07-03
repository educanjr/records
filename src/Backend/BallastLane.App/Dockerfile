#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BallastLane.App/BallastLane.App.csproj", "BallastLane.App/"]
COPY ["BallastLane.Application/BallastLane.Application.csproj", "BallastLane.Application/"]
COPY ["BallastLane.Domain/BallastLane.Domain.csproj", "BallastLane.Domain/"]
COPY ["BallastLane.Infrastructure/BallastLane.Infrastructure.csproj", "BallastLane.Infrastructure/"]
COPY ["BallastLane.Persistence/BallastLane.Persistence.csproj", "BallastLane.Persistence/"]
COPY ["BallastLane.Presentation/BallastLane.Presentation.csproj", "BallastLane.Presentation/"]
RUN dotnet restore "BallastLane.App/BallastLane.App.csproj"
COPY . .
WORKDIR "/src/BallastLane.App"
RUN dotnet build "BallastLane.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BallastLane.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BallastLane.App.dll"]