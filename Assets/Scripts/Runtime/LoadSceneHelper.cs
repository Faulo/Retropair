using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime {
    sealed class LoadSceneHelper : MonoBehaviour {

        public void LoadSceneFromName(string sceneName) {
            SceneManager.LoadScene(sceneName);
        }
    }
}
