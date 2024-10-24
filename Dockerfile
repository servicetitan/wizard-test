# syntax=docker/dockerfile:1.7-labs
FROM mcr.microsoft.com/dotnet/aspnet:8.0.6-alpine3.19-amd64 as runtime
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0.301-alpine3.19-amd64 as sdk
ARG ST__NuGetKey=""
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_CLI_UI_LANGUAGE=en-US \
    DOTNET_SVCUTIL_TELEMETRY_OPTOUT=1 \
    DOTNET_NOLOGO=1 \
    POWERSHELL_TELEMETRY_OPTOUT=1 \
    POWERSHELL_UPDATECHECK_OPTOUT=1 \
    NUGET_CERT_REVOCATION_MODE=offline \
    ST__NuGetKey=${ST__NuGetKey}
WORKDIR /src

FROM sdk as build

ARG GIT_REMOTE_URL
ARG GIT_HEAD_FRIENDLY_NAME
ENV GIT_REMOTE_URL=$GIT_REMOTE_URL\
    GIT_HEAD_FRIENDLY_NAME=$GIT_HEAD_FRIENDLY_NAME

COPY .config NuGet.Config ./
RUN dotnet tool restore

COPY --link --parents WizardTest.sln  .editorconfig **/*.csproj **/*.props **/*.targets ./
RUN dotnet restore WizardTest.sln -nologo --no-dependencies -p:RestoreIgnoreFailedSources=True -verbosity:quiet -p:Configuration=Release

COPY src src
COPY tests tests
RUN dotnet build -c Release --no-restore


# Create web-api runtime image
FROM runtime as web-api
WORKDIR /app
COPY --from=build /src/artifacts/web-api .
ENTRYPOINT ["dotnet", "ServiceTitan.WizardTest.Host.Web.dll"]
