using System.Collections.Generic;
using Slothsoft.UnityExtensions;
using UnityEngine;

public sealed class CharacterMoodSpriteManager : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer = default;
    
    [SerializeField]
    CharacterDefinition character = default;

    CharacterMood currentMood = CharacterMood.Sad;

    public void Start() {
        spriteRenderer.sprite = character.moodToSpriteMap[currentMood];
        CharacterMonologPlayer.onMoodChanged += OnMoodChanged;
    }

    void OnDestroy() {
        CharacterMonologPlayer.onMoodChanged -= OnMoodChanged;
    }

    public void OnMoodChanged(CharacterMood newMood) {
        if (currentMood == newMood) {
            return;
        }
        if (character.moodToSpriteMap.ContainsKey(newMood)) {
            currentMood = newMood;
            spriteRenderer.sprite = character.moodToSpriteMap[currentMood];
        }
    }
}
