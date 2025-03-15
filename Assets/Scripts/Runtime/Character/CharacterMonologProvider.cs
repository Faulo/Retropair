using UnityEngine;

using Ink.Runtime;

public class CharacterMonologProvider : MonoBehaviour {

    public TextAsset inkJSON;
    Story story = default;

    public Story GetStory() {
        return story ??= new Story(inkJSON.text);
    }
}
