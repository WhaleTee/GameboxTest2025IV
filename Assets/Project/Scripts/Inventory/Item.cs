using UnityEngine;

public interface Item {
  Sprite Sprite { get; }
  string Name { get; }
  GameObject RuntimeObject { get; }
}