#if MSTEST
using System;
using System.Collections;
using System.Collections.Generic;

namespace ImpromptuNinjas.ZStd.Tests {

  [AttributeUsage(AttributeTargets.Parameter)]
  public class ValuesAttribute : Attribute, IEnumerable<object> {

    private readonly object[] _data;

    public ValuesAttribute(params object[] data)
      => _data = data;

    public IEnumerator<object> GetEnumerator()
      => ((IEnumerable<object>) _data).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
      => _data.GetEnumerator();

  }

}
#endif
