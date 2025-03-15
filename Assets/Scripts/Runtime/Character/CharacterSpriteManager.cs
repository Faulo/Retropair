using System.Collections;
using UnityEngine;

public sealed class CharacterSpriteManager : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer = default;
    
    [SerializeField]
    CharacterDefinition character = default;

    CharacterMood currentMood = CharacterMood.Initial;

    bool isSideEying = false;
    bool isBlinking = false;

    Coroutine minorAnimCoroutine = default;

    public void Start() {
        ChangeMood(currentMood);
        CharacterVisitDirector.onMoodChanged += OnMoodChanged;
    }

    void OnDestroy() {
        CharacterVisitDirector.onMoodChanged -= OnMoodChanged;
    }

    void Update() {
        if (isSideEying || isBlinking) {
            return;
        }

        float sideEyeProbabilityThisFrame = Time.deltaTime * character.sideEyeProbabilityPerSecond;
        if (Random.value < sideEyeProbabilityThisFrame) {
            minorAnimCoroutine = StartCoroutine(DoMinorAnim(character.sideEyeSprite, character.sideEyeTimeRangeInSeconds));
            return;
        }

        float blinkProbabilityThisFrame = Time.deltaTime * character.blinkProbabilityPerSecond;
        if (Random.value < blinkProbabilityThisFrame) {
            minorAnimCoroutine = StartCoroutine(DoMinorAnim(character.blinkSprite, character.blinkTimeRangeInSeconds));
            return;
        }
    }

    public void OnMoodChanged(CharacterMood newMood) {
        if (currentMood == newMood) {
            return;
        }
        ChangeMood(newMood);
    }

    void ChangeMood(CharacterMood newMood) {
        if (character.moodToSpriteMap.ContainsKey(newMood)) {

            if (isSideEying || isBlinking || minorAnimCoroutine != null) {
                StopCoroutine(minorAnimCoroutine);
                isSideEying = isBlinking = false;
            }
            currentMood = newMood;
            spriteRenderer.sprite = character.moodToSpriteMap[currentMood];
        }
    }

    IEnumerator DoMinorAnim(Sprite sprite, Vector2 timeRange) {
        spriteRenderer.sprite = sprite;
        yield return new WaitForSeconds(Random.Range(timeRange.x, timeRange.y));
        ChangeMood(currentMood);
    }
}
