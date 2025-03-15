using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime {
    sealed class Player : MonoBehaviour {
        [SerializeField]
        DeviceSelector selector;

        [SerializeField]
        internal Device heldDevice;

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
            heldDevice.transform.parent = null;
            heldDevice.isTangible = true;
            heldDevice = default;
        }

        void GrabDevice(Device device) {
            if (device) {
                heldDevice = device;
                heldDevice.isTangible = false;
                heldDevice.transform.parent = transform;
                UpdateHeldDevice();
            }
        }

        void FixedUpdate() {
            UpdateHeldDevice();
        }

        void UpdateHeldDevice() {
            if (heldDevice) {
                heldDevice.transform.position = selector.surfacePosition;
            }
        }
    }
}