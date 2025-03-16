using UnityEngine;

namespace Runtime {
    sealed class Incinerator : MonoBehaviour {
        [SerializeField]
        PartSlot slot;
        [SerializeField]
        float acceleration = 1;
        [SerializeField]
        float destroyThreshold = -1;

        float speed;

        void FixedUpdate() {
            if (slot.hasAttachedDevice) {
                speed += Time.deltaTime * acceleration;
                slot.attachedDevice.transform.localPosition += speed * Time.deltaTime * Vector3.down;

                if (slot.attachedDevice.transform.localPosition.y < destroyThreshold) {
                    Destroy(slot.attachedDevice.gameObject);
                }
            } else {
                speed = 0;
            }
        }
    }
}
