#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["JwtAuthenticationServer/JwtAuthenticationServer.csproj", "JwtAuthenticationServer/"]
RUN dotnet restore "JwtAuthenticationServer/JwtAuthenticationServer.csproj"
COPY . .
WORKDIR "/src/JwtAuthenticationServer"
RUN dotnet build "JwtAuthenticationServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JwtAuthenticationServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JwtAuthenticationServer.dll"]