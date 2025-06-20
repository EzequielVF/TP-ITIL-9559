# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TP-ITIL-9559.csproj", "."]
RUN dotnet restore "./TP-ITIL-9559.csproj"
COPY . .
RUN dotnet publish "./TP-ITIL-9559.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "TP-ITIL-9559.dll"]
