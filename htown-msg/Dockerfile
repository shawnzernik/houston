FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ./webapi /src/webapi
RUN dotnet restore /src/webapi/webapi.csproj
RUN dotnet build /src/webapi/webapi.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish /src/webapi/webapi.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish /app

ENTRYPOINT ["/app/webapi"]
