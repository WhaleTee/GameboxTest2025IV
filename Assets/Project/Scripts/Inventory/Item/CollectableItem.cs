using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollectableItem : MonoBehaviour {
  [field: SerializeField] public Item Item { get; private set; }

  private void OnEnable() => Item.owner = gameObject;
}