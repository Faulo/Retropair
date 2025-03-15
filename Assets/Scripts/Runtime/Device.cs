using UnityEngine;

namespace Runtime {
    sealed class Device : MonoBehaviour {
        [SerializeField]
        internal bool isTangible = true;
        [SerializeField]
        DeviceRoot root;
        [SerializeField]
        internal DevicePart bounds;
        [SerializeField]
        internal bool isWorking = true;
        [SerializeField]
        internal DeviceId deviceId;
        [SerializeField]
        internal PartId partId;

        void OnValidate() {
            if (!root) {
                return;
            }

            if (!bounds) {
                return;
            }

            deviceId = bounds.deviceId;
            partId = bounds.partId;

            var position = -bounds.pivot;

            if (root.transform.localPosition != position) {
                root.transform.localPosition = position;
            }
        }

        internal bool TryGetPartById(DeviceId deviceId, PartId partId, out Device device) {
            foreach (var d in GetComponentsInChildren<Device>()) {
                if (d.deviceId == deviceId && d.partId == partId) {
                    device = d;
                    return true;
                }
            }

            device = default;
            return false;
        }
    }
}
