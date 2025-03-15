using System.Collections.Generic;
using Slothsoft.UnityExtensions;
using UnityEngine;

public sealed class CharacterMoodSpriteManager : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer = default;

    [SerializeField]
    SerializableKeyValuePairs<CharacterMood, Sprite> moodToSpriteMap = default;

    CharacterMood currentMood = CharacterMood.Sad;

    public void Start() {
        spriteRenderer.sprite = moodToSpriteMap[currentMood]; 
    }

    public void OnMoodChanged(CharacterMood newMood) {
        if (currentMood == newMood) {
            return;
        }
        if (moodToSpriteMap.ContainsKey(newMood)) {
            currentMood = newMood;
            spriteRenderer.sprite = moodToSpriteMap[currentMood];
        }
    }
}
