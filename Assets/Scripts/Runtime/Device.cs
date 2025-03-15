using UnityEngine;

namespace Runtime {
    sealed class Device : MonoBehaviour {
        [SerializeField]
        internal bool isTangible = true;
        [SerializeField]
        DeviceRoot root;
        [SerializeField]
        internal DevicePart bounds;

        void OnValidate() {
            if (!root) {
                return;
            }

            if (!bounds) {
                return;
            }

            var position = -bounds.pivot;

            if (root.transform.localPosition != position) {
                root.transform.localPosition = position;
            }
        }
    }
}
