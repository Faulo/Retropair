using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime {
    sealed class DeviceSelector : MonoBehaviour {
        [SerializeField]
        Camera mainCamera;
        [SerializeField]
        internal Device innerSelection;
        [SerializeField]
        internal Device outerSelection;

        void Update() {
            if (!mainCamera) {
                mainCamera = Camera.main;
            }

            if (!mainCamera) {
                return;
            }

            if (Pointer.current is not { position: { value: Vector2 position } }) {
                return;
            }

            (innerSelection, outerSelection) = GetDevices(mainCamera.ScreenPointToRay(position));
        }

        static readonly List<Device> list = new();
        static readonly RaycastHit[] hits = new RaycastHit[8];

        static (Device, Device) GetDevices(in Ray ray) {
            (Device, Device) selection = default;

            int count = Physics.RaycastNonAlloc(ray, hits);

            float distance = float.MaxValue;

            for (int i = 0; i < count; i++) {
                var hit = hits[i];
                if (distance > hit.distance && hit.collider.TryGetComponent<DevicePart>(out var part)) {
                    part.GetComponentsInParent(false, list);

                    if (list.Count > 0) {
                        distance = hit.distance;
                        selection = (list[0], list[^1]);
                    }
                }
            }

            return selection;
        }
    }
}
