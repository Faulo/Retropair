using System.Collections.Generic;
using System.Linq;
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
        [SerializeField]
        internal Vector3 surfacePosition;

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

            var ray = mainCamera.ScreenPointToRay(position);

            (innerSelection, outerSelection) = GetDevices(ray, ref surfacePosition);
        }

        static readonly List<Device> list = new();
        static readonly RaycastHit[] hits = new RaycastHit[8];

        static (Device, Device) GetDevices(in Ray ray, ref Vector3 surfacePosition) {
            (Device, Device) selection = default;

            int count = Physics.RaycastNonAlloc(ray, hits);

            float partDistance = float.MaxValue;
            float surfaceDistance = float.MaxValue;

            for (int i = 0; i < count; i++) {
                var hit = hits[i];
                if (hit.collider.TryGetComponent<DevicePart>(out var part)) {
                    part.GetComponentsInParent(false, list);

                    if (list.Count > 0 && list.All(d => d.isTangible)) {
                        if (partDistance > hit.distance) {
                            partDistance = hit.distance;
                            selection = (list[0], list[^1]);
                        }

                        if (surfaceDistance > hit.distance) {
                            surfaceDistance = hit.distance;
                            surfacePosition = hit.point;
                        }
                    }
                } else {
                    if (surfaceDistance > hit.distance) {
                        surfaceDistance = hit.distance;
                        surfacePosition = hit.point;
                    }
                }
            }

            return selection;
        }
    }
}
