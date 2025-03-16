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
            UpdateCables();
        }

        void UpdateStatus() {
            status = TVStatus.Nothing;
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
