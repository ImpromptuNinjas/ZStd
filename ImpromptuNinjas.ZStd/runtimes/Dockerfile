FROM alpine/git AS git

RUN apk add build-base \
	&& git clone -b v1.4.5 --depth 1 https://github.com/facebook/zstd.git /src

# amd64-linux-musl
FROM amd64/alpine AS amd64-linux-musl
COPY --from=git /src /src
RUN apk add build-base
WORKDIR /src/lib
RUN CC="x86_64-alpine-linux-musl-gcc" CXX="x86_64-alpine-linux-musl-g++" \
	CFLAGS="-g0 -O3 -flto -static-libgcc -fvisibility=hidden" \
	CXXFLAGS="-g0 -O3 -flto -static-libgcc -fno-rtti -fvisibility=hidden -fvisibility-inlines-hidden" \
	LDFLAGS="-static-libgcc" \
	make -j`nproc` libzstd-mt && make -C ../tests zbufftest-dll && mkdir -p /app/ && strip --strip-unneeded /src/lib/libzstd.so && cp -L /src/lib/libzstd.so /app/

# i386-linux-musl
FROM i386/alpine AS i386-linux-musl
COPY --from=git /src /src
RUN apk add build-base
WORKDIR /src/lib
RUN CC="i586-alpine-linux-musl-gcc" CXX="i586-alpine-linux-musl-g++" \
	CFLAGS="-g0 -O3 -flto -static-libgcc -fvisibility=hidden" \
	CXXFLAGS="-g0 -O3 -flto -static-libgcc -fno-rtti -fvisibility=hidden -fvisibility-inlines-hidden" \
	LDFLAGS="-static-libgcc" \
	make -j`nproc` libzstd-mt && make -C ../tests zbufftest-dll && mkdir -p /app/ && strip --strip-unneeded /src/lib/libzstd.so && cp -L /src/lib/libzstd.so /app/

# arm64-linux-musl
FROM arm64v8/alpine AS arm64-linux-musl
COPY --from=git /src /src
RUN apk add build-base \
	&& mkdir -p /app/
WORKDIR /src/lib
RUN CC="aarch64-alpine-linux-musl-gcc" CXX="aarch64-alpine-linux-musl-g++" \
	CFLAGS="-g0 -O3 -flto -static-libgcc -fvisibility=hidden" \
	CXXFLAGS="-g0 -O3 -flto -static-libgcc -fno-rtti -fvisibility=hidden -fvisibility-inlines-hidden" \
	LDFLAGS="-static-libgcc" \
	make -j`nproc` libzstd-mt && make -C ../tests zbufftest-dll && mkdir -p /app/ && strip --strip-unneeded /src/lib/libzstd.so && cp -L /src/lib/libzstd.so /app/

# arm32-linux-musl
FROM arm32v7/alpine AS arm32-linux-musl
COPY --from=git /src /src
RUN apk add build-base
WORKDIR /src/lib
RUN CC="armv7-alpine-linux-musleabihf-gcc" CXX="armv7-alpine-linux-musleabihf-g++" \
	CFLAGS="-g0 -O3 -flto -mfloat-abi=hard -static-libgcc -fvisibility=hidden" \
	CXXFLAGS="-g0 -O3 -flto -mfloat-abi=hard -static-libgcc -fno-rtti -fvisibility=hidden -fvisibility-inlines-hidden" \
	LDFLAGS="-static-libgcc" \
	make -j`nproc` libzstd-mt && make -C ../tests zbufftest-dll && mkdir -p /app/ && strip --strip-unneeded /src/lib/libzstd.so && cp -L /src/lib/libzstd.so /app/

# amd64-linux-glibc
FROM amd64/debian:stable-slim AS amd64-linux-glibc
COPY --from=git /src /src
RUN apt-get update && apt-get install -y --no-install-recommends build-essential
WORKDIR /src/lib
RUN CC="x86_64-linux-gnu-gcc" CXX="x86_64-linux-gnu-g++" \
	CFLAGS="-g0 -O3 -flto -static-libgcc -fvisibility=hidden" \
	CXXFLAGS="-g0 -O3 -flto -static-libgcc -fno-rtti -fvisibility=hidden -fvisibility-inlines-hidden" \
	LDFLAGS="-static-libgcc" \
	make -j`nproc` libzstd-mt && make -C ../tests zbufftest-dll && mkdir -p /app/ && strip --strip-unneeded /src/lib/libzstd.so && cp -L /src/lib/libzstd.so /app/

# i386-linux-glibc
FROM i386/debian:stable-slim AS i386-linux-glibc
COPY --from=git /src /src
RUN apt-get update && apt-get install -y --no-install-recommends build-essential
WORKDIR /src/lib
RUN CC="i686-linux-gnu-gcc" CXX="i686-linux-gnu-g++" \
	CFLAGS="-g0 -O3 -flto -static-libgcc -fvisibility=hidden" \
	CXXFLAGS="-g0 -O3 -flto -static-libgcc -fno-rtti -fvisibility=hidden -fvisibility-inlines-hidden" \
	LDFLAGS="-static-libgcc" \
	make -j`nproc` libzstd-mt && make -C ../tests zbufftest-dll && mkdir -p /app/ && strip --strip-unneeded /src/lib/libzstd.so && cp -L /src/lib/libzstd.so /app/

# arm64-linux-glibc
FROM arm64v8/debian:stable-slim AS arm64-linux-glibc
COPY --from=git /src /src
RUN apt-get update && apt-get install -y --no-install-recommends build-essential
WORKDIR /src/lib
RUN CC="aarch64-linux-gnu-gcc" CXX="aarch64-linux-gnu-g++" \
	CFLAGS="-g0 -O3 -flto -static-libgcc -fvisibility=hidden" \
	CXXFLAGS="-g0 -O3 -flto -static-libgcc -fno-rtti -fvisibility=hidden -fvisibility-inlines-hidden" \
	LDFLAGS="-static-libgcc" \
	make -j`nproc` libzstd-mt && make -C ../tests zbufftest-dll && mkdir -p /app/ && strip --strip-unneeded /src/lib/libzstd.so && cp -L /src/lib/libzstd.so /app/

# arm32-linux-glibc
FROM arm32v7/debian:stable-slim AS arm32-linux-glibc
RUN apt-get update && apt-get install -y --no-install-recommends build-essential
COPY --from=git /src /src
WORKDIR /src/lib
RUN CC="arm-linux-gnueabihf-gcc" CXX="arm-linux-gnueabihf-g++" \
	CC="aarch64-linux-gnu-gcc" \
	CFLAGS="-g0 -O3 -flto -mfloat-abi=hard -static-libgcc -fvisibility=hidden" \
	CXXFLAGS="-g0 -O3 -flto -mfloat-abi=hard -static-libgcc -fno-rtti -fvisibility=hidden -fvisibility-inlines-hidden" \
	LDFLAGS="-static-libgcc" \
	&& make -j`nproc` libzstd-mt && make -C ../tests zbufftest-dll && mkdir -p /app/ && strip --strip-unneeded /src/lib/libzstd.so && cp -L /src/lib/libzstd.so /app/

# osx64
FROM liushuyu/osxcross AS osx64
COPY --from=git /src /src
WORKDIR /src/lib
RUN echo '#!/bin/sh\necho Darwin' > /bin/uname
RUN CC="/opt/osxcross/bin/o64-clang" CXX="/opt/osxcross/bin/o64-clang++" \
	CFLAGS="-g0 -O3 -flto -fvisibility=hidden" \
	CXXFLAGS="-g0 -O3 -flto -fno-rtti -fvisibility=hidden -fvisibility-inlines-hidden" \
	make -j`nproc` libzstd-mt && mkdir -p /app/ && /opt/osxcross/bin/x86_64-apple-darwin18-strip -S /src/lib/libzstd.dylib \
	&& cp -L /src/lib/libzstd.dylib /app/

# win64 NOTE: makefile has wrong slash for in output causing a erroneous 'dll' file name prefix
FROM softwareperonista/archlinux-mingw-w64 AS win64
COPY --from=git /src /src
WORKDIR /src/lib
RUN CC="x86_64-w64-mingw32-gcc" CXX="x86_64-w64-mingw32-g++" OS="Windows_NT" \
	CFLAGS="-g0 -O3 -flto -static-libgcc -fvisibility=hidden" \
	CXXFLAGS="-g0 -O3 -flto -static-libgcc -fno-rtti -fvisibility=hidden -fvisibility-inlines-hidden" \
	LDFLAGS="-static-libgcc" \
	make -j`nproc` libzstd-mt && make -C ../tests zbufftest-dll && mkdir -p /app/ && strip --strip-unneeded /src/lib/dlllibzstd.dll && cp -L /src/lib/dlllibzstd.dll /app/libzstd.dll

# win32
FROM softwareperonista/archlinux-mingw-w64 AS win32
COPY --from=git /src /src
WORKDIR /src/lib
RUN CC="i686-w64-mingw32-gcc" CXX="i686-w64-mingw32-gg++" OS="Windows_NT" \
	CFLAGS="-g0 -O3 -flto -static-libgcc -fvisibility=hidden" \
	CXXFLAGS="-g0 -O3 -flto -static-libgcc -fno-rtti -fvisibility=hidden -fvisibility-inlines-hidden" \
	LDFLAGS="-static-libgcc" \
	make -j`nproc` libzstd-mt && make -C ../tests zbufftest-dll && mkdir -p /app/ && strip --strip-unneeded /src/lib/dlllibzstd.dll && cp -L /src/lib/dlllibzstd.dll /app/libzstd.dll

FROM git as finale

COPY --from=amd64-linux-musl /app/ /app/linux-musl-x64/native/
COPY --from=i386-linux-musl /app/ /app/linux-musl-x86/native/
COPY --from=arm64-linux-musl /app/ /app/linux-musl-arm64/native/
COPY --from=arm32-linux-musl /app/ /app/linux-musl-arm/native/

COPY --from=amd64-linux-glibc /app/ /app/linux-x64/native/
COPY --from=i386-linux-glibc /app/ /app/linux-x86/native/
COPY --from=arm64-linux-glibc /app/ /app/linux-arm64/native/
COPY --from=arm32-linux-glibc /app/ /app/linux-arm/native/

COPY --from=osx64 /app/ /app/osx-x64/native/

COPY --from=win64 /app/ /app/win-x64/native/
COPY --from=win32 /app/ /app/win-x86/native/

# now we just extract the app dir as an artifact
ENTRYPOINT ["/bin/sh"]