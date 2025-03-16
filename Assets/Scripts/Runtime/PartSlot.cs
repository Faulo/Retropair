using System;
using System.Linq;
using UnityEngine;

namespace Runtime {
    sealed class PartSlot : MonoBehaviour {
        [SerializeField]
        DevicePart referencePart;

        internal DeviceId referenceDeviceId => referencePart.deviceId;
        internal PartId referencePartId => referencePart.partId;

        [SerializeField]
        bool mustFitExactly = true;

        internal bool isWorkingAndCorrect => attachedDevice
            && attachedDevice.isWorking
            && attachedDevice.deviceId == referenceDeviceId
            && attachedDevice.partId == referencePartId;
        internal bool isComplete => GetComponentsInChildren<PartSlot>()
            .All(slot => slot.isWorkingAndCorrect);

        internal Action<Device> onAttachDevice;
        internal Action onFreeDevice;

        void OnValidate() {
            if (!referencePart) {
                return;
            }

            if (transform.localPosition != referencePart.pivot) {
                transform.localPosition = referencePart.pivot;
            }
        }

        internal bool CanFit(Device device) {
            if (mustFitExactly) {
                if (!referencePart.bounds.Approximately(device.bounds.bounds)) {
                    return false;
                }
            } else {
                if (!referencePart.bounds.CanContain(device.bounds.bounds)) {
                    return false;
                }
            }

            return true;
        }

        bool isFree => transform.childCount == 0;

        void OnEnable() {
            UpdateCollider(true);
        }

        void OnDisable() {
            UpdateCollider(true);
        }

        void FixedUpdate() {
            UpdateCollider();
        }

        internal bool hasAttachedDevice => attachedDevice;
        internal Device attachedDevice { get; private set; }

        BoxCollider _collider;

        bool wasFree => _collider;
        void UpdateCollider(bool forceUpdate = false) {
            if (forceUpdate || isFree != wasFree) {
                if (_collider) {
                    Destroy(_collider);
                    _collider = null;
                } else {
                    _collider = gameObject.AddComponent<BoxCollider>();
                    _collider.size = referencePart.bounds.size;
                }

                if (transform.childCount > 0 && transform.GetChild(0).TryGetComponent<Device>(out var d)) {
                    attachedDevice = d;
                    onAttachDevice?.Invoke(attachedDevice);
                } else {
                    attachedDevice = null;
                    onFreeDevice?.Invoke();
                }
            }
        }
    }
}
