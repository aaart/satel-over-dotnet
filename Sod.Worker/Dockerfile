FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Sod.Service/Sod.Service.csproj", "Sod.Service/"]
RUN dotnet restore "Sod.Service/Sod.Service.csproj"
COPY . .
WORKDIR "/src/Sod.Service"
RUN dotnet build "Sod.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sod.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sod.Service.dll"]
