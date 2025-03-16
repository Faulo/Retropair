using Slothsoft.UnityExtensions;
using UnityEngine;
namespace Runtime {
    sealed class ScannerEffects : MonoBehaviour {

        [SerializeField]
        Scanner scanner;

        [SerializeField]
        SerializableKeyValuePairs<ScanStatus, GameObject> prefabs = new();

        void OnEnable() {
            scanner.onStatusChange += HandleStatus;
        }

        void OnDisable() {
            scanner.onStatusChange -= HandleStatus;
        }

        void HandleStatus(ScanStatus status, Vector3 position) {
            if (prefabs.TryGetValue(status, out var prefab) && prefab) {
                Instantiate(prefab, position, Quaternion.identity);
            }
        }
    }
}