using System;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    sealed class Generator : MonoBehaviour {
        [SerializeField]
        PartSlot slot;
        [SerializeField]
        GameObject[] prefabs = Array.Empty<GameObject>();

        void FixedUpdate() {
            if (!slot.hasAttachedDevice) {
                var instance = Instantiate(prefabs.RandomElement(), slot.transform);
            }
        }
    }
}
