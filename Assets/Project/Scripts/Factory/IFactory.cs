using UnityEngine;

namespace WhaleTee.Factory {
  public interface IFactory<in TIn, out TOut> {
    TOut Instantiate(TIn ctx);
    TOut Instantiate(TIn ctx, Vector3 position, Quaternion rotation);
    TOut Instantiate(TIn ctx, Transform parent, Quaternion rotation);
    TOut Instantiate(TIn ctx, Vector3 position, Quaternion rotation, Transform parent);
  }
}