#if MSTEST
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ImpromptuNinjas.ZStd.Tests {

  [AttributeUsage(AttributeTargets.Parameter)]
  public class ValueSourceAttribute : Attribute, IEnumerable {

    private MemberInfo _member;

    public ValueSourceAttribute(Type declaringType, string name)
      => _member = declaringType.GetMember(name, BindingFlags.Public | BindingFlags.Static).FirstOrDefault();

    public IEnumerator GetEnumerator() {
      switch (_member) {
        default:
          throw new NotSupportedException(_member.MemberType.ToString());

        case MethodInfo method: {
          var e = (IEnumerable) method.Invoke(null, null);
          return e.GetEnumerator();
        }

        case PropertyInfo property: {
          var e = (IEnumerable) property.GetValue(null);
          return e.GetEnumerator();
        }

        case FieldInfo field: {
          var e = (IEnumerable) field.GetValue(null);
          return e.GetEnumerator();
        }
      }
    }

  }

}
#endif
