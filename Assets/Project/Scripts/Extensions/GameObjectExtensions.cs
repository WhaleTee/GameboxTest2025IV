using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WhaleTee.Extensions {
  public static class GameObjectExtensions {
    private const string PATH_DELIMITER = "/";
    
    public static void HideInHierarchy(this GameObject gameObject) => gameObject.hideFlags = HideFlags.HideInHierarchy;

    public static T GetOrAdd<T>(this GameObject gameObject) where T : Component {
      var component = gameObject.GetComponent<T>();
      if (!component) component = gameObject.AddComponent<T>();

      return component;
    }

    public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;

    public static T OrElse<T>(this T obj, T value) where T : Object => obj.OrNull() != null ? obj : value;

    public static void DestroyChildren(this GameObject gameObject) => gameObject.transform.DestroyChildren();

    public static void DestroyChildrenImmediate(this GameObject gameObject) => gameObject.transform.DestroyChildrenImmediate();

    public static void EnableChildren(this GameObject gameObject) => gameObject.transform.EnableChildren();

    public static void DisableChildren(this GameObject gameObject) => gameObject.transform.DisableChildren();

    public static void ResetTransformation(this GameObject gameObject) => gameObject.transform.Reset();

    public static string Path(this GameObject gameObject) => PATH_DELIMITER + string.Join(PATH_DELIMITER, gameObject.GetComponentsInParent<Transform>().Select(t => t.name).Reverse().ToArray());

    public static string PathFull(this GameObject gameObject) => gameObject.Path() + PATH_DELIMITER + gameObject.name;

    public static void SetLayersRecursively(this GameObject gameObject, int layer) {
      gameObject.layer = layer;
      gameObject.transform.ForEveryChild(child => child.gameObject.SetLayersRecursively(layer));
    }

    public static void ReParent(this GameObject gameObject, Transform newParent) => gameObject.transform.SetParent(newParent);
  }
}