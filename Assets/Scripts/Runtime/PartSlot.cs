using UnityEngine;

namespace Runtime {
    sealed class PartSlot : MonoBehaviour {
        [SerializeField]
        Transform referencePart;

        void OnValidate() {
            if (referencePart is { gameObject: { activeSelf: true } }) {
                transform.localPosition = transform.parent.position - referencePart.position;
            }
        }
    }
}
