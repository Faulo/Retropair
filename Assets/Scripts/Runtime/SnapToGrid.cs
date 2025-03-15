using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    [ExecuteAlways]
    sealed class SnapToGrid : MonoBehaviour {
        void Update() {
            if (transform.parent) {
                return;
            }

            if (!gameObject.scene.IsValid()) {
                return;
            }

            var position = transform.position;
            GridUtils.SnapToGrid(ref position, GridUtils.WorldToTileSize(GetComponentInChildren<Renderer>().bounds.size.SwizzleXZ()));
            transform.position = position;
        }
    }
}
