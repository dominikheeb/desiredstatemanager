FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 50234
EXPOSE 44350

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY api/DesiredStateManager.Api/DesiredStateManager.Api.csproj api/DesiredStateManager.Api/
RUN dotnet restore api/DesiredStateManager.Api/DesiredStateManager.Api.csproj
COPY . .
WORKDIR /src/api/DesiredStateManager.Api
RUN dotnet build DesiredStateManager.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish DesiredStateManager.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DesiredStateManager.Api.dll"]
