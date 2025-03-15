using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    sealed class Scanner : MonoBehaviour {
        [SerializeField]
        PartSlot slot;
        [SerializeField]
        ScanStatus status;
        [SerializeField]
        SerializableKeyValuePairs<ScanStatus, Color> colors = new();
        [SerializeField]
        Renderer attachedRenderer;

        void OnEnable() {
            slot.onAttachDevice += UpdateStatus;
            slot.onFreeDevice += UpdateStatus;
        }
        void OnDisable() {
            slot.onAttachDevice -= UpdateStatus;
            slot.onFreeDevice -= UpdateStatus;
        }

        void UpdateStatus(Device device) {
            status = CalculateStatus(device);
            UpdateColor();
        }

        void UpdateStatus() {
            status = ScanStatus.Nothing;
            UpdateColor();
        }

        void UpdateColor() {
            attachedRenderer.material.SetColor("_BaseColor", colors[status]);
        }

        ScanStatus CalculateStatus(Device attachedDevice) {
            return attachedDevice.isWorking
                ? ScanStatus.IsWorking
                : ScanStatus.IsBroken;
        }
    }
}
