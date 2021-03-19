#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Artlist.Core/Artlist.Core.csproj", "Artlist.Core/"]
COPY ["Artlist.Common/Artlist.Common.csproj", "Artlist.Common/"]
RUN dotnet restore "Artlist.Core/Artlist.Core.csproj"
COPY . .
WORKDIR "/src/Artlist.Core"
RUN dotnet build "Artlist.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Artlist.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Artlist.Core.dll"]
