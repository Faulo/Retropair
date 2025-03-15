using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    sealed class DevicePart : MonoBehaviour {
        [SerializeField]
        internal string id;
        [SerializeField]
        internal Bounds bounds;
        [SerializeField]
        internal Vector3 pivot;
        [SerializeField]
        internal Vector2Int tileSize;

        internal void LoadFromBounds(in Bounds bounds) {
            this.bounds = bounds;
            pivot = bounds.center.WithY(bounds.min.y);
            tileSize = GridUtils.WorldToTileSize(bounds.size.SwizzleXZ());
        }

        [ContextMenu(nameof(LoadFromRenderer))]
        internal void LoadFromRenderer() {
            LoadFromBounds(GetComponent<Renderer>().bounds);
        }
    }
}
