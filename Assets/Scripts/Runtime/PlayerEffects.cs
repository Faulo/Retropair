using UnityEngine;

namespace Runtime {
    sealed class PlayerEffects : MonoBehaviour {

        [SerializeField]
        DeviceSelector selector;

        [SerializeField]
        GameObject clickPrefab;

        [SerializeField]
        GameObject dismantlePrefab;

        [SerializeField]
        GameObject grabPrefab;

        [SerializeField]
        GameObject fixPrefab;

        [SerializeField]
        GameObject releasePrefab;

        void OnEnable() {
            Player.onGrabNothing += HandleNothing;
            Player.onGrabPart += HandleDismantle;
            Player.onGrabAll += HandleGrab;
            Player.onAttachPart += HandleAttachPart;
            Player.onReleasePart += HandleReleasePart;
        }

        void OnDisable() {
            Player.onGrabNothing -= HandleNothing;
            Player.onGrabPart -= HandleDismantle;
            Player.onGrabAll -= HandleGrab;
            Player.onAttachPart -= HandleAttachPart;
            Player.onReleasePart -= HandleReleasePart;
        }

        public void HandleNothing(Vector3 position) {
            if (clickPrefab) {
                Instantiate(clickPrefab, position, Quaternion.identity);
            }
        }

        public void HandleDismantle(Vector3 position) {
            if (dismantlePrefab) {
                Instantiate(dismantlePrefab, position, Quaternion.identity);
            }
        }

        public void HandleGrab(Vector3 position) {
            if (grabPrefab) {
                Instantiate(grabPrefab, position, Quaternion.identity);
            }
        }

        public void HandleAttachPart(Vector3 position) {
            if (fixPrefab) {
                Instantiate(fixPrefab, position, Quaternion.identity);
            }
        }

        public void HandleReleasePart(Vector3 position) {
            if (releasePrefab) {
                Instantiate(releasePrefab, position, Quaternion.identity);
            }
        }
    }
}
