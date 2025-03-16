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

        [SerializeField]
        AudioClip sfxSuccess = default;
        [SerializeField]
        AudioClip sfxFail = default;

        [SerializeField]
        AudioSource sfxSource = default;

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
            ScanStatus previousStatus = status;
            status = CalculateStatus(device);

            UpdateColor();

            if (status == ScanStatus.IsWorking && status != previousStatus) {
                sfxSource.resource = sfxSuccess;
                sfxSource.Play();
            }
            if (status == ScanStatus.IsBroken && status != previousStatus) {
                sfxSource.resource = sfxFail;
                sfxSource.Play();
            }
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
