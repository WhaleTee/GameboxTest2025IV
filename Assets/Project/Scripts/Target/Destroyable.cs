using Reflex.Attributes;
using UnityEngine;
using WhaleTee.Extensions;
using WhaleTee.ObjectPooling;

public class Destroyable : MonoBehaviour {
  [SerializeField]
  private GameObject particlePrefab;

  [SerializeField]
  private LayerMask bulletHitLayer;

  [Inject]
  private ObjectPoolManager objectPoolManager;

  private CapsuleCollider mainCollider;

  private void Awake() => mainCollider = GetComponent<CapsuleCollider>();

  private void FixedUpdate() {
    var results = new Collider[1];

    if (Physics.OverlapCapsuleNonAlloc(
          mainCollider.bounds.min,
          mainCollider.bounds.max,
          mainCollider.radius,
          results,
          bulletHitLayer
        )
        > 0) {
      var result = results[0];
      var bulletHit = result.GetComponent<BulletHit>();

      if (bulletHit.OrNull() != null) {
        var particle = objectPoolManager.SpawnObject(particlePrefab, result.transform, Quaternion.identity, PoolType.ParticleSystems);
        particle.gameObject.GetOrAdd<LifetimeParticle>().Play();
        objectPoolManager.ReturnObjectToPool(bulletHit.gameObject);
      }

      if (TryGetComponent<LootSpawn>(out var lootSpawn)) lootSpawn.Destroy();

      Destroy(gameObject);
    }
  }
}