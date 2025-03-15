using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    sealed class TV : MonoBehaviour {
        [SerializeField]
        PartSlot slot;
        [SerializeField]
        TVStatus status;
        [SerializeField]
        SerializableKeyValuePairs<TVStatus, Color> colors = new();
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
            status = TVStatus.Nothing;
            UpdateColor();
        }

        void UpdateColor() {
            attachedRenderer.material.SetColor("_BaseColor", colors[status]);
        }

        TVStatus CalculateStatus(Device attachedDevice) {
            if (attachedDevice.TryGetPartById(DeviceId.SNES, PartId.Mainboard, out var mainboard)) {
                bool hasMainboard = mainboard.isWorking;
                bool hasCartridge = mainboard.TryGetPartById(DeviceId.SNES, PartId.CartridgeSlot, out var cartridge) && cartridge.isWorking;
                bool hasPower = mainboard.TryGetPartById(DeviceId.SNES, PartId.PowerSwitchMechanism, out var power) && power.isWorking;

                return (hasMainboard, hasPower, hasCartridge) switch {
                    (false, _, _) => TVStatus.Blue,
                    (true, false, _) => TVStatus.Blue,
                    (true, true, false) => TVStatus.Black,
                    (true, true, true) => TVStatus.SNESGame,
                };
            }

            return TVStatus.Nothing;
        }
    }
}
