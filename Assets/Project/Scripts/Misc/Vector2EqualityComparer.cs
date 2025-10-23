using System.Collections.Generic;
using UnityEngine;
using WhaleTee.Extensions;

namespace WhaleTee.EqualityComparers {
  public sealed class Vector2EqualityComparer : IEqualityComparer<Vector2> {
    private readonly float tolerance;

    public Vector2EqualityComparer(float tolerance) => this.tolerance = tolerance;

    public bool Equals(Vector2 x, Vector2 y) => x.Approximately(y, tolerance);
    public int GetHashCode(Vector2 obj) => obj.GetHashCode();
  }
}