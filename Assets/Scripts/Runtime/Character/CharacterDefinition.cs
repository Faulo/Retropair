using UnityEngine;

using Ink.Runtime;
using Slothsoft.UnityExtensions;

public class CharacterDefinition : MonoBehaviour {

    public TextAsset inkJSON;
    Story story = default;

    public SerializableKeyValuePairs<CharacterMood, Sprite> moodToSpriteMap = default;

    public GameObject consolePrefab = default;

    public GameObject consoleSpawnPoint = default;

    public Story GetStory() {
        return story ??= new Story(inkJSON.text);
    }
}
