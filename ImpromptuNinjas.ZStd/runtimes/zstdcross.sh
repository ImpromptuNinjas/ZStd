#!/bin/sh
docker build . -t zstdcross
# clean up old zlib artifacts
rm -rf linux osx win
# produce zlib artifacts in container
ZSTDCROSS_CONTAINER=$(docker create zstdcross --name zstdcross)
# extract zlib artifacts
docker cp "${ZSTDCROSS_CONTAINER}:/app/" ./
# clean up container
docker rm "${ZSTDCROSS_CONTAINER}"
mv app/* ./
rm -rf app
# consider purging your images