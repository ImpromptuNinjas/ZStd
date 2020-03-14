#if MSTEST
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImpromptuNinjas.ZStd.Tests {

  [AttributeUsage(AttributeTargets.Method)]
  public class UseParameterValuesAttribute : Attribute, ITestDataSource {

    public UseParameterValuesAttribute() {
    }

    public IEnumerable<object[]> GetData(MethodInfo methodInfo) {
      var paramInfos = methodInfo.GetParameters();
      var sources = new IEnumerable[paramInfos.Length];
      for (var i = 0; i < paramInfos.Length; i++) {
        var pi = paramInfos[i];
        if (pi.HasDefaultValue) {
          sources[i] = new[] {pi.DefaultValue};
          continue;
        }

        var values = pi.GetCustomAttribute<ValuesAttribute>();

        if (values != null) {
          sources[i] = values;
          continue;
        }

        var valueSource = pi.GetCustomAttribute<ValueSourceAttribute>();

        if (valueSource != null) {
          sources[i] = valueSource;
          continue;
        }

        throw new NotImplementedException();
      }

      var testCases = new LinkedList<object[]>();
      var enumerators = new IEnumerator[sources.Length];

      var index = -1;
      for (;;) {
        while (++index < sources.Length) {
          enumerators[index] = sources[index].GetEnumerator();
          if (!enumerators[index].MoveNext())
            return testCases;
        }

        var testParams = new object[sources.Length];

        for (var i = 0; i < sources.Length; i++)
          testParams[i] = enumerators[i].Current;

        testCases.AddLast(testParams);

        index = sources.Length;

        while (--index >= 0 && !enumerators[index].MoveNext()) ;

        if (index < 0) break;
      }

      return testCases;
    }

    public string GetDisplayName(MethodInfo methodInfo, object[] data)
      => methodInfo.Name + "("+string.Join(", ",data)+")";

  }

}
#endif
