using Reflex.Attributes;
using UnityEngine;
using WhaleTee.Reactive.Input;

public class FPCamera : MonoBehaviour {
  [Header("Rotation")]
  [SerializeField]
  private Transform cameraTransform;

  [SerializeField]
  private Transform targetOrientationTransform;

  [SerializeField]
  private float rotationSensitivity;

  [SerializeField]
  private float rotationXLimit;

  [Header("Movement")]
  [SerializeField]
  private Transform followTransform;

  [Inject]
  private UserInput userInput;

  private float rotationX;
  private float rotationY;

  private void LateUpdate() {
    Rotate();
    Move();
  }

  private void Rotate() {
    var nextRotationX = rotationX - userInput.Look.Value.y * rotationSensitivity;
    rotationX = Mathf.Clamp(nextRotationX, -rotationXLimit, rotationXLimit);
    rotationY += userInput.Look.Value.x * rotationSensitivity;
    cameraTransform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
    targetOrientationTransform.rotation = Quaternion.Euler(0, rotationY, 0);
  }

  private void Move() => transform.position = followTransform.position;
}