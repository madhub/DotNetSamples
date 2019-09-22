FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
ENV CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-netcore20-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-netcore20-agent/libNewRelicProfiler.so \
NEW_RELIC_LICENSE_KEY=b9c9b274d55bc5d413bfcdb6bdcf8dd5458c0128 \
NEW_RELIC_APP_NAME=aspnetapidemo

WORKDIR /app

ARG files=./bin/Release/netcoreapp2.1/publish
COPY $files ./app


RUN dpkg -i ./app/*.deb

WORKDIR /app
ENTRYPOINT ["dotnet", "aspnetapidemo.dll"]

