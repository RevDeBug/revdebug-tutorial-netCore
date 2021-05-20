# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
WORKDIR /InvoicesCore

RUN apt-get install -y git

ARG REVDEBUG_RECORD_SERVER_ADDRESS_ARG=127.0.0.1
ENV REVDEBUG_RECORD_SERVER_ADDRESS=$REVDEBUG_RECORD_SERVER_ADDRESS_ARG

COPY . ./
RUN dotnet nuget add source https://nexus.revdebug.com/repository/nuget --name rdb_nexus
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:3.1

WORKDIR /InvoicesCore
COPY --from=build-env /InvoicesCore/out .
ENV ASPNETCORE_HOSTINGSTARTUPASSEMBLIES=RevDeBugAPM.Agent.AspNetCore
ENTRYPOINT ["dotnet", "InvoicesCore.dll"]
