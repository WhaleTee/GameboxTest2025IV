using UnityEngine;

namespace WhaleTee.Factory {
  public interface IPrefabFactory<out TOut> : IFactory<GameObject, TOut> { }
}