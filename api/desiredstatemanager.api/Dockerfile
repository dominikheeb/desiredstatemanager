FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY desiredstatemanager.api/desiredstatemanager.api.csproj desiredstatemanager.api/
RUN dotnet restore desiredstatemanager.api/desiredstatemanager.api.csproj
COPY . .
WORKDIR /src/desiredstatemanager.api
RUN dotnet build desiredstatemanager.api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish desiredstatemanager.api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "desiredstatemanager.api.dll"]
