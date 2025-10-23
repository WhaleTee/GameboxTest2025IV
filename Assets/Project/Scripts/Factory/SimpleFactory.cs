using UnityEngine;

namespace WhaleTee.Factory {
  public abstract class SimpleFactory<TIn, TOut> : IFactory<TIn, TOut> {
    public abstract TOut Instantiate(TIn ctx);

    public TOut Instantiate(TIn ctx, Vector3 position, Quaternion rotation) => Instantiate(ctx);

    public TOut Instantiate(TIn ctx, Transform parent, Quaternion rotation) => Instantiate(ctx);

    public TOut Instantiate(TIn ctx, Vector3 position, Quaternion rotation, Transform parent) => Instantiate(ctx);
  }
}