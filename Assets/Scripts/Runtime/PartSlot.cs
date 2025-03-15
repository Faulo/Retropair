using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    sealed class PartSlot : MonoBehaviour {
        [SerializeField]
        DevicePart referencePart;
        [SerializeField]
        bool mustFitExactly = true;

        void OnValidate() {
            if (!referencePart) {
                return;
            }

            if (transform.localPosition != referencePart.pivot) {
                transform.localPosition = referencePart.pivot;
            }
        }

        internal bool isCorrect => transform.TryGetComponentInChildren<DevicePart>(out var part) && referencePart.id == part.id;

        internal bool TryFit(Device device) {
            if (mustFitExactly) {
                if (!referencePart.bounds.Approximately(device.bounds.bounds)) {
                    return false;
                }
            } else {
                if (!referencePart.bounds.CanContain(device.bounds.bounds)) {
                    return false;
                }
            }

            device.transform.parent = transform;
            device.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            return true;
        }

        bool isFree => transform.childCount == 0;

        void FixedUpdate() {
            UpdateCollider();
        }

        BoxCollider _collider;
        bool wasFree => _collider;
        void UpdateCollider() {
            if (isFree != wasFree) {
                if (_collider) {
                    Destroy(_collider);
                } else {
                    _collider = gameObject.AddComponent<BoxCollider>();
                    _collider.size = referencePart.bounds.size;
                }
            }
        }
    }
}
