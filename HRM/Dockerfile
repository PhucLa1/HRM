# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 7025
EXPOSE 7021


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HRM/HRM.Apis.csproj", "HRM/"]
COPY ["HRM.Services/HRM.Services.csproj", "HRM.Services/"]
COPY ["HRM.Repositories/HRM.Repositories.csproj", "HRM.Repositories/"]
COPY ["HRM.Data/HRM.Data.csproj", "HRM.Data/"]
RUN dotnet restore "./HRM/HRM.Apis.csproj"
COPY . .
WORKDIR "/src/HRM"
RUN dotnet build "./HRM.Apis.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HRM.Apis.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HRM.Apis.dll"]