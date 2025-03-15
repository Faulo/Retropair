using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    [ExecuteAlways]
    sealed class EditorSnapToGrid : MonoBehaviour {
        void Update() {
            if (Application.isPlaying) {
                return;
            }

            if (transform.parent) {
                return;
            }

            if (!gameObject.scene.IsValid()) {
                return;
            }

            var renderer = GetComponentInChildren<Renderer>();

            if (!renderer) {
                return;
            }

            var position = transform.position;
            GridUtils.SnapToGrid(ref position, GridUtils.WorldToTileSize(renderer.bounds.size.SwizzleXZ()));
            transform.position = position;
        }
    }
}
