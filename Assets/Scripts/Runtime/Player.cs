using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime {
    sealed class Player : MonoBehaviour {
        [SerializeField]
        DeviceSelector selector;

        [SerializeField]
        internal Device heldDevice;

        public static event Action onDialogueLineAdvanceIntent;

        public void OnAdvanceDialogueLine(InputValue input) {
            onDialogueLineAdvanceIntent.Invoke();
        }

        public void OnClick(InputValue input) {
            if (!input.isPressed) {
                return;
            }

            if (heldDevice) {
                ReleaseDevice();
            } else {
                GrabDevice(selector.outerSelection);
            }
        }

        public void OnRightClick(InputValue input) {
            if (!input.isPressed) {
                return;
            }

            if (heldDevice) {
                ReleaseDevice();
            } else {
                GrabDevice(selector.innerSelection);
            }
        }

        void ReleaseDevice() {
            if (selector.selectedSlots.Count == 0) {
                heldDevice.transform.parent = null;
                heldDevice.isTangible = true;
                heldDevice = default;
            } else {
                foreach (var slot in selector.selectedSlots) {
                    if (slot.CanFit(heldDevice)) {
                        heldDevice.transform.parent = slot.transform;
                        heldDevice.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                        heldDevice.isTangible = true;
                        heldDevice = default;
                        return;
                    }
                }
            }
        }

        void GrabDevice(Device device) {
            if (device) {
                heldDevice = device;
                heldDevice.isTangible = false;
                heldDevice.transform.parent = transform;
                heldDevice.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                UpdateHeldDevice();
            }
        }

        void LateUpdate() {
            UpdateHeldDevice();
        }

        void UpdateHeldDevice() {
            if (heldDevice) {
                heldDevice.transform.position = selector.surfacePosition;
            }
        }
    }
}