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
            if (attachedDevice.TryGetPartById(DeviceId.SNES, PartId.Mainboard, out var snes)) {
                bool hasMainboard = snes.isWorking;
                bool hasCartridge = snes.TryGetSlotById(DeviceId.SNES, PartId.CartridgeSlot, out var cartridge) && cartridge.isWorkingAndCorrect;
                bool hasPower = snes.TryGetSlotById(DeviceId.SNES, PartId.PowerSwitchMechanism, out var power) && power.isWorkingAndCorrect;

                return (hasMainboard, hasPower, hasCartridge) switch {
                    (false, _, _) => TVStatus.Blue,
                    (true, false, _) => TVStatus.Blue,
                    (true, true, false) => TVStatus.Black,
                    (true, true, true) => TVStatus.SNESGame,
                };
            }

            if (attachedDevice.TryGetPartById(DeviceId.N64, PartId.Mainboard, out var n64)) {
                bool hasMainboard = n64.isWorking;
                bool hasCartridge = n64.TryGetSlotById(DeviceId.N64, PartId.CartridgeSlot, out var cartridge) && cartridge.isWorkingAndCorrect;
                bool hasPower = n64.TryGetSlotById(DeviceId.N64, PartId.PowerSwitchMechanism, out var power) && power.isWorkingAndCorrect;

                return (hasMainboard, hasPower, hasCartridge) switch {
                    (false, _, _) => TVStatus.Blue,
                    (true, false, _) => TVStatus.Blue,
                    (true, true, false) => TVStatus.Black,
                    (true, true, true) => TVStatus.N64Game,
                };
            }

            return TVStatus.Nothing;
        }
    }
}
