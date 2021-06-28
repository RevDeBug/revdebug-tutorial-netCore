# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
WORKDIR /InvoicesCore

ARG REVDEBUG_RECORD_SERVER_ADDRESS
ENV REVDEBUG_RECORD_SERVER_ADDRESS={$REVDEBUG_RECORD_SERVER_ADDRESS:-127.0.0.1}

RUN apt-get update && apt-get install -y git

COPY . ./

RUN dotnet nuget add source https://nexus.revdebug.com/repository/nuget --name rdb_nexus \
&& dotnet restore \
&& dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:3.1

WORKDIR /InvoicesCore

ENV ASPNETCORE_HOSTINGSTARTUPASSEMBLIES=RevDeBugAPM.Agent.AspNetCore
COPY --from=build-env /InvoicesCore/out .

ENTRYPOINT ["dotnet", "InvoicesCore.dll"]
