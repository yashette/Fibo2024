FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.19 AS build
WORKDIR /src
COPY . .
RUN apk update && apk upgrade
RUN apk add --no-cache clang build-base zlib-dev
RUN dotnet publish "Demo.csproj" -c Release -r linux-musl-x64 /p:PublishAot=true /p:PublishTrimmed=true /p:PublishReadyToRun=true -o /publish

FROM docker.io/library/alpine:3.19 AS base
WORKDIR /app
COPY --from=build /publish .
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENTRYPOINT ["/app/Demo"]
