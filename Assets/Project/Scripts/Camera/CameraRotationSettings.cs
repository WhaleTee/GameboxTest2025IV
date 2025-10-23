using System;
using UnityEngine;

[Serializable]
public class CameraRotationSettings {
  [field: SerializeField] public Transform CameraTransform { get; private set; }
  [field: SerializeField] public Transform TargetOrientationTransform { get; private set; }
  [field: SerializeField] public float RotationSensitivity { get; private set; }
  [field: SerializeField] public float RotationXLimit { get; private set; }
}