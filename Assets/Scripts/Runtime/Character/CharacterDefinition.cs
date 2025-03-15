using UnityEngine;

using Ink.Runtime;
using Slothsoft.UnityExtensions;
using MyBox;

public class CharacterDefinition : MonoBehaviour {

    [Header("Story")]

    public TextAsset inkJSON;
    Story story = default;

    [Header("Console")]

    public GameObject consolePrefab = default;
    public GameObject consoleSpawnPoint = default;

    [Header("Moods")]

    public SerializableKeyValuePairs<CharacterMood, Sprite> moodToSpriteMap = default;

    [Header("Minor Animations")]

    public Sprite blinkSprite = default;
    public Sprite sideEyeSprite = default;

    public float blinkProbabilityPerSecond = 0.3f;
    public float sideEyeProbabilityPerSecond = 0.1f;

    public Vector2 blinkTimeRangeInSeconds = new(0.08f, 0.12f);
    public Vector2 sideEyeTimeRangeInSeconds = new(0.5f, 1.3f);

    public Story GetStory() {
        return story ??= new Story(inkJSON.text);
    }
}
