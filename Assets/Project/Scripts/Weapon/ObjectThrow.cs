using UnityEngine;
using WhaleTee.Extensions;

public class ObjectThrow {
  public void ThrowObject(Transform origin, float angle, float force, GameObject prefab) {
    var gameObject = Object.Instantiate(prefab, origin.position, origin.rotation);
    var rb = gameObject.GetOrAdd<Rigidbody>();
    var radians = angle * Mathf.Deg2Rad;
    var originForward = origin.forward;

    rb.AddForce(
      Vector3.zero.With(originForward.x * Mathf.Cos(radians), Mathf.Sin(radians), originForward.z * Mathf.Cos(radians)).normalized * force,
      ForceMode.Impulse
    );
  }
}