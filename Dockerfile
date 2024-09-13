# Base image for build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY WebAPI/api/*.csproj ./api/
WORKDIR /app/api
RUN dotnet restore

# Copy everything else and build
COPY WebAPI/api/. ./
RUN dotnet build -c Release -o out

# Publish the application
RUN dotnet publish -c Release -o out

# Runtime image for final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "api.dll"]
