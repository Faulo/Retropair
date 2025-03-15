using System;
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
                    part.deviceId = Enum.Parse<DeviceId>(gameObject.name.Replace("Device_", ""));
                    part.partId = Enum.Parse<PartId>(renderer.name.Replace("_", ""));
                    part.LoadFromBounds(renderer.bounds);

                    var collider = renderer.gameObject.AddComponent<MeshCollider>();
                    collider.convex = false;
                }
            }
        }
    }
}
