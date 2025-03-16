using System;
using System.Linq;
using UnityEngine;

namespace Runtime {
    sealed class PartSlot : MonoBehaviour {
        [SerializeField]
        DevicePart referencePart;
        [SerializeField]
        internal string displayName = "Slot";
        [SerializeField]
        internal Color color = Color.white;

        internal DeviceId referenceDeviceId => referencePart.deviceId;
        internal PartId referencePartId => referencePart.partId;

        [SerializeField]
        bool mustFitExactly = true;
        [SerializeField]
        bool mustBeFilledForCustomer = true;

        internal bool isWorkingAndCorrect => attachedDevice
            && attachedDevice.isWorking
            && attachedDevice.deviceId == referenceDeviceId
            && attachedDevice.partId == referencePartId;
        internal bool isComplete => GetComponentsInChildren<PartSlot>()
            .All(slot => slot.isWorkingAndCorrect);
        internal bool isCustomerComplete => GetComponentsInChildren<PartSlot>()
            .All(slot => !slot.mustBeFilledForCustomer || slot.isWorkingAndCorrect);

        internal Action<Device> onAttachDevice;
        internal Action onFreeDevice;

#if UNITY_EDITOR
        void OnValidate() {
            if (!referencePart) {
                return;
            }

            if (displayName == "Slot") {
                displayName = "Attach";
                UnityEditor.EditorUtility.SetDirty(this);
            }

            if (transform.localPosition != referencePart.pivot) {
                transform.localPosition = referencePart.pivot;
            }

            string name = $"Slot_{referencePart.name}";
            if (gameObject.name != name) {
                gameObject.name = name;
                UnityEditor.EditorUtility.SetDirty(gameObject);
            }
        }
#endif

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

        void OnEnable() {
            UpdateCollider(true);
        }

        void OnDisable() {
            UpdateCollider(true);
        }

        void FixedUpdate() {
            UpdateCollider();
        }

        internal bool hasAttachedDevice => transform.childCount != 0;
        internal Device attachedDevice => hasAttachedDevice
            ? transform.GetChild(0).GetComponent<Device>()
            : null;
        bool hadAttachedDevice;

        BoxCollider _collider;
        bool hasCollider => _collider;

        void UpdateCollider(bool forceUpdate = false) {
            if (forceUpdate || hadAttachedDevice != hasAttachedDevice) {
                if (hasAttachedDevice) {
                    if (hasCollider) {
                        Destroy(_collider);
                        _collider = null;
                    }
                } else {
                    if (!hasCollider) {
                        _collider = gameObject.AddComponent<BoxCollider>();
                        _collider.size = referencePart.bounds.size;
                    }
                }

                if (hasAttachedDevice) {
                    onAttachDevice?.Invoke(attachedDevice);
                } else {
                    onFreeDevice?.Invoke();
                }

                hadAttachedDevice = hasAttachedDevice;
            }
        }

        internal void ReportIncompletion() {
            string report = name + "\n";
            foreach (var slot in GetComponentsInChildren<PartSlot>()) {
                report += isWorkingAndCorrect + " " + slot.name + " [ "
                        + " working " + slot.attachedDevice.isWorking
                        + " | deviceID match " + attachedDevice.deviceId + " " + (attachedDevice.deviceId == referenceDeviceId ? "==" : "!=") + " " + referenceDeviceId
                        + " | partID match " + attachedDevice.partId + " " + (attachedDevice.partId == referencePartId ? "==" : "!=") + " " + referencePartId
                        + " ]\n";
            }

            Debug.Log(report);
        }
    }
}
