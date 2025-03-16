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

        void UpdateStatus(Device device) {
            status = CalculateStatus(device);
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

            return attachedDevice.isWorking
                ? ScanStatus.IsWorking
                : ScanStatus.IsBroken;
        }
    }
}
