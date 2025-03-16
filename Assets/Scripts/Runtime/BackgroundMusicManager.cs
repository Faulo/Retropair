using UnityEngine;

namespace Runtime {
    public class BackgroundMusicManager : MonoBehaviour {
        protected void Awake() {
            DontDestroyOnLoad(gameObject);
        }

        protected void Start() {
            GetComponent<AudioSource>().Play();
        }
    }
}