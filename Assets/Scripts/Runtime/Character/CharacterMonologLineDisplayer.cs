using UnityEngine;

public sealed class CharacterMonologLineDisplayer : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text textMesh = default;

    void Start() {
        DisplayLine("");
        CharacterMonologPlayer.onLineChanged += DisplayLine;
    }

    void OnDestroy() {
        CharacterMonologPlayer.onLineChanged -= DisplayLine;
    }

    void DisplayLine(string line) {
        textMesh.text = line;
    }
}
