FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/VideoGames.Api/VideoGames.Api.csproj", "src/VideoGames.Api/"]
COPY ["src/VideoGames.Application/VideoGames.Application.csproj", "src/VideoGames.Application/"]
COPY ["src/VideoGames.Infrastructure/VideoGames.Infrastructure.csproj", "src/VideoGames.Infrastructure/"]
COPY ["src/VideoGames.Domain/VideoGames.Domain.csproj", "src/VideoGames.Domain/"]
RUN dotnet restore "src/VideoGames.Api/VideoGames.Api.csproj"

COPY . .
WORKDIR "/src/src/VideoGames.Api"
RUN dotnet publish "VideoGames.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "VideoGames.Api.dll"]
