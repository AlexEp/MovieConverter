#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Artlist.Server/Artlist.Server.csproj", "Artlist.Server/"]
COPY ["Artlist.Common/Artlist.Common.csproj", "Artlist.Common/"]
COPY ["Artlist.Core/Artlist.Core.csproj", "Artlist.Core/"]
RUN dotnet restore "Artlist.Server/Artlist.Server.csproj"
COPY . .
WORKDIR "/src/Artlist.Server"
RUN dotnet build "Artlist.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Artlist.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Artlist.Server.dll"]
