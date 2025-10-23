using System;
using UnityEngine;

[Serializable]
public class CameraFollowSettings {
  [field: SerializeField] public Transform Transform { get; private set; }
  [field: SerializeField] public Transform FollowTransform { get; private set; }
  [field: SerializeField] public float SmoothTime { get; private set; }
  [field: SerializeField] public float MaxSpeed { get; private set; }
}