using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime {
    sealed class Player : MonoBehaviour {
        [SerializeField]
        DeviceSelector selector;

        [SerializeField]
        internal Device heldDevice;
        [SerializeField]
        internal PartSlot selectedSlot;

        public static event Action onDialogueLineAdvanceIntent;

        public static event Action<float> onCameraSwitchIntent;

        public static event Action<Device> onDeviceGrabbed;

        public static event Action<Vector3> onGrabNothing;
        public static event Action<Vector3> onGrabAll;
        public static event Action<Vector3> onGrabPart;
        public static event Action<Vector3> onAttachPart;
        public static event Action<Vector3> onReleasePart;

        public void OnAdvanceDialogueLine(InputValue input) => onDialogueLineAdvanceIntent.Invoke();

        public void OnScrollWheel(InputValue input) {
            onCameraSwitchIntent.Invoke(input.Get<Vector2>().y);
        }

        public void OnClick(InputValue input) {
            if (!input.isPressed) {
                return;
            }

            if (heldDevice) {
                ReleaseDevice();
            } else {
                GrabDevice(selector.outerSelection, false);
            }
        }

        public void OnRightClick(InputValue input) {
            if (!input.isPressed) {
                return;
            }

            if (heldDevice) {
                ReleaseDevice();
            } else {
                GrabDevice(selector.innerSelection, selector.innerSelection != selector.outerSelection);
            }
        }

        void ReleaseDevice() {
            UpdateSelectedSlot();

            if (selectedSlot) {
                heldDevice.transform.parent = selectedSlot.transform;
                heldDevice.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                onAttachPart?.Invoke(heldDevice.transform.position);
            } else {
                heldDevice.transform.parent = null;
                onReleasePart?.Invoke(heldDevice.transform.position);
            }

            heldDevice.isTangible = true;
            heldDevice = default;
        }

        void GrabDevice(Device device, bool isPart) {
            if (device) {
                heldDevice = device.Grab();
                heldDevice.isTangible = false;
                heldDevice.transform.parent = transform;
                heldDevice.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                UpdateHeldDevice();
                onDeviceGrabbed?.Invoke(device);

                if (isPart) {
                    onGrabPart?.Invoke(device.transform.position);
                } else {
                    onGrabAll?.Invoke(device.transform.position);
                }
            } else {
                if (selector.hasSurface) {
                    onGrabNothing?.Invoke(selector.surfacePosition);
                }
            }
        }

        void LateUpdate() {
            UpdateSelectedSlot();
            UpdateHeldDevice();
        }

        void UpdateSelectedSlot() {
            selectedSlot = heldDevice
                ? selector.selectedSlots.FirstOrDefault(s => s.CanFit(heldDevice))
                : default;
        }

        void UpdateHeldDevice() {
            if (heldDevice) {
                heldDevice.transform.position = selector.surfacePosition;
            }
        }
    }
}