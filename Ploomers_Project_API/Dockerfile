#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Ploomers_Project_API/Ploomers_Project_API.csproj", "Ploomers_Project_API/"]
RUN dotnet restore "Ploomers_Project_API/Ploomers_Project_API.csproj"
COPY . .
WORKDIR "/src/Ploomers_Project_API"
RUN dotnet build "Ploomers_Project_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ploomers_Project_API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ploomers_Project_API.dll"]