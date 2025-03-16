using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime {
    sealed class DeviceSelector : MonoBehaviour {
        [SerializeField]
        internal Camera mainCamera;
        [SerializeField]
        internal Device innerSelection;
        [SerializeField]
        internal Device outerSelection;
        [SerializeField]
        internal Vector3 surfacePosition;
        [SerializeField]
        internal List<PartSlot> selectedSlots = new();
        [SerializeField]
        internal float maxReach = 5;

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

            GetDevices(ray);
        }

        static readonly List<Device> list = new();
        static readonly RaycastHit[] hits = new RaycastHit[32];

        void GetDevices(in Ray ray) {
            innerSelection = default;
            outerSelection = default;

            var slots = new SortedList<float, PartSlot>();

            int count = Physics.RaycastNonAlloc(ray, hits);

            float partDistance = float.MaxValue;
            float surfaceDistance = float.MaxValue;

            for (int i = 0; i < count; i++) {
                var hit = hits[i];
                hit.collider.GetComponentsInParent(false, list);
                if (list.Any(d => !d.isTangible)) {
                    continue;
                }

                if (hit.collider.TryGetComponent<DevicePart>(out var part)) {
                    if (list.Count > 0) {
                        if (partDistance > hit.distance) {
                            partDistance = hit.distance;
                            innerSelection = list[0];
                            outerSelection = list[^1];
                        }

                        if (surfaceDistance > hit.distance) {
                            surfaceDistance = hit.distance;
                            surfacePosition = hit.point;
                        }
                    }
                } else {
                    if (hit.collider.TryGetComponent<PartSlot>(out var slot)) {
                        slots.Add(hit.distance, slot);
                    } else {
                        if (surfaceDistance > hit.distance) {
                            surfaceDistance = hit.distance;
                            surfacePosition = hit.point;
                        }
                    }
                }
            }

            selectedSlots.Clear();
            selectedSlots.AddRange(slots.Where(slot => slot.Key < surfaceDistance).Select(slot => slot.Value));
        }
    }
}
