using System;
using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    sealed class Scanner : MonoBehaviour {
        [SerializeField]
        PartSlot slot;
        [SerializeField]
        ScanStatus status;
        [SerializeField]
        SerializableKeyValuePairs<ScanStatus, Sprite> colors = new();
        [SerializeField]
        SpriteRenderer attachedRenderer;

        internal Action<ScanStatus, Vector3> onStatusChange;

        void OnEnable() {
            slot.onAttachDevice += UpdateStatus;
            slot.onFreeDevice += UpdateStatus;
        }

        void OnDisable() {
            slot.onAttachDevice -= UpdateStatus;
            slot.onFreeDevice -= UpdateStatus;
        }

        void Start() {
            UpdateStatus(slot.attachedDevice);
        }

        void FixedUpdate() {
            UpdateStatus(slot.attachedDevice);
        }

        void UpdateStatus(Device device) {
            var newStatus = CalculateStatus(device);
            if (status != newStatus) {
                status = newStatus;
                onStatusChange?.Invoke(status, slot.transform.position);
            }

            UpdateColor();
        }

        void UpdateStatus() {
            status = ScanStatus.Nothing;
            UpdateColor();
        }

        void UpdateColor() {
            attachedRenderer.sprite = colors[status];
        }

        ScanStatus CalculateStatus(Device attachedDevice) {
            if (!attachedDevice) {
                return ScanStatus.Nothing;
            }

            return attachedDevice.GetComponentsInChildren<Device>().All(d => d.isWorking)
                ? ScanStatus.IsWorking
                : ScanStatus.IsBroken;
        }
    }
}
