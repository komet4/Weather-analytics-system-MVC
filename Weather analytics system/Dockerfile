FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Weather analytics system/Weather analytics system.csproj", "Weather analytics system/"]
RUN dotnet restore "Weather analytics system/Weather analytics system.csproj"
COPY . .
WORKDIR "/src/Weather analytics system"
RUN dotnet build "Weather analytics system.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Weather analytics system.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Weather analytics system.dll"]
