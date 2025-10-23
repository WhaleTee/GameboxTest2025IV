using UnityEngine;

namespace WhaleTee.Extensions {
  public static class Vector3Extensions {
    public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null) => new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);

    public static Vector3 With(this Vector2 vector, float? x = null, float? y = null, float? z = null) => new Vector3(x ?? vector.x, y ?? vector.y, z ?? 0);

    public static Vector3 Add(this Vector3 vector, float x = 0, float y = 0, float z = 0) => new Vector3(vector.x + x, vector.y + y, vector.z + z);

    public static bool InRangeOf(this Vector3 current, Vector3 target, float range) => (current - target).sqrMagnitude <= range * range;

    public static Vector3 ComponentDivide(this Vector3 v0, Vector3 v1) {
      return new Vector3(
        v1.x != 0 ? v0.x / v1.x : v0.x,
        v1.y != 0 ? v0.y / v1.y : v0.y,
        v1.z != 0 ? v0.z / v1.z : v0.z
      );
    }

    public static Vector3 ToVector3(this Vector2 v2) => new Vector3(v2.x, 0, v2.y);

    public static Vector3 RandomPointInAnnulus(this Vector3 origin, float minRadius, float maxRadius) {
      var angle = Random.value * Mathf.PI * 2f;
      var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

      // Squaring and then square-rooting radii to ensure uniform distribution within the annulus
      var minRadiusSquared = minRadius * minRadius;
      var maxRadiusSquared = maxRadius * maxRadius;
      var distance = Mathf.Sqrt(Random.value * (maxRadiusSquared - minRadiusSquared) + minRadiusSquared);

      // Converting the 2D direction vector to a 3D position vector
      var position = new Vector3(direction.x, 0, direction.y) * distance;
      return origin + position;
    }

    public static Vector3 Abs(this Vector3 vector) => new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));

    public static Vector2 ToScreenDirection(this Vector3 worldDirection, Vector3 position, Camera camera) {
      return (camera.WorldToScreenPoint(position + worldDirection) - camera.WorldToScreenPoint(position)).normalized;
    }

    public static bool Approximately(this Vector3 vector, Vector3 other, float tolerance) {
      return Mathf.Abs(vector.x - other.x) <= tolerance && Mathf.Abs(vector.y - other.y) <= tolerance && Mathf.Abs(vector.z - other.z) <= tolerance;
    }

    public static bool Approximately(this Vector3 vector, Vector3 other) {
      return Mathf.Approximately(vector.x, other.x) && Mathf.Approximately(vector.y, other.y) && Mathf.Approximately(vector.z, other.z);
    }
  }
}