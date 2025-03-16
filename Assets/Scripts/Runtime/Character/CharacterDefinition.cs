using Ink.Runtime;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    public class CharacterDefinition : MonoBehaviour {

        [Header("Story")]

        public TextAsset inkJSON;
        Story story = default;
        Story highPrioStory = default;

        [Header("Console")]
        [SerializeField]
        internal PartSlot consoleSpawnPoint = default;

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

        public Story GetHighPrioStory() {
            return highPrioStory ??= new Story(inkJSON.text);
        }

        public void ReportIncompletion() {
            consoleSpawnPoint.ReportIncompletion();
        }
    }
}