using UnityEngine;
using WhaleTee.Extensions;
using WhaleTee.ObjectPooling;

public class ObjectThrowService {
  private readonly ObjectPoolManager objectPoolManager;

  public ObjectThrowService(ObjectPoolManager objectPoolManager) => this.objectPoolManager = objectPoolManager;

  public void ThrowObject(Transform origin, Vector3 scale, float angle, float force, GameObject prefab) {
    var gameObject = objectPoolManager.SpawnObject(prefab, origin.position, origin.rotation);
    
    gameObject.transform.localScale = scale;
    
    var rb = gameObject.GetOrAdd<Rigidbody>();
    var radians = angle * Mathf.Deg2Rad;
    var originForward = origin.forward;
    var cos = Mathf.Cos(radians);
    var sin = Mathf.Sin(radians);
    var throwDirection = new Vector3(originForward.x * cos, sin, originForward.z * cos).normalized * force;

    rb.AddForce(throwDirection, ForceMode.Impulse);
  }
}