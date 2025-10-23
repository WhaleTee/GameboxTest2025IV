using UnityEngine;
using WhaleTee.Extensions;

public class CameraFollow {
  private readonly CameraFollowSettings settings;
  private Vector3 velocity;

  public CameraFollow(CameraFollowSettings settings) {
    this.settings = settings;
  }

  public void Update() => MoveToTarget();

  private void MoveToTarget() {
    if (settings.FollowTransform.OrNull() == null) return;

    settings.Transform.position = Vector3.SmoothDamp(
      settings.Transform.position,
      settings.FollowTransform.position,
      ref velocity,
      settings.SmoothTime,
      settings.MaxSpeed,
      Time.deltaTime
    );
  }
}