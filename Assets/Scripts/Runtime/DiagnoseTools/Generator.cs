using System;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    sealed class Generator : MonoBehaviour {
        [SerializeField]
        Device device;
        [SerializeField]
        GameObject[] prefabs = Array.Empty<GameObject>();
        [SerializeField]
        float destructionChance = 0.2f;
        [SerializeField]
        float notWorkingChance = 0.5f;

        void Awake() {
            device.spawn = Generate;
        }

        Device Generate() {
            var instance = Instantiate(prefabs.RandomElement());
            var device = instance.GetComponentsInChildren<Device>().RandomElement();

            if (device.gameObject != instance) {
                device.transform.parent = null;
                Destroy(instance);
            }

            foreach (var d in device.GetComponentsInChildren<Device>()) {
                if (!d) {
                    continue;
                }

                if (UnityEngine.Random.value > notWorkingChance) {
                    d.isWorking = false;
                    continue;
                }

                if (d != device && UnityEngine.Random.value > destructionChance) {
                    Destroy(d.gameObject);
                }
            }

            return device;
        }
    }
}
