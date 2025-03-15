using UnityEngine;

namespace Runtime {
    sealed class Scanner : MonoBehaviour {
        [SerializeField]
        PartSlot slot;
        [SerializeField]
        ScanStatus status;

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
        }

        void UpdateStatus() {
            status = ScanStatus.Nothing;
        }

        ScanStatus CalculateStatus(Device attachedDevice) {
            return attachedDevice.isWorking
                ? ScanStatus.IsWorking
                : ScanStatus.IsBroken;
        }
    }
}
