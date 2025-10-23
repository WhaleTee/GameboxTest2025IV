using Cysharp.Threading.Tasks;
using Reflex.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using WhaleTee.ObjectPooling;

public class BulletHit : MonoBehaviour {
  [SerializeField]
  private float destructionTime = 10f;

  private ObjectPoolManager objectPoolManager;

  private async void OnEnable() {
    transform.localScale = new Vector3(.1f, .1f, .1f);
    objectPoolManager = SceneManager.GetSceneByBuildIndex(0).GetSceneContainer().Resolve<ObjectPoolManager>();
    await UniTask.WaitForSeconds(destructionTime);
    objectPoolManager.ReturnObjectToPool(gameObject);
  }
}