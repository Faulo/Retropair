using Runtime;
using Slothsoft.UnityExtensions;
using UnityEditor;
using UnityEngine;

namespace Editor {
    sealed class DevicePostProcessor : AssetPostprocessor {
        void OnPostprocessModel(GameObject gameObject) {
            if (!gameObject.name.StartsWith("Device_")) {
                return;
            }

            var device = gameObject.AddComponent<Device>();

            foreach (var renderer in gameObject.GetComponentsInChildren<MeshRenderer>()) {
                if (renderer.name.EndsWith("_Modifier")) {
                    renderer.gameObject.SetActive(false);
                } else {
                    var bounds = renderer.bounds;

                    var part = renderer.gameObject.AddComponent<DevicePart>();
                    part.bounds = bounds;
                    part.pivot = bounds.center.WithY(bounds.min.y);
                    part.tileSize = GridUtils.CeilToGrid(bounds.size.SwizzleXZ());
                }
            }
        }
    }
}
