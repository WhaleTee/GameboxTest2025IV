using UnityEngine;
using Object = UnityEngine.Object;

namespace WhaleTee.Factory {
  public class GameObjectFactory : DeactivatedGameObjectFactory {
    public GameObject Instantiate(GameObject prefab) => Instantiate(prefab, Vector3.zero, Quaternion.identity);

    public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation) => Instantiate(prefab, position, rotation, null);

    public GameObject Instantiate(GameObject prefab, Transform parent, Quaternion rotation) => Instantiate(prefab, Vector3.zero, rotation, parent);

    public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent) {
      if (rotation == default) rotation = Quaternion.identity;
      var go = Object.Instantiate(prefab, position, rotation, parent);
      go.name = prefab.name;
      go.transform.localScale = Vector3.one;
      return go;
    }
  }
}