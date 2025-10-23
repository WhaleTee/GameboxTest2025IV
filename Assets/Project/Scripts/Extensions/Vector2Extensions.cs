using UnityEngine;

namespace WhaleTee.Extensions {
  public static class Vector2Extensions {
    public static Vector2 Abs(this Vector2 vector) => new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));

    public static Vector2 ToScreenDirection(this Vector2 worldDirection, Vector2 position, Camera camera) {
      return camera.WorldToScreenPoint(position + worldDirection) - camera.WorldToScreenPoint(position);
    }

    public static bool Approximately(this Vector2 vector, Vector2 other, float tolerance) {
      return Mathf.Abs(vector.x - other.x) <= tolerance && Mathf.Abs(vector.y - other.y) <= tolerance;
    }

    public static bool Approximately(this Vector2 vector, Vector2 other) => Mathf.Approximately(vector.x, other.x) && Mathf.Approximately(vector.y, other.y);
  }
}