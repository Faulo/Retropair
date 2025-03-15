using UnityEngine;

namespace Runtime {
    static class GridUtils {
        internal const int TILES_PER_UNIT = 16;
        internal const float UNITS_PER_TILES = 1f / TILES_PER_UNIT;

        internal static Vector2Int CeilToGrid(Vector2 worldPosition) {
            return Vector2Int.CeilToInt(TILES_PER_UNIT * worldPosition);
        }

        internal static void SnapToGrid(ref Vector2 worldPosition) {
            worldPosition *= TILES_PER_UNIT;
            worldPosition.x = Mathf.Round(worldPosition.x);
            worldPosition.y = Mathf.Round(worldPosition.y);
            worldPosition *= UNITS_PER_TILES;
        }
    }
}