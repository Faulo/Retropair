using UnityEngine;

namespace Runtime {
    static class Extensions {
        internal static bool Approximately(this in Vector3 a, in Vector3 b) {
            return Mathf.Approximately(a.x, b.x)
                || Mathf.Approximately(a.y, b.y)
                || Mathf.Approximately(a.z, b.z);
        }

        internal static bool Approximately(this in Bounds bounds, in Bounds other) {
            return bounds.size.Approximately(other.size);
        }

        internal static bool CanContain(this in Bounds bounds, in Bounds other) {
            return bounds.Approximately(other)
                || (bounds.size.x > other.size.x && bounds.size.y > other.size.y && bounds.size.z > other.size.z);
        }
    }
}
