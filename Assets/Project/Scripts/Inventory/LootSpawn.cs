using UnityEngine;

public class LootSpawn : MonoBehaviour {
  [SerializeField]
  private GameObject lootPrefab;

  public void Destroy() => Instantiate(lootPrefab, transform.position, Quaternion.identity);
}