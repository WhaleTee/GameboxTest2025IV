using Reflex.Core;
using Reflex.Injectors;

namespace WhaleTee.Reflex.Extensions {
  public static class ReflexObjectExtensions {
    public static T InjectAttributes<T>(this T obj, Container container) where T : class {
      AttributeInjector.Inject(obj, container);
      return obj;
    }
  }
}