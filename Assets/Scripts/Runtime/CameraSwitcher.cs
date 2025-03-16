using Unity.Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField]
    CinemachineCamera[] camerasDownToUp = default;

    uint camIndex = 2;

    protected void Start() {
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
