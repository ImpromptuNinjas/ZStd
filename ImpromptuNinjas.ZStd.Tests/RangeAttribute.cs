#if MSTEST
using System;
using System.Collections;
using System.Collections.Generic;

namespace ImpromptuNinjas.ZStd.Tests {

  [AttributeUsage(AttributeTargets.Parameter)]
  public class RangeAttribute : Attribute, IEnumerable<object> {

    private readonly int _min;

    private readonly int _max;

    private readonly int _step;

    public RangeAttribute(int min, int max, int step) {
      _min = min;
      _max = max;
      _step = step;
    }

    public IEnumerator<object> GetEnumerator() {
      for (var i = _min; i < _max; i += _step)
        yield return i;
    }

    IEnumerator IEnumerable.GetEnumerator()
      => GetEnumerator();
  }

}
#endif
