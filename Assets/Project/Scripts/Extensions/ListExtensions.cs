using System.Collections.Generic;

namespace WhaleTee.Extensions {
  public static class ListExtensions {
    public static void RefreshWith<T>(this List<T> list, IEnumerable<T> items) {
      list.Clear();
      list.AddRange(items);
    }

    public static bool IsNullOrEmpty<T>(this IList<T> list) => list == null || list.Count == 0;
  }
}