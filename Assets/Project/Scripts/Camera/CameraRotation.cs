using UnityEngine;
using WhaleTee.Reactive.Input;

public class CameraRotation {
  private readonly CameraRotationSettings settings;
  private readonly UserInput userInput;

  private float rotationX;
  private float rotationY;

  public CameraRotation(CameraRotationSettings settings, UserInput userInput) {
    this.settings = settings;
    this.userInput = userInput;
  }

  public void Update() => RotateWithInput();

  private void RotateWithInput() {
    var nextRotationX = rotationX - userInput.Look.Value.y * settings.RotationSensitivity;
    rotationX = Mathf.Clamp(nextRotationX, -settings.RotationXLimit, settings.RotationXLimit);
    rotationY += userInput.Look.Value.x * settings.RotationSensitivity;
    settings.CameraTransform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
    settings.TargetOrientationTransform.rotation = Quaternion.Euler(0, rotationY, 0);
  }
}