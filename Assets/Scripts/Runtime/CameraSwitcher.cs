using Unity.Cinemachine;
using UnityEngine;

namespace Runtime {
    public class CameraSwitcher : MonoBehaviour {
        [SerializeField]
        CinemachineCamera[] camerasDownToUp = default;

        [SerializeField]
        uint camIndex = 1;

        protected void Start() {
            for (int i = 0; i < camerasDownToUp.Length; i++) {
                camerasDownToUp[i].Priority = i == camIndex ? 10 : 5;
            }

            Runtime.Player.onCameraSwitchIntent += HandleCameraSwitchIntent;
        }

        protected void OnDestroy() {
            Runtime.Player.onCameraSwitchIntent -= HandleCameraSwitchIntent;
        }

        void HandleCameraSwitchIntent(float direction) {
            if (direction > 0) {
                TrySwitchCameraUp();
            }

            if (direction < 0) {
                TrySwitchCameraDown();
            }
        }

        void TrySwitchCameraUp() {
            if (camIndex != camerasDownToUp.Length - 1) {
                camerasDownToUp[camIndex].Priority = 5;
                camIndex++;
                camerasDownToUp[camIndex].Priority = 10;
            }
        }

        void TrySwitchCameraDown() {
            if (camIndex != 0) {
                camerasDownToUp[camIndex].Priority = 5;
                camIndex--;
                camerasDownToUp[camIndex].Priority = 10;
            }
        }
    }
}