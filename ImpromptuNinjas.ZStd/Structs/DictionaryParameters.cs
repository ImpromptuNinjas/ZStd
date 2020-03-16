using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
  [StructLayout(LayoutKind.Sequential)]
  public struct DictionaryParameters {

    /* optimize for a specific zstd compression level; 0 means default */
    public int CompressionLevel;

    /* Write log to stderr; 0 = none (default); 1 = errors; 2 = progression; 3 = details; 4 = debug; */
    public uint NotificationLevel;

    /* force dictID value; 0 means auto mode (32-bits random value) */
    public uint DictionaryId;

  }

}
