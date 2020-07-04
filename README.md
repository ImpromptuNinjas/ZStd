![ZStd for .NET](https://media.githubusercontent.com/media/ImpromptuNinjas/ZStd/master/icon.png)
ZStd for .NET
==================
[![NuGet](https://img.shields.io/nuget/v/ImpromptuNinjas.ZStd.svg)](https://www.nuget.org/packages/ImpromptuNinjas.ZStd/) [![Build & Test](https://github.com/ImpromptuNinjas/ZStd/workflows/Build%20&%20Test/badge.svg?branch=master)](https://github.com/ImpromptuNinjas/ZStd/actions?query=workflow%3A%22Build+%26+Test%22+branch%3Amaster)

**ZStd** is a multi-platform .NET binding of Facebook's Zstandard library.

## Supported platforms:
* Windows
  - x64
  - x86
* Linux
  - GNU flavors (Debian, Ubuntu, ...)
  - Musl flavors (Alpine, Void, ...)
  - Intel x86
  - AMD64 / Intel x86-64
  - ARMv8-64 / AArch64
  - ARMv7-HF / AArch32
* Apple OSX
  - 64-bit only

_Note: ARM is support is currently hard-float only, no soft-float platform support unless requested._
_Note: In the not too distant future support for 32-bit architectures will likely be dropped._

## Features:
* Compression and decompression
  - Dictionary support
  - Managed streams into streams of frames
  - Byte arrays and spans into frames
  - Byte arrays and spans into blocks (without frames)
* Generation of dictionaries from a collection of samples
* Loading and saving of dictionaries to and from streams, arrays and spans
* Helpers for collection of dictionary samples and training
* Training will automatically tune unspecified dictionary parameters

Take a look at the unit tests to explore its behavior in different situations.

This is in the later stages of development, and features are less likely to be added over time.

### Known Issues:
* Currently the native dependencies are shipped with this NuGet package for all platforms.
  _Separate NuGet runtime packages should be created to provide each specific platform dependency._
* Coverage currently stands at 50%. Critical path coverage is high.
  Coverage of non-critical path operations is low.
* The GC is not yet automatically made aware of unmanaged memory usage.


Acknowlegedments
----------------

### Official documentation

* [Facebook's Zstandard home page](https://facebook.github.io/zstd/)
* [Facebook's Zstandard manual](https://facebook.github.io/zstd/zstd_manual.html)
* [Facebook's Zstandard GitHub page](https://github.com/facebook/zstd)

This project is heavily inspired by ZstdNet, but shares very little in the way of implementation.

Reference
---------

### Basic usage examples

#### Dictionaries

```csharp
// pick a compression level or use 0, considered 'auto' in most cases
var compressionLevel = 3;
//var compressionLevel = ZStdCompressor.MinimumCompressionLevel; // probably 1
//var compressionLevel = ZStdCompressor.MaximumCompressionLevel; // probably 22

// allocate a 32kB buffer to build a new dictionary
var dict = new ZStdDictionaryBuilder(32 * 1024);
// or load from some external data
//var dict = new ZStdDictionaryBuilder(someBytes);
//var dict = new ZStdDictionaryBuilder(someBytes, someSize);


// train the dictionary, and retrieve tuned training parameters for future training
var trainedParams = dict.Train(async () => {
  // sample your data by returning ArraySegment<byte>s of regions you expect to occur often
  // refer to Facebook's Zstd documentation for details on what should be sampled
  yield return ArraySegment<byte>(trainingData, 7, 42);
}, compressionLevel);

// optionally save the dictionary to a file somehow
using (var fs = File.Create(path))
  dict.WriteTo(fs); // preferred
//  fs.Write(dict); // implicitly casts to ReadOnlySpan<byte>
//File.WriteAllBytes("saved_dictionary", ((ArraySegment<byte>)dict).ToArray());

// create an unmanaged reference for use with compression
using var cDict = dict.CreateCompressorDictionary(compressionLevel);

// create an unmanaged reference for use with decompression
using var dDict = dict.CreateDecompressorDictionary();

// be sure to dispose of your compressors/decompressors before your dictionary references

```

#### Compression

```csharp
// create a context
using var cCtx = new ZStdCompressor();

// specify the dictionary you want to use, or don't
cCtx.UseDictionary(cDict);

// figure out about how big of a buffer you need
var compressBufferSize = CCtx.GetUpperBound((UIntPtr) inputData.Length);

var compressBuffer = new byte[compressBufferSize];

// set the compressor to the compression level (it may inherit it from the dictionary)
cCtx.Set(CompressionParameter.CompressionLevel, compressionLevel);

// actually perform the compression operation
var compressedSize = cCtx.Compress(compressBuffer, inputData);

// retrieve your compressed frame
var compressedFrame = new ArraySegment<byte>(compressBuffer, 0, (int) compressedSize);

```

#### Decompression

```csharp
// create a context
var dCtx = new ZStdDecompressor();

// specify the dictionary you want to use, or don't
dCtx.UseDictionary(dDict);

// figure out about how big of a buffer you need
var decompressBufferSize = DCtx.GetUpperBound(compressedFrame);

var decompressBuffer = new byte[decompressBufferSize];

// actually perform the decompression operation
var decompressedSize = dCtx.Decompress(decompressBuffer, compressedFrame);

// retrieve your decompressed frame
var decompressedFrame = new ArraySegment<byte>(decompressBuffer, 0, (int) decompressedSize);

```

#### Stream Compression

```csharp
// have somewhere to put the compressed output, any stream will do
using var compressed = new MemoryStream();

// create a compression context over the output stream
using var compressStream = new ZStdCompressStream(compressed);

// ZStdCompressStream.Compressor is just a ZStdCompressor, so you can set parameters and use dictionaries
compressStream.Compressor.Set(CompressionParameter.CompressionLevel, compressionLevel);

// write some data into it
compressStream.Write(someData);

// if you're done writing data and you're not at the end of a frame, it'd be wise to flush
compressStream.Flush();

```

#### Stream Decompression

```csharp
// consume the compressed input stream with the stream decompressor
using var decompressStream = new ZStdDecompressStream(compressedInput);

// you can access ZStdCompressStream.Decompressor, it's a ZStdDecmpressor, to specify a dictionary and such

// decompress some data..
var decompressed = new byte[someDataSize];

decompressStream.Read(decompressed, 0, decompressed.Length);

```

