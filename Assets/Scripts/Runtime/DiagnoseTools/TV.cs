using System;
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
        [SerializeField]
        GameObject[] cables = Array.Empty<GameObject>();

        void Start() {
            UpdateStatus(slot.attachedDevice);
        }

        void FixedUpdate() {
            UpdateStatus(slot.attachedDevice);
        }

        void UpdateStatus(Device device) {
            status = CalculateStatus(device);
            UpdateColor();
            UpdateCables();
        }

        void UpdateColor() {
            attachedRenderer.material.SetColor("_BaseColor", colors[status]);
        }

        void UpdateCables() {
            foreach (var cable in cables) {
                cable.SetActive(status != TVStatus.Nothing);
            }
        }

        TVStatus CalculateStatus(Device attachedDevice) {
            if (!attachedDevice) {
                return TVStatus.Nothing;
            }

            if (attachedDevice.TryGetPartById(DeviceId.SNES, PartId.Mainboard, out var snes)) {
                bool hasPower = snes.isWorking
                    && snes.TryGetSlotById(DeviceId.SNES, PartId.PowerSwitchMechanism, out var power) && power.isComplete;
                bool hasCartridge = snes.TryGetSlotById(DeviceId.SNES, PartId.CartridgeSlot, out var cartridge) && cartridge.isComplete;

                return (hasPower, hasCartridge) switch {
                    (false, _) => TVStatus.Blue,
                    (true, false) => TVStatus.Black,
                    (true, true) => TVStatus.SNESGame,
                };
            }

            if (attachedDevice.TryGetPartById(DeviceId.N64, PartId.Mainboard, out var n64)) {
                bool hasPower = n64.isWorking
                    && n64.TryGetSlotById(DeviceId.N64, PartId.PowerSwitchMechanism, out var power) && power.isComplete
                    && n64.TryGetSlotById(DeviceId.N64, PartId.ChipCPU, out var cpu) && cpu.isComplete
                    && n64.TryGetSlotById(DeviceId.N64, PartId.ChipRCP, out var rcp) && rcp.isComplete
                    && n64.TryGetSlotById(DeviceId.N64, PartId.ChipRDRAM, out var ram) && ram.isComplete;
                bool hasCartridge = n64.TryGetSlotById(DeviceId.N64, PartId.CartridgeSlot, out var cartridge) && cartridge.isComplete;

                return (hasPower, hasCartridge) switch {
                    (false, _) => TVStatus.Blue,
                    (true, false) => TVStatus.Black,
                    (true, true) => TVStatus.N64Game,
                };
            }

            return TVStatus.Nothing;
        }
    }
}
