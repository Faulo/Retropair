using System.Collections.Generic;
using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    [CreateAssetMenu]
    sealed class MaterialStorage : ScriptableObject {
#if UNITY_EDITOR
        internal static MaterialStorage instance => _instance
            ? _instance
            : _instance = UnityEditor.AssetDatabase
                .FindAssets($"t:{nameof(MaterialStorage)}")
                .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
                .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<MaterialStorage>)
                .FirstOrDefault();

        static MaterialStorage _instance;

        [SerializeField]
        Material defaultMaterial;
        [SerializeField]
        SerializableKeyValuePairs<string, Material> _storage = new();

        internal Material GetOrCreate(string name) {
            if (_storage.TryGetValue(name, out var material)) {
                return material;
            }

            var newStorage = new Dictionary<string, Material>(_storage) {
                [name] = defaultMaterial
            };
            _storage.SetItems(newStorage);
            UnityEditor.EditorUtility.SetDirty(this);

            return defaultMaterial;
        }

        [ContextMenu(nameof(AutoAssign))]
        public void AutoAssign() {
            var allMaterials = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(Material)}")
                .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
                .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<Material>)
                .Where(m => m.name.StartsWith("MAT_"))
                .ToList();

            var newStorage = new Dictionary<string, Material>(_storage);
            bool hasChanged = false;
            foreach (string name in _storage.Keys) {
                var material = allMaterials.FirstOrDefault(m => m.name == name);
                if (material) {
                    if (newStorage[name] != material) {
                        newStorage[name] = material;
                        hasChanged = true;
                    }
                }
            }

            if (hasChanged) {
                _storage.SetItems(newStorage);
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}
