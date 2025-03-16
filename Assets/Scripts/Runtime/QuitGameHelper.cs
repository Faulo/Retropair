using UnityEngine;

namespace Runtime {
    public class QuitGameHelper : MonoBehaviour {
#if UNITY_WEBPLAYER
    public static string webplayerQuitURL = "https://gregorsoenn.itch.io/retropair";
#endif
        public static void Quit() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
        Application.OpenURL(webplayerQuitURL);
#else
        Application.Quit();
#endif
        }
    }
}
