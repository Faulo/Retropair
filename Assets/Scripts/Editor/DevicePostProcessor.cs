using Runtime;
using UnityEditor;
using UnityEngine;

namespace Editor {
    sealed class DevicePostProcessor : AssetPostprocessor {
        void OnPostprocessModel(GameObject gameObject) {
            if (!gameObject.name.StartsWith("Device_")) {
                return;
            }

            var device = gameObject.AddComponent<DeviceRoot>();

            foreach (var renderer in gameObject.GetComponentsInChildren<MeshRenderer>()) {
                if (renderer.name.EndsWith("_Modifier")) {
                    renderer.gameObject.SetActive(false);
                } else {
                    if (MaterialStorage.instance) {
                        var materials = renderer.sharedMaterials;
                        for (int i = 0; i < materials.Length; i++) {
                            materials[i] = MaterialStorage.instance.GetOrCreate(materials[i].name);
                        }

                        renderer.sharedMaterials = materials;
                    }

                    var part = renderer.gameObject.AddComponent<DevicePart>();
                    part.id = $"{gameObject.name}.{renderer.name}";
                    part.LoadFromBounds(renderer.bounds);

                    var collider = renderer.gameObject.AddComponent<MeshCollider>();
                    collider.convex = false;
                }
            }
        }
    }
}
