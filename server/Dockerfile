FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PcMate.Api.csproj", "./"]
RUN dotnet restore "./PcMate.Api.csproj"
COPY . .
RUN dotnet publish "PcMate.Api.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PcMate.Api.dll"]