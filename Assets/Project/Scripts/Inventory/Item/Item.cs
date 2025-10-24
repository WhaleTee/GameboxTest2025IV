using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Data")]
public class Item : ScriptableObject {
  [field: SerializeField] public Sprite Sprite { get; private set; }
  [field: SerializeField] public string Name { get; private set; }
  [HideInInspector] public GameObject owner;
}