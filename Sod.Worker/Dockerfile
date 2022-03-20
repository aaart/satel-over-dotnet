FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Sod.Worker/Sod.Worker.csproj", "Sod.Worker/"]
RUN dotnet restore "Sod.Worker/Sod.Worker.csproj"
COPY . .
WORKDIR "/src/Sod.Worker"
RUN dotnet build "Sod.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sod.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sod.Worker.dll"]
