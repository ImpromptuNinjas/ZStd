using System;
using JetBrains.Annotations;
#if !NETSTANDARD1_4 && !NETSTANDARD1_1
using System.Runtime.Serialization;
using System.Security.Permissions;
#endif

namespace ImpromptuNinjas.ZStd {

  [PublicAPI]
#if !NETSTANDARD1_4 && !NETSTANDARD1_1
  [Serializable]
#endif
  public class ZStdException : Exception {

    public uint Code { get; }

    public ZStdException(string message)
      : base(message)
      => Code = 0;
    public ZStdException(string message, uint code)
      : base(message)
      => Code = code;

#if !NETSTANDARD1_4 && !NETSTANDARD1_1
    protected ZStdException(SerializationInfo info, StreamingContext context)
      : base(info, context)
      => Code = info.GetUInt32(nameof(Code));

    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
      if (info == null)
        throw new ArgumentNullException(nameof(info));

      info.AddValue(nameof(Code), Code);

      base.GetObjectData(info, context);
    }
#endif

  }

}
