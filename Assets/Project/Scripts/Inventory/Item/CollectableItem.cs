using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollectableItem : MonoBehaviour, Item {
  [field: SerializeField] public Sprite Sprite { get; private set; }
  [field: SerializeField] public string Name { get; private set; }
  
  public GameObject RuntimeObject { get; private set; }

  private void Awake() {
    RuntimeObject = gameObject;
  }
}