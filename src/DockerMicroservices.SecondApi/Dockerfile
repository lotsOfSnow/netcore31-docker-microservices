#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/DockerMicroservices.SecondApi/DockerMicroservices.SecondApi.csproj", "src/DockerMicroservices.SecondApi/"]
COPY ["src/DockerMicroservices.Shared/DockerMicroservices.Shared.csproj", "src/DockerMicroservices.Shared/"]
RUN dotnet restore "src/DockerMicroservices.SecondApi/DockerMicroservices.SecondApi.csproj"
COPY . .
WORKDIR "/src/src/DockerMicroservices.SecondApi"
RUN dotnet build "DockerMicroservices.SecondApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerMicroservices.SecondApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerMicroservices.SecondApi.dll"]