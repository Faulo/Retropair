using UnityEngine;

namespace Runtime {
    sealed class SFX : MonoBehaviour {
        [SerializeField]
        AudioSource source;
        [SerializeField]
        float minPitch = 0.9f;
        [SerializeField]
        float maxPitch = 1.1f;

        void Awake() {
            source.pitch = Random.Range(minPitch, maxPitch);
        }

        void Update() {
            if (!source.isPlaying) {
                Destroy(gameObject);
            }
        }
    }
}
