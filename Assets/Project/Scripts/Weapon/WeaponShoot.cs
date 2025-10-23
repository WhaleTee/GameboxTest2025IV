using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Reflex.Attributes;
using Reflex.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using WhaleTee.Extensions;
using WhaleTee.ObjectPooling;
using WhaleTee.Reactive.Input;

public class WeaponShoot : MonoBehaviour {
  [SerializeField]
  private AudioSource shotFX;

  [SerializeField]
  private bool continuousShooting;

  [SerializeField]
  private int shootsPerSecond;

  [SerializeField]
  private BulletHit bulletHitPrefab;

  [SerializeField]
  private Transform bulletShootPosition;
  
  [SerializeField]
  private GameObject particlePrefab;

  [Inject]
  private UserInput userInput;

  [Inject]
  private ObjectPoolManager objectPoolManager;

  private bool isShooting;
  private CancellationTokenSource cts;

  private void OnEnable() {
    cts = new CancellationTokenSource();

    userInput ??= SceneManager.GetSceneByBuildIndex(0).GetSceneContainer().Resolve<UserInput>();
    objectPoolManager ??= SceneManager.GetSceneByBuildIndex(0).GetSceneContainer().Resolve<ObjectPoolManager>();
    
    userInput.Shoot
             .Where(value => value)
             .Subscribe(value => {
                          if (isShooting) return;

                          if (value) UniTask.Void(ContinuousShooting);
                        }
             )
             .AddTo(cts.Token);
  }

  private void OnDisable() {
    cts.Cancel();
    cts.Dispose();
  }

  private async UniTaskVoid ContinuousShooting() {
    isShooting = userInput.Shoot.Value;

    while (isShooting) {
      SingleShoot();

      await (continuousShooting ? UniTask.WaitForSeconds(1f / shootsPerSecond) : UniTask.WaitUntil(() => !userInput.Shoot.Value));
      isShooting = userInput.Shoot.Value;
    }
  }

  private void SingleShoot() {
    if (Physics.Raycast(
          transform.position,
          transform.forward,
          out var hit,
          Mathf.Infinity,
          ~(1 << gameObject.layer | 1 << bulletHitPrefab.gameObject.layer)
        )) {
      shotFX.Stop();
      shotFX.Play();
      objectPoolManager.SpawnObject(bulletHitPrefab, hit.point, Quaternion.identity);
      var particle = objectPoolManager.SpawnObject(particlePrefab, bulletShootPosition, Quaternion.identity, PoolType.ParticleSystems);
      particle.transform.localScale = new Vector3(.1f, .1f, .1f);
      particle.gameObject.GetOrAdd<LifetimeParticle>().Play();
    }
  }
}