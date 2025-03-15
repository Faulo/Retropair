using UnityEngine;

namespace Runtime {
    static class GridUtils {
        internal const int TILES_PER_UNIT = 16;
        internal const float UNITS_PER_TILES = 1f / TILES_PER_UNIT;

        internal static Vector2Int WorldToTileSize(Vector2 worldSize) {
            return Vector2Int.CeilToInt(TILES_PER_UNIT * worldSize);
        }

        internal static void SnapToGrid(ref Vector3 worldPosition, Vector2Int tileSize) {
            if (tileSize.x % 2 == 1) {
                worldPosition.x += UNITS_PER_TILES * 0.5f;
            }

            if (tileSize.y % 2 == 1) {
                worldPosition.z += UNITS_PER_TILES * 0.5f;
            }

            worldPosition *= TILES_PER_UNIT;
            worldPosition.x = Mathf.Round(worldPosition.x);
            worldPosition.z = Mathf.Round(worldPosition.z);
            worldPosition *= UNITS_PER_TILES;

            if (tileSize.x % 2 == 1) {
                worldPosition.x -= UNITS_PER_TILES * 0.5f;
            }

            if (tileSize.y % 2 == 1) {
                worldPosition.z -= UNITS_PER_TILES * 0.5f;
            }
        }
    }
}