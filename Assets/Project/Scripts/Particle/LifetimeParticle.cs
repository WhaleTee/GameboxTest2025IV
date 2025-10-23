using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using Reflex.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using WhaleTee.ObjectPooling;

[RequireComponent(typeof(ParticleSystem))]
public class LifetimeParticle : MonoBehaviour {
  [SerializeField]
  [Range(0, 1)]
  private float lifetime;

  private ObjectPoolManager objectPoolManager;

  private ParticleSystem particles;

  private void OnEnable() {
    particles = GetComponent<ParticleSystem>();
    particles.Stop();
    objectPoolManager = SceneManager.GetSceneByBuildIndex(0).GetSceneContainer().Resolve<ObjectPoolManager>();
  }

  public void Play() {
    particles.Play();
    UniTask.Void(WaitAndDestroy);
  }

  private async UniTaskVoid WaitAndDestroy() {
    await UniTask.WaitForSeconds(lifetime);
    particles.Stop();
    objectPoolManager.ReturnObjectToPool(gameObject, PoolType.ParticleSystems);
  }
}