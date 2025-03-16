using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour {
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        GetComponent<AudioSource>().Play();
    }
}
