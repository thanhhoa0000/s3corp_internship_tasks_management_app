# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

RUN mkdir -p /home/app/.aspnet/https

COPY ./certs/.aspnet/https/Web.pfx /home/app/.aspnet/https/Web.pfx
COPY ./certs/.aspnet/https/Authen.API.pfx /home/app/.aspnet/https/Authen.API.pfx
COPY ./certs/.aspnet/https/Tasks.API.pfx /home/app/.aspnet/https/Tasks.API.pfx
COPY ./certs/.aspnet/https/Users.API.pfx /home/app/.aspnet/https/Users.API.pfx

RUN chown -R app:app /home/app/.aspnet/https

COPY ./certs/.aspnet/https/mytasks_ca.crt /usr/local/share/ca-certificates/mytasks_ca.crt
RUN chmod 644 /usr/local/share/ca-certificates && update-ca-certificates

USER app
WORKDIR /app
COPY ["nlog.config", "."]
EXPOSE 7080
EXPOSE 7081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["Frontends/Web/Web/Web.csproj", "Frontends/Web/Web/"]
RUN dotnet restore "./Frontends/Web/Web/Web.csproj"
COPY . .
WORKDIR "/src/Frontends/Web/Web"
RUN dotnet build "./Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]