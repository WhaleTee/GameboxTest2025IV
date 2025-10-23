using UnityEngine;

public class CharacterGroundMovement {
  private const float SPEED_MULTIPLIER = 10f;
  private readonly Transform orientation;
  private readonly GameObject player;
  private readonly Rigidbody rigidbody;
  
  public void MovePlayer(Vector2 direction, float speed) {
    var orientationTransform = orientation.transform;
    var movement = orientationTransform.right * direction.x + orientationTransform.forward * direction.y;
    rigidbody.AddForce(movement.normalized * (speed * SPEED_MULTIPLIER), ForceMode.Force);
  }
}