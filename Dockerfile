FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Sod.Infrastructure/* Sod.Infrastructure/
COPY Sod.Model/* Sod.Model/
COPY Sod.Worker/* Sod.Worker/

RUN dotnet restore Sod.Infrastructure/Sod.Infrastructure.csproj
RUN dotnet restore Sod.Model/Sod.Model.csproj
RUN dotnet restore Sod.Worker/Sod.Worker.csproj

RUN dotnet build Sod.Worker/Sod.Worker.csproj -c Release -o bin

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS runtime
WORKDIR /app
COPY --from=build /src/bin .

ENTRYPOINT ["dotnet", "Sod.Worker.dll"]
