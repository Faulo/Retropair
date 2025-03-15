using UnityEngine;

namespace Runtime {
    sealed class Device : MonoBehaviour {
        [SerializeField]
        DevicePart referenceBounds;

        void OnValidate() {
            if (!referenceBounds) {
                return;
            }

            if (!transform.parent) {
                return;
            }

            var position = -referenceBounds.pivot;

            if (transform.localPosition != position) {
                transform.localPosition = position;
            }
        }
    }
}
