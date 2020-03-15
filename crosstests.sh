#!/bin/sh

dotnet build -c Release ImpromptuNinjas.ZStd

set -- ImpromptuNinjas.ZStd.*.nupkg

if test ! -f "$1"; then
    exit 1
fi

docker build -f Dockerfile.crosstests . -t zstdcrosstests

CONTAINER=$(docker create zstdcrosstests --name zstdcrosstests)

docker cp "${CONTAINER}:/TestResults/" ./

docker rm "${CONTAINER}"
