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

        [SerializeField]
        AudioClip sfxGrab = default;
        [SerializeField]
        AudioClip sfxRelease = default;

        [SerializeField]
        AudioSource sfxSource = default;

        public static event Action onDialogueLineAdvanceIntent;

        public static event Action<float> onCameraSwitchIntent;

        public static event Action<Device> onDeviceGrabbed;

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
            UpdateSelectedSlot();

            if (selectedSlot) {
                heldDevice.transform.parent = selectedSlot.transform;
                heldDevice.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            } else {
                heldDevice.transform.parent = null;
            }

            heldDevice.isTangible = true;
            heldDevice = default;

            sfxSource.resource = sfxRelease;
            sfxSource.Play();
        }

        void GrabDevice(Device device) {
            if (device) {
                heldDevice = device.Grab();
                heldDevice.isTangible = false;
                heldDevice.transform.parent = transform;
                heldDevice.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                UpdateHeldDevice();
                onDeviceGrabbed?.Invoke(device);

                sfxSource.resource = sfxGrab;
                sfxSource.Play();
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