# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["lms-app.sln", "./"]

COPY ["lms-app/lms-app.csproj", "lms-app/"]
COPY ["Application/Application.csproj", "Application/"]

RUN dotnet restore "lms-app.sln"

COPY . .

WORKDIR "/src/lms-app"

RUN dotnet build "lms-app.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR /src/lms-app

RUN dotnet publish "lms-app.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 7157
ENV ASPNETCORE_URLS=http://+:7157

ENV ASPNETCORE_HTTPS_PORT=""

ENTRYPOINT ["dotnet", "lms-app.dll"]