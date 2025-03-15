using UnityEngine;

using Ink.Runtime;

public class CharacterMonologProvider : MonoBehaviour {

    public TextAsset inkJSON;
    public Story story;

    private void Start() {
        story = new Story(inkJSON.text);
    }
}
