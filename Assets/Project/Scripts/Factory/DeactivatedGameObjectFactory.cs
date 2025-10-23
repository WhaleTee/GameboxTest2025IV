using System;
using UnityEngine;

namespace WhaleTee.Factory {
  public interface DeactivatedGameObjectFactory : IPrefabFactory<GameObject> {
    private GameObject InstantiateDeactivated(GameObject prefab, Func<GameObject> factory) {
      prefab.SetActive(false);
      var go = factory?.Invoke();
      prefab.SetActive(true);
      return go;
    }

    GameObject InstantiateDeactivated(GameObject prefab) => InstantiateDeactivated(prefab, () => Instantiate(prefab));

    GameObject InstantiateDeactivated(
      GameObject prefab,
      Vector3 position,
      Quaternion rotation
    ) =>
    InstantiateDeactivated(prefab, () => Instantiate(prefab, position, rotation));

    GameObject InstantiateDeactivated(
      GameObject prefab,
      Transform parent,
      Quaternion rotation
    ) =>
    InstantiateDeactivated(prefab, () => Instantiate(prefab, parent, rotation));

    GameObject InstantiateDeactivated(
      GameObject prefab,
      Vector3 position,
      Quaternion rotation,
      Transform parent
    ) =>
    InstantiateDeactivated(prefab, () => Instantiate(prefab, position, rotation, parent));
  }
}