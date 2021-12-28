#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Api/Api.csproj", "Api/"]
COPY ["Infrastructure/Persistence/Persistence.csproj", "Infrastructure/Persistence/"]
COPY ["Core/Domain/Domain.csproj", "Core/Domain/"]
COPY ["Core/Service/Service.csproj", "Core/Service/"]
COPY ["Core/Service.Abstractions/Service.Abstractions.csproj", "Core/Service.Abstractions/"]
COPY ["Core/Contracts/Contracts.csproj", "Core/Contracts/"]
COPY ["Infrastructure/Presentation/Presentation.csproj", "Infrastructure/Presentation/"]
RUN dotnet restore "Api/Api.csproj"
COPY . .
WORKDIR "/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]